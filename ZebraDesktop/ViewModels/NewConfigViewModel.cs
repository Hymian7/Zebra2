using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows;
using Zebra.Library;
using ZebraDesktop.Enums;
using ZebraDesktop.Views;

namespace ZebraDesktop.ViewModels
{
    public class NewConfigViewModel : ViewModelBase
    {
        #region Properties

        private String _configName;

        public String ConfigName
        {
            get { return _configName; }
            set { _configName = value; NotifyPropertyChanged(); }
        }

        private String _repositoryPath;

        public String RepositoryPath
        {
            get { return _repositoryPath; }
            set { _repositoryPath = value; NotifyPropertyChanged(); }
        }

        private string _IPAddress;

        public string IPAddress
        {
            get { return _IPAddress; }
            set { _IPAddress = value; NotifyPropertyChanged(); }
        }

        private string _port;

        public string Port
        {
            get { return _port; }
            set { _port = value; NotifyPropertyChanged(); }
        }


        private EnumSelection<RepositoryType> _repositoryType = new EnumSelection<RepositoryType>(Zebra.Library.RepositoryType.Remote);

        public EnumSelection<RepositoryType> RepositoryType
        {
            get { return _repositoryType; }
            set { _repositoryType = value; NotifyPropertyChanged(); }
        }


        private ZebraConfig zebraConfig;

        public ZebraConfig ConfigFile
        {
            get { return zebraConfig; }
            set { zebraConfig = value; NotifyPropertyChanged(); }
        }


        private DelegateCommand _browseRepositoryDirectoryCommand;

        public DelegateCommand BrowseRepositoryDirectoryCommand
        {
            get { return _browseRepositoryDirectoryCommand; }
            set { _browseRepositoryDirectoryCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _saveNewConfigCommand;

        public DelegateCommand SaveNewConfigCommand
        {
            get { return _saveNewConfigCommand; }
            set { _saveNewConfigCommand = value; NotifyPropertyChanged(); }
        }

        private IClosable _parentContainer;
        public IClosable ParentContainer
        {
            get { return _parentContainer; }
            set { _parentContainer = value; NotifyPropertyChanged();}
        }
        

        #endregion

        public NewConfigViewModel()
        {

            RepositoryType = new EnumSelection<RepositoryType>(Zebra.Library.RepositoryType.Local);

            BrowseRepositoryDirectoryCommand = new DelegateCommand(ExecuteBrowseRepositoryPathCommand);
            
            SaveNewConfigCommand = new DelegateCommand(ExecuteSaveNewConfigCommand, canExecuteSaveNewConfigCommand);

            this.PropertyChanged += UpdateButtonStatus;
            
            //TODO: Implemet INotifyPropertyChanged for Credentials, so that Update can trigger the button state
            
        }

        #region Commands
        private bool canExecuteSaveNewConfigCommand(object obj)
        {
            return true;
        }

        private void ExecuteSaveNewConfigCommand(object obj)
        {
            // Check if Name and Path are emtpy
            if (String.IsNullOrEmpty(ConfigName) || String.IsNullOrEmpty(RepositoryPath))
            {
                MessageBox.Show("Bitte alle Felder ausfüllen");
                return;
            }

            if(RepositoryType.Value == Zebra.Library.RepositoryType.Remote && (String.IsNullOrEmpty(IPAddress) || String.IsNullOrEmpty(Port)))
            {
                MessageBox.Show("Bitte alle Felder ausfüllen");
                return;
            }

            if(Directory.Exists(RepositoryPath) && Directory.GetFiles(RepositoryPath).Length > 0)
            {
                MessageBox.Show("Bitte einen leeren Ordner wählen");
                return;
            }

            if (RepositoryType.Value == Zebra.Library.RepositoryType.Remote && (IPAddress != "localhost" && (System.Net.IPAddress.TryParse(IPAddress, out _) == false)|| long.TryParse(Port, out _) == false))
            {
                MessageBox.Show("IP Adresse oder Port sind ungültig");
                return;
            }



            if (!Directory.Exists(RepositoryPath))
            {
                Directory.CreateDirectory(RepositoryPath);
            }

            ZebraConfig conf;

            if(RepositoryType.Value == Zebra.Library.RepositoryType.Remote)
            {
                conf = new ZebraConfig(ConfigName, new DirectoryInfo(RepositoryPath), IPAddress, Port);
            }
            else
            {
                conf = new ZebraConfig(ConfigName, new DirectoryInfo(RepositoryPath));
            }


            try
            {
                if(!Directory.Exists(@"configs"))
                {
                    Directory.CreateDirectory(@"configs");
                }
                conf.Serialize(@"configs");
                ParentContainer.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        

        private void ExecuteBrowseRepositoryPathCommand(object obj)
        {
            MessageBox.Show("Einfach Pfad in die Textbox kopieren");
        }

        #endregion
    }
}
