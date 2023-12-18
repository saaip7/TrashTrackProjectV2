using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Navigation;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;
using System.IO;

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
        string userID = File.ReadAllText(@"jwt.json");

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
                            // Gantilah "nama_kolom" dengan nama kolom yang sesuai di tabel Anda
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

            // Ganti dengan informasi koneksi database Anda
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
    }
   
}
