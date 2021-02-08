using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Zebra.Library;

namespace Zebra.PdfHandling
{
    public class ImportCandidate : INotifyPropertyChanged
    {
        private SortedList<int, ImportPage> pages;

        public SortedList<int, ImportPage> Pages
        {
            get { return pages; }
            private set { pages = value; NotifyPropertyChanged(); }
        }


        private bool isAssigned;
        public bool IsAssigned 
        {
            get { return (assignedPiece != null && assignedPart != null); }
        }
            //set { isAssigned = value; NotifyPropertyChanged(); } }
        
        private Piece assignedPiece;
        public Piece AssignedPiece {
            get { return assignedPiece; }
            set { assignedPiece = value; NotifyPropertyChanged(); }
        }


        private Part assignedPart;
        public Part AssignedPart
        {
            get { return assignedPart; }
            set { assignedPart = value; NotifyPropertyChanged(); }
        }


        public ImportCandidate(SortedList<int,ImportPage> pages)
        {
            Pages = pages;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
