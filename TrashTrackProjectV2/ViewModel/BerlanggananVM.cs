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
        public int DisplayDayPaket
        {
            get { return _pageModel.dayPaketA; }
            set
            {
                _pageModel.dayPaketA = value;
                OnPropertyChanged();
            }
        }

        public BerlanggananVM()
        {

            _pageModel = new PageModel();
            DisplayDayPaket = 8;
           
        }
     
    }
}
