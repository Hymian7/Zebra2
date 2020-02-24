using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Konsolenanwendung
{
    class ZebraCommand : IComparable<ZebraCommand>
    {
        public string Command { get; set; }

        public string Helptext { get; set; }

        public ZebraCommand(string command, string helptext)
        {
            this.Command = command;
            this.Helptext = helptext;
        }

        public int CompareTo([AllowNull] ZebraCommand other)
        {
            return this.Command.CompareTo(other.Command);
        }
    }
}
