using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace IIASA.FotoQuestApi.Database
{
    public class DatabaseProvider : IDatabaseProvider
    {
        private readonly string connectionString;
        private readonly IConfiguration configuration;

        public DatabaseProvider(IConfiguration configuration)
        {
            //var host = configuration["DBHOST"] ?? "localhost";
            //var port = configuration["DBPORT"] ?? "3306";
            //var userid = configuration["MYSQL_USER"] ?? "testuser";
            //var password = "pass@word1234";
            //var db = configuration["MYSQL_DATABASE"] ?? "fotoquest_db";

            //connectionString = $"server={host}; userid={userid};pwd={password};port={port};database={db}";
            //this.configuration = configuration;
            this.connectionString = configuration.GetConnectionString("DbConnectionString");
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
                        cmd.Parameters.AddWithValue("@Id", imageData.Id);
                        cmd.Parameters.AddWithValue("@OriginalName", imageData.OriginalName);
                        cmd.Parameters.AddWithValue("DateOfUpload", imageData.DateOfUpload);
                        cmd.Parameters.AddWithValue("HorizontalResolution", imageData.HorizontalResolution);
                        cmd.Parameters.AddWithValue("VerticalResolution", imageData.VerticalResolution);
                        cmd.Parameters.AddWithValue("Height", imageData.Height);
                        cmd.Parameters.AddWithValue("Width", imageData.Width);
                        cmd.Parameters.AddWithValue("FilePath", imageData.FilePath);
                        cmd.Parameters.AddWithValue("FileName", imageData.FileName);

                    }
                    cmd.ExecuteNonQuery();
                    //using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    //{
                    //    DataTable dt = new DataTable();
                    //    sda.Fill(dt);uspStoreImageData
                    //    GridView1.DataSource = dt;
                    //    GridView1.DataBind();
                    //}
                }
                connection.Close();
            }

        }
    }
}
