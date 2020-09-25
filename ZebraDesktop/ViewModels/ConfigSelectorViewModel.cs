using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Zebra.Library;

namespace ZebraDesktop.ViewModels
{
    public class ConfigSelectorViewModel : ViewModelBase
    {
        #region Properties
        private ZebraConfig _loadedConfiguration = null;
        public ZebraConfig LoadedConfiguration { get => _loadedConfiguration; set { _loadedConfiguration = value; NotifyPropertyChanged(); } }

        private FileInfo _selectedFile;
        public FileInfo SelectedFile
        {
            get { return _selectedFile; }
            set { _selectedFile = value; NotifyPropertyChanged(); UpdateButtonStatus(); }
        }

        private DelegateCommand _newConfigCommand;
        public DelegateCommand NewConfigCommand
        {
            get { return _newConfigCommand; }
            set { _newConfigCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _loadConfigCommand;
        public DelegateCommand LoadConfigCommand
        {
            get { return _loadConfigCommand; }
            set { _loadConfigCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand
        {
            get { return _cancelCommand; }
            set { _cancelCommand = value; NotifyPropertyChanged(); }
        }



        #endregion


        private List<FileInfo> _configFiles;

        public List<FileInfo> ConfigFiles
        {
            get { return _configFiles; }
            set { _configFiles = value; NotifyPropertyChanged(); }
        }


        #region Constructors

        public ConfigSelectorViewModel()
        {
            ConfigFiles = new List<FileInfo>();

            NewConfigCommand = new DelegateCommand(ExecuteNewConfigCommand);
            LoadConfigCommand = new DelegateCommand(ExecuteLoadConfigCommand, canExecuteLoadConfigCommand);
            CancelCommand = new DelegateCommand(ExecuteCancelCommand);

            GetConfigs();

        }

        #endregion

        #region Commands

        private void ExecuteCancelCommand(object obj)
        {
            CloseWindow((Window)obj);
        }

        private bool canExecuteLoadConfigCommand(object obj)
        {
            return SelectedFile != null;
        }

        private void ExecuteLoadConfigCommand(object obj)
        {
            LoadConfig();
            CloseWindow((Window)obj);
        }

        private void ExecuteNewConfigCommand(object obj)
        {
            frmNewConfig frm = new frmNewConfig();
            frm.Show();
        }

        private void ExecuteUpdateButtonStateCommand(object obj)
        {
            UpdateButtonStatus();
        }

        #endregion

        #region Methods
        private void GetConfigs()
        {
            ConfigFiles.Clear();
            foreach (var info in Directory.GetFiles("configs", "*.zebraconfig"))
            {
                ConfigFiles.Add(new FileInfo(info));
            };

        }

        private void LoadConfig()
        {
            try
            {
                LoadedConfiguration = ZebraConfig.FromXML(SelectedFile.FullName);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler beim Laden der Konfiguration");
            }
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }
        #endregion
    }
}
