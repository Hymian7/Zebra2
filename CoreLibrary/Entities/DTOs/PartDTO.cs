using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library
{
    public class PartDTO
    {
        public int PartID { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }

        public List<PieceDTO> Piece { get; set; }
    }
}
