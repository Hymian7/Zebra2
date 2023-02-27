using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Zebra.Library;

namespace ZebraDesktop.ViewModels
{
    public class PartDetailViewModel : ViewModelBase
    {

        #region Properties

        private PartDTO _currentPart;

        public PartDTO CurrentPart
        {
            get { return _currentPart; }
            set { _currentPart = value; NotifyPropertyChanged(); }
        }

        public IZebraDBManager Manager
        { get { return ((Application.Current) as App).Manager; } }


        #endregion

        #region Constructors

        public PartDetailViewModel(PartDTO part)
        {
            CurrentPart = part;
        }

        public PartDetailViewModel()
        {

        }

        #endregion

        #region Commands

        #endregion

    }
}
