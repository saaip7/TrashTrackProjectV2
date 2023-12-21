using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TrashTrackProjectV2.Model;
using TrashTrackProjectV2.Utilities;
using TrashTrackProjectV2.View;

namespace TrashTrackProjectV2.ViewModel
{
    class BerlanggananVM : Utilities.ViewModelBase
    {
        public readonly PageModel _pageModel;
        public readonly User _user;
        public int DisplayDayPaket
        {
            get { return _pageModel.dayPaketA; }
            set
            {
                _pageModel.dayPaketA = value;
                OnPropertyChanged();
            }
        }
        public decimal DisplaySaldo
        {
            get { return _user.getSaldoInfo().Saldo; }
            set
            {
                _user.Saldo = (value);
                OnPropertyChanged();
            }
        }

        public BerlanggananVM()
        {
            _pageModel = new PageModel();
            _user = new User();
            DisplayDayPaket = new basicVoucher().voucherValue();
            DisplaySaldo = _user.getSaldoInfo().Saldo;
        }
     
    }
}
