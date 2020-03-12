using System;
using System.Collections.Generic;

namespace Konsolenanwendung
{
    internal class ZebraCommandParser
    {
        public Dictionary<string, ZebraCommand> ValidCommands { get; private set; }

        public ZebraCommandParser(IDictionary<string, ZebraCommand> commands)
        {
            ValidCommands = new Dictionary<string, ZebraCommand>();

            foreach (var cmd in commands)
            {
                ValidCommands.Add(cmd.Key, cmd.Value);
            }
        }

        public bool Parse(string input, out ZebraCommand outCommand, out string[] outArgs)
        {
            //Split String into Command and Arguments
            string cmd;
            string[] args;

            cmd = input.Split(' ')[0];

            args = input.Substring(input.IndexOf(' ', StringComparison.OrdinalIgnoreCase) + 1, input.Length - 1 - input.IndexOf(' ', StringComparison.OrdinalIgnoreCase)).Split(';');

            //If no argumens are given, set them Null
            if (args[0] == cmd) args = null;

            //If command is not in List of valid Commands, print error and return false
            if (!ValidCommands.ContainsKey(cmd)) { Console.WriteLine($"Unknown Command '{cmd}'\n"); outCommand = null; outArgs = null; return false; }

            //Test, if Number of Arguments in the Input String = Number of Arguments in ValidCommands List
            ZebraCommand command = ValidCommands[cmd];

            if (args == null ^ (command.Arguments == null)) { Console.WriteLine($"(Null Error) Invalid Number of Arguments. '{command.Command}' requires {command.Arguments.Length} Arguments. \n"); outCommand = null; outArgs = null; return false; }

            //Only check further if args is not null
            if (!(args == null))
            {
                //If one of the arguments is empty, show Error
                foreach (var item in args)
                {
                    if (string.IsNullOrWhiteSpace(item))
                    {
                        Console.WriteLine("Error: One ore more of the arguments were empty.\n");
                        outCommand = null; outArgs = null; return false;
                    }
                }

                if (!(command.Arguments.Length == args.Length)) { Console.WriteLine($"Invalid Number of Arguments. '{command.Command}' requires {command.Arguments.Length} Arguments. \n"); outCommand = null; outArgs = null; return false; }

                //Test, if Types of Arguments in the Input String are the same as in the valid Command
                for (int i = 0; i < command.Arguments.Length; i++)
                {
                    if (!CanConvert(args[i], command.Arguments[i].Item2))
                    {
                        Console.WriteLine($"The argument #{i} '{args[i]}' cannot be converted to the required type '{command.Arguments[i].Item2}'\n");
                        outCommand = null;
                        outArgs = null;
                        return false;
                    }
                }
            }

            outCommand = command;
            outArgs = args;
            return true;

            //Checks if Conversion is valid
            bool CanConvert(string str, Type type)
            {
                try
                {
                    var obj = Convert.ChangeType(str, type);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

    }
}
