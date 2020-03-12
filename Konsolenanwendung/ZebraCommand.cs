using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Konsolenanwendung
{
        public class ZebraCommand : IComparable<ZebraCommand>
    {
        public String Command { get; private set; }
        public ValueTuple<string, Type>[] Arguments { get; private set; }
        public String Helptext { get; private set; }

        public readonly ZebraCommandDelegate _delegate;
        public readonly ZebraCommandDelegateArgs _delegateArgs;

              
        public ZebraCommand(string command, ValueTuple<string, Type>[] args, string helptext, ZebraCommandDelegateArgs delArgs)
        {
            Command = command;
            Arguments = args;
            Helptext = helptext;
            _delegateArgs = delArgs;
        }

        public ZebraCommand(string command, ValueTuple<string, Type>[] args, string helptext, ZebraCommandDelegate del)
        {
            Command = command;
            Arguments = args;
            Helptext = helptext;
            _delegate = del;
        }

        public void Execute(object[] args) => _delegateArgs(args);

        public void Execute() => _delegate();

        public int CompareTo([AllowNull] ZebraCommand other)
        {
            return this.Command.CompareTo(other.Command);
        }
    }

    public delegate void ZebraCommandDelegateArgs(params object[] args);
    public delegate void ZebraCommandDelegate();
}
