using System;
using System.Collections.Generic;
using System.Text;
using Zebra.Library;

namespace ZebraDesktop
{
    public class ImportAssignment
    {
        public Piece Piece { get; set; }
        public Part Part { get; set; }
        public List<int> Pages { get; set; }

        public ImportAssignment(Piece _piece, Part _part, List<int> _pages)
        {
            Piece = _piece;
            Part = _part;
            Pages = _pages;

        }

    }
}
