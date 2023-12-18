using System;
using System.Collections.Generic;
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
using TrashTrackProjectV2.Model;

namespace TrashTrackProjectV2
{
    /// <summary>
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Window
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
            this.Close();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void textUsername_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtUsername.Focus();
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsername.Text) && txtUsername.Text.Length > 0)
            {
                textUsername.Visibility = Visibility.Collapsed;
            }
            else
            {
                textUsername.Visibility = Visibility.Visible;
            }
        }

        private void textNama_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtNama.Focus();
        }

        private void txtNama_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNama.Text) && txtNama.Text.Length > 0)
            {
                textNama.Visibility = Visibility.Collapsed;
            }
            else
            {
                textNama.Visibility = Visibility.Visible;
            }
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

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Text) && txtPassword.Text.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
            }
            else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }

        private void textNomorTelp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtNoTelp.Focus();
        }

        private void txtNoTelp_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNoTelp.Text) && txtNoTelp.Text.Length > 0)
            {
                textNomorTelp.Visibility = Visibility.Collapsed;
            }
            else
            {
                textNomorTelp.Visibility = Visibility.Visible;
            }
        }

        private void textAlamat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtAlamat.Focus();
        }

        private void txtAlamat_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAlamat.Text) && txtAlamat.Text.Length > 0)
            {
                textAlamat.Visibility = Visibility.Collapsed;
            }
            else
            {
                textAlamat.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            User user = new User();

            // Ambil data dari antarmuka pengguna
            string username = txtUsername.Text;
            string nama = txtNama.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string notelp = txtNoTelp.Text;
            string alamat = txtAlamat.Text;

            // Lakukan operasi insert menggunakan ViewModel
           bool success=user.Insert(username, nama, email, password, notelp, alamat);

            // Tampilkan pesan bahwa insert berhasil
            if (success == true)
            {
                //Successfully Inserted
                MessageBox.Show("New User Successfully Added");
                //Call the Clear Method Here
                //Clear();
                txtUsername.Clear();
                txtNama.Clear();
                txtEmail.Clear();
                txtPassword.Clear();
                txtNoTelp.Clear();
                txtAlamat.Clear();

            }
            else
            {
                //Failed to Add Contact
                MessageBox.Show("Failed to Add New User. Check your Username and email");
            }
        }

       
    }


}
