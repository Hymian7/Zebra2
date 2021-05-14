using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library
{
    public class PieceDTO
    {

        public int PieceID { get; set; }

        public string Name { get; set; }

        public string Arranger { get; set; }

        public List<PartDTO> Part { get; set; }

        public List<SetlistDTO> Setlist { get; set; }
    }
}
