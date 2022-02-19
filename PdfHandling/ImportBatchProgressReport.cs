using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Zebra.PdfHandling;

namespace Zebra.PdfHandling
{
    public class ImportBatchProgressReport : INotifyPropertyChanged
    {
        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private int _percentage;
        private string message;

        public int Percentage
        {
            get { return _percentage; }
            set { _percentage = value; NotifyPropertyChanged(); }
        }

        public String Message
        {
            get { return message; }
            set { message = value; NotifyPropertyChanged(); }
        }

        public ImportCandidate LastImported { get; set; }

        public List<ImportCandidate> ImportedCandidates { get; set; }

        public ImportBatchProgressReport()
        {
            Percentage = 0;

            LastImported = null;
            ImportedCandidates = new List<ImportCandidate>();

            Message = "";
        }

        public void GenerateMessage(int currentPosition, int maxPosition)
        {
            Message = $"Importiere Seite {currentPosition} von {maxPosition}:";
        }


    }
}
