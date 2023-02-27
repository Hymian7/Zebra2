using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Zebra.Library;
using System.Collections;

namespace ZebraDesktop.ViewModels
{
    public class NewPartViewModel : ViewModelBase
    {

        #region Properties

        private PartDTO _part;

        public PartDTO Part
        {
            get { return _part; }
            set { _part = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _cancelCommand;

        public DelegateCommand CancelCommand
        {
            get { return _cancelCommand; }
            set { _cancelCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _saveCommand;

        public DelegateCommand SaveCommand
        {
            get { return _saveCommand; }
            set { _saveCommand = value; NotifyPropertyChanged(); }
        }

        public IZebraDBManager Manager
        { get { return ((Application.Current) as App).Manager; } }

        #endregion

        #region Constructors

        public NewPartViewModel()
        {
            Part = new PartDTO() { Name = "<Neue Stimme>" };

            CancelCommand = new DelegateCommand(executeCancelCommand, canExecuteCancelCommand);
            SaveCommand = new DelegateCommand(executeSaveCommand, canExecuteSaveCommand);
        }

        #endregion

        #region Commands

        private bool canExecuteCancelCommand(object obj)
        {
            return true;
        }

        private void executeCancelCommand(object obj)
        {
            (obj as Window).Close();
        }

        private bool canExecuteSaveCommand(object obj)
        {
            return !String.IsNullOrEmpty(Part.Name);
        }

        private void executeSaveCommand(object obj)
        {
            Manager.PostPartAsync(Part);
            (obj as Window).Close();
        }

        #endregion
    }
}
