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
using System.IO;

namespace TrashTrackProjectV2.View
{
    /// <summary>
    /// Interaction logic for Berlangganan.xaml
    /// </summary>
    public partial class Berlangganan : UserControl
    {
        string userID = File.ReadAllText(@"jwt.json");

        public Berlangganan()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            subscription subscription = new subscription();
            subscription.AddVoucher(userID, 8);
            MessageBox.Show("Berhasil Menambahkan 8 Voucher");
        }
    }
}
