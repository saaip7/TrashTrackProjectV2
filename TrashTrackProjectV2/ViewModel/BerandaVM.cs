using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrashTrackProjectV2.Model;
using System.IO;

namespace TrashTrackProjectV2.ViewModel
{
    class BerandaVM : Utilities.ViewModelBase
    {
        public readonly PageModel _pageModel;
        public long DisplayVoucher
        {
            get { return _pageModel.voucherCounter; }
            set
            {
                _pageModel.voucherCounter = value;
                OnPropertyChanged();
            }
        }

        public BerandaVM()
        {
            _pageModel = new PageModel();
            subscription subscription = new subscription();
            DisplayVoucher = subscription.GetVoucherValue();
        }
    }
}
