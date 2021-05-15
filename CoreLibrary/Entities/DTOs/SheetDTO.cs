using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Zebra.Library
{
    public class SheetDTO
    {
        [JsonPropertyName("sheetid")]
        public int SheetID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("pieceid")]
        public int PieceID { get; set; }
        [JsonPropertyName("piecename")]
        public string PieceName { get; set; }
        [JsonPropertyName("partide")]
        public int PartID { get; set; }
        [JsonPropertyName("partname")]
        public string PartName { get; set; }
        [JsonPropertyName("lastupdate")]
        public DateTime LastUpdate { get; set; }

    }
}
