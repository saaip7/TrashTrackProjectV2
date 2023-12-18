using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrashTrackProjectV2.Model;

namespace TrashTrackProjectV2.ViewModel
{
    class PesanVM : Utilities.ViewModelBase
    {
        public readonly PageModel _pageModel;
        public long DisplaySlotPaket
        {
            get { return _pageModel.slotPaketA; }
            set
            {
                _pageModel.slotPaketA = value;
                OnPropertyChanged();
            }
        }

        public PesanVM()
        {
            subscription subscription = new subscription();
            _pageModel = new PageModel();
            DisplaySlotPaket = subscription.GetVoucherValue();
        }

    }
}
