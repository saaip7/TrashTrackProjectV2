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
        private readonly User _pageModel;
        public string userUsername
        {
            get { return _pageModel.Username; }
            set
            {
                _pageModel.Username = value;
                OnPropertyChanged();
            }
        }
        public string userNama
        {
            get { return _pageModel.Nama; }
            set
            {
                _pageModel.Nama = value;
                OnPropertyChanged();
            }
        }
        public string userEmail
        {
            get { return _pageModel.Email; }
            set
            {
                _pageModel.Email = value;
                OnPropertyChanged();
            }
        }
        public string userAlamat
        {
            get { return _pageModel.Alamat; }
            set
            {
                _pageModel.Alamat = value;
                OnPropertyChanged();
            }
        }
        public string userNoTelp
        {
            get { return _pageModel.NoTelp; }
            set
            {
                _pageModel.NoTelp = value;
                OnPropertyChanged();
            }
        }
        public AkunVM()
        {
            _pageModel = new User();
            userUsername = _pageModel.GetUserProfile().Username;
            userNama = _pageModel.GetUserProfile().Nama;
            userEmail = _pageModel.GetUserProfile().Email;
            userAlamat = _pageModel.GetUserProfile().Alamat;
            userNoTelp = _pageModel.GetUserProfile().NoTelp;
        }

    }
}
