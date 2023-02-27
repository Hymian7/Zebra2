using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using Zebra.Library;

namespace ZebraDesktop.ViewModels
{
    public class NewPieceViewModel : ViewModelBase
    {
        #region Properties

        private Piece _piece;

        public Piece Piece
        {
            get { return _piece; }
            set { _piece = value; NotifyPropertyChanged(); }
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

        public NewPieceViewModel()
        {
            Piece = new Piece("<Neues Stück>");

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
            return !String.IsNullOrEmpty(Piece.Name); 
        }

        private void executeSaveCommand(object obj)
        {
            throw new NotImplementedException();
            //Manager.Context.Add<Piece>(this.Piece);
            //Manager.Context.SaveChanges();
            //(obj as Window).Close();
        }

        #endregion

    }
}
