using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Data;

namespace TrashTrackProjectV2.Model
{
    internal class Pesan
    {
        public string namaPetugas { get; set; }
        public string alamat { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string estimasi { get; set; }
        public string status { get; set; } 
       
        string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        string userID = File.ReadAllText(@"jwt.json");

        public bool isPesanActive()
        {
            string query = "SELECT * FROM tb_pesanan WHERE user_id = @UserId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userID);

                    connection.Open();

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // Jika ada data
                        {
                            return true;
                        }
                        else
                        {
                            return false;                        }
                    }
                }
            }
        }
        public Pesan GetPesanActive()
        {
            Pesan pesan1 = new Pesan();

            string query = "SELECT * FROM tb_pesanan WHERE user_id = @UserId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userID);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Jika ada data
                        { 
                            // Setel nilai-nilai dari database ke instance User
                            pesan1.namaPetugas = reader["NamaPetugas"].ToString();
                            pesan1.alamat =reader["Alamat"].ToString();
                            pesan1.latitude= (double) reader.GetDecimal(reader.GetOrdinal("latitude"));
                            pesan1.longitude = (double) reader.GetDecimal(reader.GetOrdinal("longitude"));
                            pesan1.estimasi = reader["EstimasiPengambilan"].ToString();
                            pesan1.status = reader["Status"].ToString();
                        }
                    }
                }
            }

            return pesan1;
        }
    }
}
