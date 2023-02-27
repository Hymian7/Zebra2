using Newtonsoft.Json.Linq;
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
using Zebra.Library.Services;

namespace ZebraDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, INotifyPropertyChanged
    {

        public App()
        {
            this.ConfigurationService = new ZebraConfigurationService();
            _ZebraConfig = null;
            _manager = null;

            try
            {
                if(!System.IO.Directory.Exists(@"configs"))
                {
                    System.IO.Directory.CreateDirectory(@"configs");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            ConfigurationService.ConfigurationLoaded += ConfigurationService_ConfigurationLoaded;
            ConfigurationService.ConfigurationUnloaded += ConfigurationService_ConfigurationUnloaded;
        }

        private async void ConfigurationService_ConfigurationUnloaded(object sender, EventArgs e)
        {
            Manager = null;
            if(LocalZebraServer != null)
            {
                await LocalZebraServer.StopAsync();
            }
        }

        private async void ConfigurationService_ConfigurationLoaded(object sender, EventArgs e)
        {
            // If the repository type is local, we need to spin up a local running server
            if (ConfigurationService.GetRepositoryType() == RepositoryType.Local)
            {
                LocalZebraServer = new ZebraServer.ZebraServer(ConfigurationService.GetConfigurationFilePath());
                await LocalZebraServer.StartAsync();
            }

            Manager = ZebraDbManagerFactory.GetManager(ConfigurationService.GetZebraConfig());
        }

        private ZebraServer.ZebraServer _localServer;

        public ZebraServer.ZebraServer LocalZebraServer
        {
            get { return _localServer; }
            set { _localServer = value; }
        }

        private ZebraConfigurationService _configurationService;

        public ZebraConfigurationService ConfigurationService
        {
            get { return _configurationService; }
            set { _configurationService = value; }
        }


        [Obsolete]
        private ZebraConfig _ZebraConfig;

        [Obsolete]
        public ZebraConfig ZebraConfig
        {
            get { return _ZebraConfig; }
            set { 
                    _ZebraConfig = value;

                if (value != null)
                {
                    Manager = ZebraDbManagerFactory.GetManager(value); 
                }
                else
                {
                    Manager = null;
                }
                    
                    OnPropertyChanged(nameof(ZebraConfig)); }
        }

        private IZebraDBManager _manager;

        public IZebraDBManager Manager
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
