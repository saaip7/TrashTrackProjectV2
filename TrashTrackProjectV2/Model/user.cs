using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Navigation;
using System.Text.Json;
using System.IO;
using System.Windows;

namespace TrashTrackProjectV2.Model
{
    public class User
    {   
        public string user_id { get; set; }
        public string Username { get; set; }
        public string Nama { get; set; }
        public string Email { get; set; }
        private string Password { get; set; }
        public string Alamat { get; set; }
        public string NoTelp { get; set; }
        public decimal Saldo { get; set; }

        public User()
        {
            user_id = string.Empty;
            Username = string.Empty;
            Nama = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Alamat = string.Empty;
            NoTelp = string.Empty;
        }
        public User(string user_id, string username, string nama, string email, string password, string notelp, string alamat)
        {
            this.user_id = user_id;
            Username = username;
            Nama = nama;
            Email = email;
            Password = password;
            NoTelp = notelp;
            Alamat = alamat;
        }
        
        string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        string userID = File.ReadAllText(@"jwt.json");

        public bool Insert(string username, string nama, string email, string password, string notelp, string alamat)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            bool isSuccessful = false;
            try
            {// Query SQL untuk insert data
                string query = "INSERT INTO tb_user (username, nama, email, password, notelp, address, Saldo) VALUES (@Username, @Nama, @Email, @Password, @noTelp, @Alamat, @Saldo)";

                // Membuka koneksi ke database
                connection.Open();

                // Membuat objek SqlCommand
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Menambahkan parameter ke query
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Nama", nama);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@noTelp", notelp);
                    command.Parameters.AddWithValue("@Alamat", alamat);
                    command.Parameters.AddWithValue("@Saldo", 0);

                    // Eksekusi perintah SQL
                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        isSuccessful = true;
                    }
                    else
                    {
                        isSuccessful = false;
                    }
                }
            }
            catch (Exception ex)
            {
                  // Tampilkan pesan error
                  Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                  // Tutup koneksi ke database
                  connection.Close();
            }
           return isSuccessful;

        }
        public bool UpdateUserProfile()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            bool isSuccessful = false;
            try
            {
                string query = "UPDATE tb_user SET nama = @Nama, address = @Alamat, noTelp = @NomorTelp, email=@Email WHERE user_id = @UserId";
                
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parameterized query untuk menghindari SQL Injection
                        command.Parameters.AddWithValue("@Nama", Nama);
                        command.Parameters.AddWithValue("@Alamat", Alamat);
                        command.Parameters.AddWithValue("@NomorTelp", NoTelp);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@UserId", userID);

                        connection.Open();

                        // Eksekusi pernyataan UPDATE
                        int rows= command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                            isSuccessful = true;
                        }
                        else
                    {
                            isSuccessful = false;
                        }
                    }
                
            }
            catch (Exception ex)
            {
                // Tampilkan pesan error
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Tutup koneksi ke database
                connection.Close();
            }
            return isSuccessful;
        }
        
        public LoginResult IsValidUser(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Sesuaikan nama tabel dan kolom dengan struktur database Anda
                string query = "SELECT * FROM tb_user WHERE email = @email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Jika username ditemukan, periksa validitas password
                        if (reader.HasRows)
                        {
                            reader.Read(); // Baca baris pertama (harusnya hanya satu baris)
                            string storedPassword = reader["password"].ToString();

                            // Sesuaikan dengan metode keamanan yang sesuai (contoh sederhana)
                            if (password == storedPassword)
                            {
                                return LoginResult.Success; // Login berhasil
                            }
                            else
                            {
                                return LoginResult.InvalidPassword; // Password tidak valid
                            }
                        }
                        else
                        {
                            return LoginResult.EmailNotFound; // Username tidak ditemukan
                        }
                    }
                }
            }
        }
        public enum LoginResult
        {
            Success,
            EmailNotFound,
            InvalidPassword
        }
       public string getUserID(string email)
        {
            string userID= string.Empty;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Gantilah "nama_tabel" dengan nama tabel yang sesuai di database Anda
                string query = "SELECT * FROM tb_user where email = @email";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userID = reader["user_id"].ToString();
                        }
                    }
                }
            }
            return userID;
        }
        public void createJWT(string getuserID)
        {
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;
            File.WriteAllText(@"jwt.json", getuserID);
        }
        public User GetUserProfile()
        {
            User user = new User();
            string query = "SELECT * FROM tb_user WHERE user_id = @UserId";

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
                            user.Username = reader["username"].ToString();
                            user.Nama = reader["nama"].ToString();
                            user.Email = reader["email"].ToString();
                            user.NoTelp = reader["notelp"].ToString();
                            user.Alamat = reader["address"].ToString();
                            // Setel properti lain sesuai dengan kolom-kolom lain pada tabel tb_user
                        }
                    }
                }
            }
            return user;
        }

        public User getSaldoInfo()
        {
            User user = new User();
            string query = "SELECT * FROM tb_user WHERE user_id = @UserId";

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
                            user.Saldo = (decimal) reader.GetDecimal(reader.GetOrdinal("Saldo"));
                        }
                    }
                }
            }
            return user;
        }
        public bool useSaldo(decimal jumlah)
        {
            User user = new User();
            user.Saldo = user.getSaldoInfo().Saldo - jumlah;
            if(user.Saldo < 0)
            {
                MessageBox.Show("Saldo tidak cukup");
                return false;
            }
            else
            {
                MessageBox.Show($"Saldo berhasil digunakan. Saldo sekarang: {user.Saldo}");
                // Simpan informasi pengisian saldo ke database
                SimpanSaldo(-jumlah);
                return true;
            }
        }
        public void IsiSaldo(decimal jumlah)
        {
            User user = new User();

            if (jumlah > 0)
            {
                user.Saldo += jumlah;
                // Simpan informasi pengisian saldo ke database
                user.SimpanSaldo(jumlah);
                MessageBox.Show($"berhasil menambahkan Rp {jumlah}. Saldo sekarang: Rp {user.getSaldoInfo().Saldo}");
            }
            else
            {
                MessageBox.Show("Jumlah pengisian saldo harus lebih dari 0.");
            }
        }

        public void SimpanSaldo(decimal jumlah)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE tb_user SET Saldo = Saldo + @Jumlah WHERE user_id = @user_id;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@user_id", userID);
                        command.Parameters.AddWithValue("@Jumlah", jumlah);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
   
}

/*public bool IsValidUser(string email, string password)
        {
            // Connection string: Ganti dengan connection string Anda
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Sesuaikan nama tabel dan kolom dengan struktur database Anda
                string query = "SELECT * FROM tb_user WHERE email = @email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Jika email ditemukan, periksa validitas password
                        if (reader.HasRows)
                        {
                            reader.Read(); // Baca baris pertama (harusnya hanya satu baris)
                            string storedPassword = reader["Password"].ToString();

                            // Sesuaikan dengan metode keamanan yang sesuai (contoh sederhana)
                            return password == storedPassword;
                        }
                        else
                        {
                            return false; // email tidak ditemukan
                        }
                    }
                }

            }
        }*/