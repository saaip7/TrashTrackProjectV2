using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TrashTrackProjectV2.Model
{
    internal class subscription
    {
       string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        string userID = File.ReadAllText(@"jwt.json");
        public void AddVoucher(string userId, int additionalVouchers)
        {
            // Cek apakah user_id sudah ada dalam tb_subscription
            if (UserExists(userId))
            {
                // Jika user_id sudah ada, tambahkan jumlah voucher
                UpdateVoucher(userId, additionalVouchers);
            }
            else
            {
                // Jika user_id belum ada, tambahkan row baru ke tb_subscription
                InsertNewSubscription(userId, additionalVouchers);
            }
        }
        

        private bool UserExists(string userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM tb_subscription WHERE UserID = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        private void UpdateVoucher(string userId, int additionalVouchers)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UPDATE tb_subscription SET Voucher = Voucher + @AdditionalVouchers WHERE UserID = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@AdditionalVouchers", additionalVouchers);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void InsertNewSubscription(string userId, int initialVouchers)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("INSERT INTO tb_subscription (UserID, Voucher) VALUES (@UserId, @InitialVouchers)", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@InitialVouchers", initialVouchers);

                    command.ExecuteNonQuery();
                }
            }
        }
        public long GetVoucherValue()
        {
            string query = "SELECT Voucher FROM tb_subscription WHERE UserID = @UserId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Parameterized query untuk menghindari SQL Injection
                    command.Parameters.AddWithValue("@UserId", userID);

                    connection.Open();

                    // Menggunakan ExecuteScalar untuk mendapatkan nilai tunggal dari query
                    object result = command.ExecuteScalar();

                    // Memeriksa apakah nilai tidak null sebelum mengonversi ke tipe data yang diharapkan
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt64(result);
                    }
                    else
                    {
                        // Handle jika nilai voucher tidak ditemukan atau bernilai NULL
                        Console.WriteLine($"Nilai voucher untuk user dengan ID {userID} tidak ditemukan.");
                        return 0; // Nilai default jika tidak ditemukan
                    }
                }
            }
        }
    }

}

