using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Zebra.Library
{
    public class SetlistDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("setlistitems")]
        public List<SetlistItemDTO> SetlistItems { get; set; }
    }
}
