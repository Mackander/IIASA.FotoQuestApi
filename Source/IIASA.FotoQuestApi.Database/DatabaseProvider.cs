﻿using Dapper;
using IIASA.FotoQuestApi.Model;
using IIASA.FotoQuestApi.Model.Exceptions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace IIASA.FotoQuestApi.Database
{
    public class DatabaseProvider : IDatabaseProvider
    {
        private readonly string connectionString;

        public DatabaseProvider(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<FileData> LoadImageData(IDataRequest dataRequest)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var fileId = (dataRequest as GetImageDataRequest).Id;
                var procedure = dataRequest.Command;
                var values = new { arg_Id = fileId };
                var fileData = await connection.QuerySingleAsync<FileData>(procedure, values, commandType: dataRequest.CommandType);
                connection.Close();

                if (fileData == null)
                {
                    throw new NotFoundException($"Image not found for provided Id : {fileId}");
                }

                return fileData;
            }
        }

        public void SaveImageData(IDataRequest data)
        {
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
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }

        }
    }
}
