using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TrashTrackProjectV2.Model;
using System.Reflection;

namespace TrashTrackProjectV2.Model
{
    public class subscription
    {
        public static subscription Subscription  = new subscription();
        protected int additionalVouchers { get; set; }
        protected decimal price { get; set; }
        public subscription()
        {
            additionalVouchers = 0;
        }
        public subscription(int additionalVouchers)
        {
            this.additionalVouchers = additionalVouchers;
        }

        //string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB; AttachDbFilename = {Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\trashTrackProject.mdf; Integrated Security=True";
        string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB; AttachDbFilename = {Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\trashTrackProject.mdf; Integrated Security=True";
        string userID = File.ReadAllText(@"jwt.json");
        internal long voucherCounter;

        public decimal getPrice()
        {
            return price;
        }
        public int voucherValue()
        {
            return additionalVouchers;
        }
        public void AddVoucher()
        {
            // Cek apakah user_id sudah ada dalam tb_subscription
            if (UserSubscriptionExists())
            {
                if (GetVoucherValue()+additionalVouchers > 100)
                {
                    throw new ArgumentException("Jumlah voucher tidak boleh melebihi 99.");
                }
                else if (GetVoucherValue() + additionalVouchers < 0)
                {
                    throw new ArgumentException("Jumlah voucher tidak boleh negatif.");
                }
                else
                {
                    // Jika user_id sudah ada, tambahkan jumlah voucher
                    UpdateVoucher(additionalVouchers);
                }
            }
            else
            {
                // Jika user_id belum ada, tambahkan row baru ke tb_subscription
                InsertNewSubscription();
                if (GetVoucherValue()+additionalVouchers< 0)
                {
                    throw new ArgumentException("Jumlah voucher tidak boleh negatif.");
                }
                else
                {
                    // Jika user_id sudah ada, tambahkan jumlah voucher
                    UpdateVoucher(additionalVouchers);
                }
            }
        }
        

        private bool UserSubscriptionExists()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM tb_subscription WHERE UserID = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userID);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        private void UpdateVoucher(int additionalVouchers)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UPDATE tb_subscription SET Voucher = Voucher + @AdditionalVouchers WHERE UserID = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userID);
                    command.Parameters.AddWithValue("@AdditionalVouchers", additionalVouchers);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void InsertNewSubscription()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("INSERT INTO tb_subscription (UserID, Voucher) VALUES (@UserId, @InitialVouchers)", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userID);
                    command.Parameters.AddWithValue("@InitialVouchers", 0);

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
    public class basicVoucher : subscription
    {
        public basicVoucher()
        {
            additionalVouchers = 3;
            price = 30000;
        }
    }
    public class mediumVoucher : subscription
    {
        public mediumVoucher()
        {
            additionalVouchers = 6;
            price = 50000;
        }
    }
    public class premiumVoucher : subscription
    {
        public premiumVoucher()
        {
            additionalVouchers = 10;
            price = 65000;
        }
    }
}

