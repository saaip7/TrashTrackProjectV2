using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrashTrackProjectV2.Model;
using System.IO;
using Pesan = TrashTrackProjectV2.Model.Pesan;

namespace TrashTrackProjectV2.ViewModel
{
    class BerandaVM : Utilities.ViewModelBase
    {
        public readonly Pesan _pageModel;
        subscription subscription = new subscription();
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

        public string DisplayEstimasi
        {
            get { return _pageModel.estimasi; }
            set
            {
                _pageModel.estimasi = value;
                OnPropertyChanged();
            }
        }

        public BerandaVM()
        {
            _pageModel = new Pesan();
            if (_pageModel.isPesanActive())
            {
                _pageModel = _pageModel.GetPesanActive();
                DisplayNamaPetugas = _pageModel.namaPetugas;
                DisplayEstimasi = _pageModel.estimasi;
            }
            DisplayVoucher = subscription.GetVoucherValue();
        }
    }
}
