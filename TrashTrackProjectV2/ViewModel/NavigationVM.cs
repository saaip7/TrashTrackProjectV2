using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrashTrackProjectV2.Utilities;
using System.Windows.Input;

namespace TrashTrackProjectV2.ViewModel
{
    class NavigationVM : Utilities.ViewModelBase
    {
        private object _curretView;
        public object CurrentView
        {
            get { return _curretView; }
            set
            {
                _curretView = value;
                OnPropertyChanged();
            }
        }

        public ICommand BerandaCommand { get; set; }
        public ICommand PesanCommand { get; set; }
        public ICommand BerlanggananCommand { get; set; }
        public ICommand AkunCommand { get; set; }

        private void Beranda(object obj) => CurrentView = new BerandaVM();
        private void Pesan(object obj) => CurrentView = new PesanVM();
        private void Berlangganan(object obj) => CurrentView = new BerlanggananVM();
        private void Akun(object obj) => CurrentView = new AkunVM();

        public NavigationVM()
        {
            BerandaCommand = new RelayCommand(Beranda);
            PesanCommand = new RelayCommand(Pesan);
            BerlanggananCommand = new RelayCommand(Berlangganan);
            AkunCommand = new RelayCommand(Akun);
            CurrentView = new BerandaVM();
        }
        
    }
}
