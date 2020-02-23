using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library
{
    public partial class Piece
    {

        /// <summary>
        /// Additional Public constructor with required data
        /// </summary>
        /// <param name="name">Name of the Piece</param>
        /// <param name="arranger">Name of the Arranger</param>
        public Piece(string name, string arranger)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            this.Name = name;
            this.Arranger = arranger;

            this.Sheet = new System.Collections.Generic.List<global::Zebra.Library.Sheet>();
            this.SetlistItem = new System.Collections.Generic.List<global::Zebra.Library.SetlistItem>();

            Init();
        }

        /// <summary>
        /// Additional Static create function (for use in LINQ queries, etc.)
        /// </summary>
        /// <param name="name">Name of the Piece</param>
        /// <param name="arranger">Name of the arranger</param>
        public static Piece Create(string name, string arranger)
        {
            return new Piece(name, arranger);
        }

    }
}
