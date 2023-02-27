using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Zebra.Library.PdfHandling
{
    public class ImportPage
    {
        [JsonPropertyName("pagenumber")]
        public int PageNumber { get; set; }

        [JsonIgnore]
        public System.Drawing.Image Thumbnail { get; set; }

        [JsonIgnore]
        public ImportCandidate ImportCandidate { get; set; }

        public ImportPage(int pageNumber, System.Drawing.Image thumbnail)
        {
            PageNumber = pageNumber;
            Thumbnail = thumbnail;
        }

        public ImportPage(int pageNumber)
        {
            PageNumber = pageNumber;
        }

        public ImportPage()
        {

        }

    }
}
