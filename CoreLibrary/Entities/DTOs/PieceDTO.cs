using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Zebra.Library
{
    public class PieceDTO
    {
        [JsonPropertyName("pieceid")]
        public int PieceID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("arranger")]
        public string Arranger { get; set; }

        [JsonPropertyName("sheet")]
        public List<SheetDTO> Sheet { get; set; }

        [JsonPropertyName("setlist")]
        public List<SetlistDTO> Setlist { get; set; }
    }
}
