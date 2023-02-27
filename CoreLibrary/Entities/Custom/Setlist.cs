using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zebra.Library
{
    partial class Setlist
    {
        public Setlist(string name) : this()
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            this._Name = name;
        }

    }
}
