using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace xjtu_campus_uwp.ViewModel
{
    public class CurrentTheme : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private ElementTheme _Theme = ElementTheme.Dark;

        public ElementTheme Theme
        {
            get { return _Theme; }
            set
            {
                _Theme = value;
                this.OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
