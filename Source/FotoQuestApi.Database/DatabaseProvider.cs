using Dapper;
using FotoQuestApi.Model;
using FotoQuestApi.Model.Exceptions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace FotoQuestApi.Database;

public class DatabaseProvider : IDatabaseProvider
{
    private readonly string connectionString;

    public DatabaseProvider(IConfiguration configuration)
    {
        this.connectionString = configuration.GetConnectionString("DbConnectionString");
    }

    public async Task<bool> CheckConnection()
    {
        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                await connection.CloseAsync();
                return true;
            }
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    public async Task<FileData> LoadImageData(IDataRequest dataRequest)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            var fileId = (dataRequest as GetImageDataRequest).Id;
            var procedure = dataRequest.Command;
            var values = new { arg_Id = fileId };
            FileData fileData = null;
            try
            {
                fileData = await connection.QuerySingleAsync<FileData>(procedure, values, commandType: dataRequest.CommandType);
            }
            catch (System.InvalidOperationException)
            {
                connection.Close();
                throw new NotFoundException($"Image not found for provided Id : {fileId}");
            }
            connection.Close();
            return fileData;
        }
    }

    public async Task<int> SaveImageData(IDataRequest data)
    {
        int result = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (MySqlCommand cmd = new MySqlCommand(data.Command, connection))
            {
                cmd.CommandType = data.CommandType;
                if (data is StoreImageDataRequest imageData)
                {
                    cmd.Parameters.AddWithValue("Id", imageData.Id);
                    cmd.Parameters.AddWithValue("OriginalName", imageData.OriginalName);
                    cmd.Parameters.AddWithValue("DateOfUpload", imageData.DateOfUpload);
                    cmd.Parameters.AddWithValue("HorizontalResolution", imageData.HorizontalResolution);
                    cmd.Parameters.AddWithValue("VerticalResolution", imageData.VerticalResolution);
                    cmd.Parameters.AddWithValue("Height", imageData.Height);
                    cmd.Parameters.AddWithValue("Width", imageData.Width);
                    cmd.Parameters.AddWithValue("FilePath", imageData.FilePath);
                    cmd.Parameters.AddWithValue("FileName", imageData.FileName);

                }
                result = await cmd.ExecuteNonQueryAsync();
            }
            connection.Close();
        }
        return result;
    }
}