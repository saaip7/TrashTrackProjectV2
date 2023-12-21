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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrashTrackProjectV2.Model;

namespace TrashTrackProjectV2.View
{
    /// <summary>
    /// Interaction logic for Akun.xaml
    /// </summary>
    public partial class Akun : UserControl
    {
        public Akun()
        {
            InitializeComponent();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            SetTextBoxesReadOnly(false);
            update_button.Visibility = Visibility.Hidden;
            selesai_button.Visibility = Visibility.Visible;
        }
        private void Selesai_Click(object sender, RoutedEventArgs e)
        {   
            User user = new User();
            user.Nama = txtbName.Text;
            user.Email = txtbEmail.Text;
            user.NoTelp = txtbNoTelp.Text;
            user.Alamat = txtbAlamat.Text;
            bool success = user.UpdateUserProfile();
            if (success)
            {
                MessageBox.Show("Update Profile berhasil!");
            }
            else
            {
                MessageBox.Show("Update Profile gagal!");
            }
            SetTextBoxesReadOnly(true);
            selesai_button.Visibility = Visibility.Hidden;
            update_button.Visibility = Visibility.Visible;
        }
        private void SetTextBoxesReadOnly(bool isReadOnly)
        {
            txtbName.IsReadOnly = isReadOnly;
            txtbEmail.IsReadOnly = isReadOnly;
            txtbNoTelp.IsReadOnly = isReadOnly;
            txtbAlamat.IsReadOnly = isReadOnly;
            /*// Mengakses seluruh TextBox dalam StackPanel
            foreach (UIElement element in akun_panel.Children)
            {
                if (element is TextBox textBox)
                {
                    textBox.IsReadOnly = isReadOnly;
                }
            }*/
        }
    }
}
