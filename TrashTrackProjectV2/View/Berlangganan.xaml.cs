﻿using System;
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

        private void btn_click_basic(object sender, RoutedEventArgs e)
        {
            basicVoucher basicVoucher = new basicVoucher();
            User user = new User();
            bool isEnough = user.useSaldo(basicVoucher.getPrice());
            if (isEnough)
            {
                basicVoucher.AddVoucher();
                MessageBox.Show($"Berhasil Menambahkan {basicVoucher.voucherValue()} Voucher");
            }
            lblSaldo.Text = user.getSaldoInfo().Saldo.ToString();

        }

        private void btn_click_medium(object sender, RoutedEventArgs e)
        {
            mediumVoucher medium = new mediumVoucher();
            User user = new User();
            bool isEnough = user.useSaldo(medium.getPrice());
            if (isEnough) {
                medium.AddVoucher();
                MessageBox.Show($"Berhasil Menambahkan {medium.voucherValue()} Voucher");
            }
            lblSaldo.Text = user.getSaldoInfo().Saldo.ToString();

        }

        private void btn_click_premium(object sender, RoutedEventArgs e)
        {
            premiumVoucher premium = new premiumVoucher();
            User user = new User();
            bool isEnough = user.useSaldo(premium.getPrice());
            if (isEnough){
                premium.AddVoucher();
                MessageBox.Show($"Berhasil Menambahkan {premium.voucherValue()} Voucher");
            }
            lblSaldo.Text = user.getSaldoInfo().Saldo.ToString();

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            //user.IsiSaldo(Convert.ToDecimal(txtbIsiSaldo.Text));
            user.IsiSaldo(Convert.ToDecimal(txtbIsiSaldo.Text));
            lblSaldo.Text = user.getSaldoInfo().Saldo.ToString();
        }
    }
}
