using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisioterapiaCuerposano.Model
{
    class INotifyCita
    {
        public partial class Cita : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged(string propertyName)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
