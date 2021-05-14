using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library
{
    public class SheetDTO
    {

        public int SheetID { get; set; }

        public string Name { get; set; }

        public int PieceID { get; set; }

        public string PieceName { get; set; }

        public int PartID { get; set; }

        public string PartName { get; set; }

        public DateTime LastUpdate { get; set; }

    }
}
