using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows;
using Zebra.Library;
using ZebraDesktop.Views;

namespace ZebraDesktop.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Properties
        private App _currentApp;
        public App CurrentApp
        {
            get { return _currentApp; }
            set { _currentApp = value; NotifyPropertyChanged(); }
        }

        private PartsPage partsPage;

        public PartsPage PartsPage
        {
            get { return partsPage; }
            set { partsPage = value; NotifyPropertyChanged(); }
        }

        private PiecesPage _piecesPage;

        public PiecesPage PiecesPage
        {
            get { return _piecesPage; }
            set { _piecesPage = value; NotifyPropertyChanged(); }
        }

        private PiecesPageViewModel _piecesPageViewModel;

        public PiecesPageViewModel PiecesPageViewModel
        {
            get { return _piecesPageViewModel; }
            set { _piecesPageViewModel = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _loadConfigCommand;

        public DelegateCommand LoadConfigCommand
        {
            get { return _loadConfigCommand; }
            set { _loadConfigCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _unloadConfigCommand;

        public DelegateCommand UnloadConfigCommand
        {
            get { return _unloadConfigCommand; }
            set { _unloadConfigCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _newConfigCommand;

        public DelegateCommand NewConfigCommand
        {
            get { return _newConfigCommand; }
            set { _newConfigCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _exitCommand;

        public DelegateCommand ExitCommand
        {
            get { return _exitCommand; }
            set { _exitCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _addPieceCommand;

        public DelegateCommand AddPieceCommand
        {
            get { return _addPieceCommand; }
            set { _addPieceCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _editPieceCommand;

        public DelegateCommand EditPieceCommand
        {
            get { return _editPieceCommand; }
            set { _editPieceCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _deletePieceCommand;

        public DelegateCommand DeletePieceCommand
        {
            get { return _deletePieceCommand; }
            set { _deletePieceCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _importPDFBatchCommand;

        public DelegateCommand ImportPDFBatchCommand
        {
            get { return _importPDFBatchCommand; }
            set { _importPDFBatchCommand = value; NotifyPropertyChanged(); }
        }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            CurrentApp = (App)Application.Current;

            //Setup commands
            LoadConfigCommand = new DelegateCommand(ExecuteLoadConfigCommand, canExecuteLoadConfigCommand);
            UnloadConfigCommand = new DelegateCommand(ExecuteUnloadConfigCommand, canExecuteUnloadConfigCommand);
            NewConfigCommand = new DelegateCommand(ExecuteNewConfigCommand);
            ExitCommand = new DelegateCommand((object obj) => { Application.Current.Shutdown(); });
            AddPieceCommand = new DelegateCommand(ExecuteNewPieceCommand, CanExecuteNewPieceCommand);
            EditPieceCommand = new DelegateCommand(ExecuteEditPieceCommand, CanExecuteEditPieceCommand);
            DeletePieceCommand = new DelegateCommand(ExecuteDeletePieceCommand, canExecuteDeletePieceCommand);
            ImportPDFBatchCommand = new DelegateCommand(ExecuteImportPDFBatchCommand, canExecuteImportPDFBatchCommand);
            
        }

        
        #endregion

        #region Interfaces

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Commands

        private bool canExecuteLoadConfigCommand(object obj) { return true; }

        private void ExecuteLoadConfigCommand(object obj)
        { LoadConfig(); }

        private bool canExecuteUnloadConfigCommand(object obj) { return (CurrentApp.Manager != null); }

        private void ExecuteUnloadConfigCommand(object obj)
        {
            UnloadConfig();
        }
        private void ExecuteNewConfigCommand(object obj)
        {
            frmNewConfig frm = new frmNewConfig();
            frm.Show();
        }
        private bool canExecuteDeletePieceCommand(object obj)
        {
            return PiecesPageViewModel?.SelectedPiece != null;
        }

        private void ExecuteDeletePieceCommand(object obj)
        {
            var pc = PiecesPageViewModel?.SelectedPiece;

            switch (MessageBox.Show($"Möchten Sie den Notensatz #{pc.PieceID} - {pc.Name} - {pc.Arranger} und alle zugehörigen Notenblätter wirklich löschen? Die Änderung kann nicht rückgängig gemacht werden!", "Löschen bestätigen", MessageBoxButton.YesNo, MessageBoxImage.Warning))
            {
                case MessageBoxResult.Yes:

                    foreach (Sheet sht in pc.Sheet)
                    {
                        CurrentApp.Manager.Context.Remove<Sheet>(sht);
                    }

                    CurrentApp.Manager.Context.Remove<Piece>(pc);

                    break;
            }
        }
        private bool canExecuteImportPDFBatchCommand(object obj)
        {
            return (CurrentApp.Manager != null);
        }

        private void ExecuteImportPDFBatchCommand(object obj)
        {
            frmPdfBatchImporter frm = new frmPdfBatchImporter(CurrentApp.Manager.Context);
            frm.Show();
        }

        private bool CanExecuteNewPieceCommand(object obj)
        {
            return CurrentApp.Manager != null;
        }

        private void ExecuteNewPieceCommand(object obj)
        {
            frmNewPiece frm = new frmNewPiece();
            frm.Show();
        }

        private bool CanExecuteEditPieceCommand(object obj)
        {
            return (PiecesPageViewModel?.SelectedPiece != null);
        }

        private void ExecuteEditPieceCommand(object obj)
        {
            frmPieceDetail frm = new frmPieceDetail(PiecesPageViewModel?.SelectedPiece);
            frm.Show();
        }


        /// <summary>
        /// Manually trigger the RaiseCanExecuteChanged method for all DelegateCommands
        /// </summary>
        private void UpdateButtonStatus()
        {
            foreach (System.Reflection.PropertyInfo prop in this.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(DelegateCommand))
                {
                    var func = prop.GetValue(this);                       
                    (func as DelegateCommand).RaiseCanExecuteChanged();
                }
            }
        }
        #endregion

        #region Methods

        private void LoadConfig()
        {
            ConfigSelector frmConfigSelector = new ConfigSelector();
            frmConfigSelector.ShowDialog();


            if (!(CurrentApp.Manager == null))
            {
                UnloadConfig();
            }

            if (frmConfigSelector.DialogResult == true)
            {
                var conf = frmConfigSelector.SelectedConfiguration;
                CurrentApp.ZebraConfig = conf;

                PiecesPageViewModel = new PiecesPageViewModel();
                //PartsPageViewModel = new PartsPageViewModel();

                PiecesPage = new PiecesPage();
                PiecesPage.DataContext = PiecesPageViewModel;
                PartsPage = new PartsPage(CurrentApp.Manager.Context);
                PartsPage.DataContext = PiecesPage;
            }

            UpdateButtonStatus();
        }

        private void UnloadConfig()
        {
            CurrentApp.ZebraConfig = null;
            MessageBox.Show("Konfiguration erfolgreich geschlossen", "Konfiguration geschlossen");

            UpdateButtonStatus();
        }

        #endregion


    }
}
