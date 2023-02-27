﻿using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Zebra.Library;
using ZebraDesktop.Views;

namespace ZebraDesktop.ViewModels
{
    public class ConfigSelectorViewModel : ViewModelBase
    {
        #region Properties
        //private ZebraConfig _loadedConfiguration = null;
        //public ZebraConfig LoadedConfiguration { get => _loadedConfiguration; set { _loadedConfiguration = value; NotifyPropertyChanged(); } }

        private App _currentApp;
        public App CurrentApp
        {
            get { return _currentApp; }
            set { _currentApp = value; NotifyPropertyChanged(); }
        }

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

        private IClosable _parentContainer;
        public IClosable ParentContainer
        {
            get { return _parentContainer; }
            set { _parentContainer = value; NotifyPropertyChanged();}
        }

        private IDialogPrompt _dialog;

        public IDialogPrompt Dialog
        {
            get { return _dialog; }
            set { _dialog = value; NotifyPropertyChanged();  }
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

            CurrentApp = (App)Application.Current;

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
            CloseParentContainer();
        }

        private bool canExecuteLoadConfigCommand(object obj)
        {
            return SelectedFile != null;
        }

        private void ExecuteLoadConfigCommand(object obj)
        {
            LoadConfig();
            CloseParentContainer();
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
            try
            {
                foreach (var info in Directory.GetFiles("configs", "*.zebraconfig"))
                {
                    ConfigFiles.Add(new FileInfo(info));
                };
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Konfigurationen!\n {ex.Message}");
            }

        }

        private void LoadConfig()
        {
            //try
            //{
                CurrentApp.ConfigurationService.UnloadConfig();
                CurrentApp.ConfigurationService.LoadConfigurationFromFile(SelectedFile);
                Dialog.SetDialogResult(true);
                MessageBox.Show("Konfiguration erfolgreich geladen!");
                CloseParentContainer();
                return;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Fehler beim Laden der Konfiguration");
            //}
        }

        private void CloseParentContainer()
        {
            ParentContainer.Close();
        }
        #endregion
    }
}
