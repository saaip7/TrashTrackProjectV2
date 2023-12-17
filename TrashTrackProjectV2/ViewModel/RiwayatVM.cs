using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrashTrackProjectV2.Model;

namespace TrashTrackProjectV2.ViewModel
{
    class RiwayatVM : Utilities.ViewModelBase
    {
        public readonly PageModel _pageModel;
        public bool RiwayatMati
        {
            get { return _pageModel.RiwayatOn; }
            set
            {
                _pageModel.RiwayatOn = value;
                OnPropertyChanged();
            }
        }

        public RiwayatVM()
        {
            _pageModel = new PageModel();
            RiwayatMati = true;
        }
    }

}
