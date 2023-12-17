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

        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textNama_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtNama_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }

        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textNomorTelp_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtNoTelp_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textAlamat_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtAlamat_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            User user = new User();

            // Ambil data dari antarmuka pengguna
            string username = txtUsername.Text;
            string nama = txtNama.Password.ToString();
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
