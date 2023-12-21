using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrashTrackProjectV2.Model;
using System.Security.Cryptography.X509Certificates;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Windows.Controls;
using TrashTrackProjectV2.View;
using Pesan = TrashTrackProjectV2.Model.Pesan;

namespace TrashTrackProjectV2.ViewModel
{
    class PesanVM : Utilities.ViewModelBase
    {
        public readonly Pesan _pageModel;
        public subscription subscription = new subscription();

        public long DisplayVoucher
        {
            get { return subscription.voucherCounter; }
            set
            {
                subscription.voucherCounter = value;
                OnPropertyChanged();
            }
        }
        public string DisplayNamaPetugas
        {
            get { return _pageModel.namaPetugas; }
            set
            {
                _pageModel.namaPetugas = value;
                OnPropertyChanged();
            }
        }
        public string DisplayAlamat
        {
            get { return _pageModel.alamat; }
            set
            {
                _pageModel.alamat = value;
                OnPropertyChanged();
            }
        }
        public double DisplayLatitude
        {
            get { return _pageModel.latitude; }
            set
            {
                _pageModel.latitude = value;
                OnPropertyChanged();
            }
        }
        public double DisplayLongitude
        {
            get { return _pageModel.longitude; }
            set
            {
                _pageModel.longitude = value;
                OnPropertyChanged();
            }
        }
        public string DisplayEstimasi
        {
            get { return _pageModel.estimasi; }
            set
            {
                _pageModel.estimasi = value;
                OnPropertyChanged();
            }
        }

        public PesanVM()
        {
            _pageModel = new Pesan();
            if (_pageModel.isPesanActive())
            {
                _pageModel = _pageModel.GetPesanActive();
                DisplayNamaPetugas = _pageModel.namaPetugas;
                DisplayAlamat = _pageModel.alamat;
                DisplayLatitude = _pageModel.latitude;
                DisplayLongitude = _pageModel.longitude;
                DisplayEstimasi = _pageModel.estimasi;
            }
            DisplayVoucher = subscription.GetVoucherValue();
        }
    }
}
