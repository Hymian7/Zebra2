using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Zebra.Library;

namespace ZebraDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, INotifyPropertyChanged
    {

        public App()
        {
            _ZebraConfig = null;
            _manager = null;
        }

        private ZebraConfig _ZebraConfig;

        public ZebraConfig ZebraConfig
        {
            get { return _ZebraConfig; }
            set { 
                    _ZebraConfig = value;

                if (value != null)
                {
                    Manager = new ZebraDBManager(value); 
                }
                else
                {
                    Manager = null;
                }
                    
                    OnPropertyChanged(nameof(ZebraConfig)); }
        }

        private ZebraDBManager _manager;

        public ZebraDBManager Manager
        {
            get { return _manager; }
            private set { _manager = value; OnPropertyChanged(nameof(Manager)); }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        #endregion
    }
}
