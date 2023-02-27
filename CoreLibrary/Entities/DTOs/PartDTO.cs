using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Zebra.Library
{
    public class PartDTO
    {
        [JsonPropertyName("partid")]
        public int PartID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("position")]
        public short? Position { get; set; }
        [JsonPropertyName("sheet")]
        public List<SheetDTO> Sheet { get; set; }
    }
}
