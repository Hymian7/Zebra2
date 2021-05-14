using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library
{
    public class SetlistDTO
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public DateTime? Date { get; set; }

        public IList<SetlistItemDTO> SetlistItems { get; set; }
    }
}
