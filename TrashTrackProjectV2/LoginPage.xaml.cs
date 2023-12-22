using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using System.IO;
using TrashTrackProjectV2.Model;
using static TrashTrackProjectV2.Model.User;
using System.Reflection;

namespace TrashTrackProjectV2
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
            /*var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var databasePath = System.IO.Path.Combine(appDataPath, "YourAppName", "Database.mdf");

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionString = config.ConnectionStrings.ConnectionStrings["connString"];

            // Set the dynamic part of the connection string
            connectionString.ConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={databasePath};Integrated Security=True";

            // Save the changes to the configuration file
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");*/

        }

        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
            {
                textEmail.Visibility = Visibility.Collapsed;
            }
            else
            {
                textEmail.Visibility = Visibility.Visible;
            }
        }

        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(@"jwt.json", string.Empty);

            User user = new User();
            if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
            {
                if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
                {
                    string email = txtEmail.Text;
                    string password = txtPassword.Password.ToString();
                    LoginResult loginResult = user.IsValidUser(email, password);

                    switch (loginResult)
                    {
                        case LoginResult.Success:
                            user.createJWT(user.getUserID(email));
                            MainWindow main = new MainWindow();
                            main.Show();
                            this.Close();
                            break;

                        case LoginResult.EmailNotFound:
                            MessageBox.Show("Email tidak ditemukan.", "Gagal Login");
                            break;

                        case LoginResult.InvalidPassword:
                            MessageBox.Show("Password tidak valid.", "Gagal Login");
                            break;
                    }
                    /*if (user.IsValidUser(txtEmail.Text, txtPassword.Password.ToString()))
                    {
                        MainWindow main = new MainWindow();
                        main.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Email or Password is incorrect");
                    }*/
                }
                else
                {
                    MessageBox.Show("Please enter your password");
                }
            }
            else
            {
                MessageBox.Show("Please enter your email");
            }   
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SignUpPage signUp = new SignUpPage();
            signUp.Show();
            this.Close();
        }
        
    }
}
