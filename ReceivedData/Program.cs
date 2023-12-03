using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DatabaseMover
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();

        public class ReceivedRecord
        {
            public int Id { get; set; }
            public string DataValue { get; set; }
            public DateTime ReceivedDate { get; set; }
        }

        static void Main(string[] args)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            var records = new List<ReceivedRecord>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Fetch all data from the ReceivedData table
                using (SqlCommand cmdFetch = new SqlCommand("SELECT * FROM ReceivedData", connection))
                using (SqlDataReader reader = cmdFetch.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        records.Add(new ReceivedRecord
                        {
                            Id = (int)reader["Id"],
                            DataValue = reader["DataValue"].ToString(),
                            ReceivedDate = (DateTime)reader["ReceivedDate"]
                        });
                    }
                }

                foreach (var record in records)
                {
                    Console.WriteLine($"Copying data: {record.DataValue} received on {record.ReceivedDate}");

                    // Insert into ArchivedData
                    using (SqlCommand cmdInsert = new SqlCommand(@"
                            INSERT INTO ArchivedData (DataValue, ReceivedDate, ArchivedDate)
                            VALUES (@DataValue, @ReceivedDate, @CurrentDate)", connection))
                    {
                        cmdInsert.Parameters.AddWithValue("@DataValue", record.DataValue);
                        cmdInsert.Parameters.AddWithValue("@ReceivedDate", record.ReceivedDate);
                        cmdInsert.Parameters.AddWithValue("@CurrentDate", DateTime.Now);
                        cmdInsert.ExecuteNonQuery();
                    }

                    // Delete from ReceivedData
                    using (SqlCommand cmdDelete = new SqlCommand(@"
                            DELETE FROM ReceivedData WHERE Id = @Id", connection))
                    {
                        cmdDelete.Parameters.AddWithValue("@Id", record.Id);
                        cmdDelete.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }
    }
}
