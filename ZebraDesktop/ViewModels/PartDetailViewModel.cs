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

        private Part _currentPart;

        public Part CurrentPart
        {
            get { return _currentPart; }
            set { _currentPart = value; }
        }

        public ZebraDBManager Manager
        { get { return ((Application.Current) as App).Manager; } }


        #endregion

        #region Constructors

        public PartDetailViewModel(Part part)
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
