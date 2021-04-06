using Microsoft.EntityFrameworkCore;
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
    public class MainWindowViewModel : ViewModelBase
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

        private PartsPageViewModel _partsPageViewModel;

        public PartsPageViewModel PartsPageViewModel
        {
            get { return _partsPageViewModel; }
            set { _partsPageViewModel = value; NotifyPropertyChanged(); }
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

        private DelegateCommand _addPartCommand;

        public DelegateCommand AddPartCommand
        {
            get { return _addPartCommand; }
            set { _addPartCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _editPartCommand;

        public DelegateCommand EditPartCommand
        {
            get { return _editPartCommand; }
            set { _editPartCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _deletePartCommand;

        public DelegateCommand DeletePartCommand
        {
            get { return _deletePartCommand; }
            set { _deletePartCommand = value; NotifyPropertyChanged(); }
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

            AddPartCommand = new DelegateCommand(ExecuteNewPartCommand, CanExecuteNewPartCommand);
            EditPartCommand = new DelegateCommand(ExecuteEditPartCommand, CanExecuteEditPartCommand);
            DeletePartCommand = new DelegateCommand(ExecuteDeletePartCommand, canExecuteDeletePartCommand);


            ImportPDFBatchCommand = new DelegateCommand(ExecuteImportPDFBatchCommand, canExecuteImportPDFBatchCommand);
            
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

        private bool canExecuteDeletePartCommand(object obj)
        {
            return (PartsPageViewModel?.SelectedPart != null);
        }

        private void ExecuteDeletePartCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanExecuteEditPartCommand(object obj)
        {
            return (PartsPageViewModel?.SelectedPart != null);
        }

        private void ExecuteEditPartCommand(object obj)
        {
            frmPartDetail frm = new frmPartDetail(PartsPageViewModel?.SelectedPart);
            frm.Show();
        }

        private bool CanExecuteNewPartCommand(object obj)
        {
            return CurrentApp.Manager != null;
        }

        private void ExecuteNewPartCommand(object obj)
        {
            frmNewPart frm = new frmNewPart();
            frm.Show();
        }
        #endregion

        #region Methods

        private async void LoadConfig()
        {
            ConfigSelector frmConfigSelector = new ConfigSelector();
            frmConfigSelector.DataContext = new ConfigSelectorViewModel();
            frmConfigSelector.ShowDialog();


            if (!(CurrentApp.Manager == null))
            {
                UnloadConfig();
            }

            if ((frmConfigSelector.DataContext as ConfigSelectorViewModel).LoadedConfiguration != null)
            {
                var conf = (frmConfigSelector.DataContext as ConfigSelectorViewModel).LoadedConfiguration;
                CurrentApp.ZebraConfig = conf;

                await CurrentApp.Manager.EnsureDatabaseCreatedAsync();
                // TODO: Check for pending migrations
                
                //await CurrentApp.Manager.Context.Database.GetPendingMigrationsAsync();


                //TODO: Make Creation of ViewModels Async
                PiecesPageViewModel = new PiecesPageViewModel();
                PartsPageViewModel = new PartsPageViewModel();
                
                //Register Eventhandler for Button States to be updated
                PiecesPageViewModel.PropertyChanged += ChildSelectionChanged;
                PartsPageViewModel.PropertyChanged += ChildSelectionChanged;

                PiecesPage = new PiecesPage();
                PiecesPage.DataContext = PiecesPageViewModel;
                PartsPage = new PartsPage();
                PartsPage.DataContext = PartsPageViewModel;
            }

            UpdateButtonStatus();
        }


        private void UnloadConfig()
        {
            // Cleanup
            CurrentApp.ZebraConfig = null;

            PiecesPage = null;
            PartsPage = null;

            PiecesPageViewModel = null;
            PartsPageViewModel =  null;

            MessageBox.Show("Konfiguration erfolgreich geschlossen", "Konfiguration geschlossen");

            UpdateButtonStatus();
        }

        private void ChildSelectionChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateButtonStatus();
        }

        #endregion


    }
}
