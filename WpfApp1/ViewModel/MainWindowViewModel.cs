using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Pages;

namespace WpfApp1.ViewModel
{
    class MainWindowViewModel : NotifyPropertyChanged
    {
        private System.Windows.Controls.Page mainPage;

        public System.Windows.Controls.Page MainPage
        {
            get => mainPage;
            set
            {
                mainPage = value;
                OnPropertyChanged();
            }
        }

        private readonly Page1 page1;

        public MainWindowViewModel()
        {
            page1 = new Page1();
            MainPage = page1;
        }
    }
}
