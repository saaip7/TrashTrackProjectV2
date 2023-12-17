using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrashTrackProjectV2.Model;

namespace TrashTrackProjectV2.ViewModel
{
    class AkunVM :Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        public string userAkun
        {
            get { return _pageModel.nama; }
            set
            {
                _pageModel.nama = value;
                OnPropertyChanged();
            }
        }
        public AkunVM()
        {
            _pageModel = new PageModel();
            userAkun = "Joko";
        }

    }
}
