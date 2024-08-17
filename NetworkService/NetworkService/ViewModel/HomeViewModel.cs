using NetworkService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
    public class HomeViewModel : BindableBase
    {
        private string mainTitle;
        private string subtitle;

        public HomeViewModel()
        {
            MainTitle = "Network\nService\nApplication";
            Subtitle = "Nikola Inđić PR-150/2020";
        }
        public string MainTitle
        {
            get { return mainTitle; }
            set
            {
                mainTitle = value;
                OnPropertyChanged("MainTitle");
            }
        }
        public string Subtitle
        {
            get { return subtitle; }
            set
            {
                subtitle = value;
                OnPropertyChanged("Subtitle");
            }
        }
    }
}
