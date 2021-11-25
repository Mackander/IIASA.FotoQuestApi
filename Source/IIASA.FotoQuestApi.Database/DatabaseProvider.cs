using Dapper;
using IIASA.FotoQuestApi.Model;
using IIASA.FotoQuestApi.Model.Exceptions;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

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

        public FileData LoadImageData(IDataRequest dataRequest)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                //using (MySqlCommand cmd = new MySqlCommand(dataRequest.Command, connection))
                //{
                //    cmd.CommandType = dataRequest.CommandType;
                //    if (dataRequest is GetImageDataRequest imageData)
                //    {
                //        cmd.Parameters.AddWithValue("arg_Id", imageData.Id);

                //    }
                //    cmd.ExecuteNonQuery();
                //    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                //    {
                //        DataTable dt = new DataTable();
                //        sda.Fill(dt); 
                //        //GridView1.DataSource = dt;
                //        //GridView1.DataBind();
                //    }
                //}

                var fileId = (dataRequest as GetImageDataRequest).Id;
                var procedure = dataRequest.Command;
                var values = new { arg_Id = fileId };
                var fileData = connection.QuerySingle<FileData>(procedure, values, commandType: dataRequest.CommandType);
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
