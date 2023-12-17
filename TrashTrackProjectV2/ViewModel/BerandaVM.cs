using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrashTrackProjectV2.Model;

namespace TrashTrackProjectV2.ViewModel
{
    class BerandaVM : Utilities.ViewModelBase
    {
        public readonly PageModel _pageModel;
        public int DisplayDayCounter
        {
            get { return _pageModel.dayCounter; }
            set
            {
                _pageModel.dayCounter = value;
                OnPropertyChanged();
            }
        }

        public BerandaVM()
        {
            _pageModel = new PageModel();
            DisplayDayCounter = 20;
        }
    }
}
