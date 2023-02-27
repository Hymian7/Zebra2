using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library
{
    public class SetlistItemDTO
    {
        public int? Position { get; set; }
        public int SetlistItemID { get; set; }
        public int PieceID { get; set; }
        public string PieceName { get; set; }
    }
}
