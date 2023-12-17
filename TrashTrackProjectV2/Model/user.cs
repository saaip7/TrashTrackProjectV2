using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace TrashTrackProjectV2.Model
{
  
    // Contoh kelas User dalam Model
    public class User
    {
        // Properti pengguna
        public string Username { get; set; }
        public string Nama { get; set; }
        public string Email { get; set; }
        private string Password { get; set; }
        public string Alamat { get; set; }
        public string NoTelp { get; set; }


        // Konstruktor
        /* public User(string username, string nama, string email, string password, string notelp, string alamat)
         {
             Username = username;
             Nama = nama;
             Email = email;
             Password = password;
             NoTelp = notelp;
             Alamat = alamat;
         }*/
        string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        //string connectionString = "Data Source=localhost\\sqlexpress01;Initial Catalog=trashTrackProject;Integrated Security=True";
        public bool Insert(string username, string nama, string email, string password, string notelp, string alamat)
        {
            // Connection string: Ganti dengan connection string Anda

            SqlConnection connection = new SqlConnection(connectionString);
            bool isSuccessful = false;
            try
            {// Query SQL untuk insert data
                string query = "INSERT INTO tb_user (username, nama, email, password, notelp, address) VALUES (@Username, @Nama, @Email, @Password, @noTelp, @Alamat)";

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
    }

}
