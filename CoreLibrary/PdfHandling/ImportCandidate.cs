using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Zebra.Library.PdfHandling
{
    public class ImportCandidate : INotifyPropertyChanged
    {
        private Guid _documentId;

        [JsonPropertyName("documentid")]
        public Guid DocumentId
        {
            get { return _documentId; }
            set { _documentId = value; }
        }

        public string FileName { get; set; }

        private ObservableCollection<ImportPage> pages;

        [JsonPropertyName("pages")]
        public ObservableCollection<ImportPage> Pages
        {
            get { return pages; }
            set { pages = value; NotifyPropertyChanged(); }
        }

        [JsonIgnore]
        public bool IsAssigned 
        {
            get { return (assignedPiece != null && assignedPart != null); }
        }

        
        private PieceDTO assignedPiece;
        public PieceDTO AssignedPiece {
            get { return assignedPiece; }
            set { assignedPiece = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(IsAssigned)); }
        }


        private PartDTO assignedPart;
        public PartDTO AssignedPart
        {
            get { return assignedPart; }
            set { assignedPart = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(IsAssigned)); }
        }


        public ImportCandidate(Guid _DocumentId, IList<ImportPage> pages)
        {
            Pages = new ObservableCollection<ImportPage>(pages);
            DocumentId = _DocumentId;

            foreach (var page in pages)
            {
                page.ImportCandidate = this;
            }
        }

        public ImportCandidate()
        {

        }

        public ImportCandidate Split(int newFirstPage, bool TakeOverAssignedPiece = false)
        {

            var newPages = new List<ImportPage>();
            var newImportCandidate = new ImportCandidate(this.DocumentId, newPages) ;

            //Set static, because Pages.Count decreases with each loop
            var pagecount = Pages.Count;

            for (int i = newFirstPage; i < pagecount; i++)
            {
                // Always same pageIndex, because page gets removed and following page indices decrease
                var page = this.Pages[newFirstPage];

                // Set new parent for the page and add to new Import Candidate
                page.ImportCandidate = newImportCandidate;
                newImportCandidate.Pages.Add(page);

                // Remove page from old ImportCandidate
                this.Pages.Remove(page);

            }

            if (TakeOverAssignedPiece == true)
            {
                newImportCandidate.AssignedPiece = this.AssignedPiece;
            }

            return newImportCandidate;
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

    public static class ImportCandidateExtension
    {
        public static ImportCandidate LoadThumbnails(this ImportCandidate ic, string filepath)
        {
            PreviewablePdfDocument doc = new PreviewablePdfDocument(filepath);
            for (int i = 0; i < ic.Pages.Count; i++)
            {
                ic.Pages[i].Thumbnail = doc.Pages[i].Thumbnail;
            }
            return ic;
        }
    }
}
