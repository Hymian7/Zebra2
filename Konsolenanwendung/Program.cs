using System;
using Zebra.Library;
using Zebra.DatabaseAccess;
using System.Linq;
using System.Collections.Generic;

namespace Konsolenanwendung
{
    class Program
    {
        private static List<ZebraCommand> cmdlist = new List<ZebraCommand> {
            new ZebraCommand("createpiece", "Creates new Piece. Args: Name and Arranger"),
            new ZebraCommand("createpart", "Creates new Part. Args: Name"),
            new ZebraCommand("createsheet", "Creates new Sheet. Args: PieceID, SheetID"),
            new ZebraCommand("exit","Exits the console."),
            new ZebraCommand("listpieces","Lists all Pieces"),
            new ZebraCommand("listparts","Lists all Parts"),
            new ZebraCommand("listsheets", "Lists all Sheets"),
            new ZebraCommand("clear", "Clears the Console Window"),
            new ZebraCommand("help", "Shows all Commands"),
            new ZebraCommand("findpiecebyname","Lists all Pieces that contain the Searchstring. Args: Searchstring"),
            new ZebraCommand("piecedetail","Shows Detail to specific piece. Args: PieceID")};

        private static ZebraDBManager manager = new ZebraDBManager();

        private static void HLine() => Console.WriteLine("-".PadRight(Console.BufferWidth, '-'));

        static void Main(string[] args)
        {
            cmdlist.Sort();

            Console.WriteLine("Zebra Console");
            Console.WriteLine("Type help for a list of all commands");
            Console.WriteLine("Parameters have to be seperated by a semicolon (;)");
            Console.WriteLine();

            while (true)
            {
                Console.Write("zebra>");
                var input = Console.ReadLine();

                if (input.StartsWith("createpiece"))
                {
                    string[] cpargs = input.Substring(input.IndexOf(' ', StringComparison.OrdinalIgnoreCase) + 1, input.Length - 1 - input.IndexOf(' ', StringComparison.OrdinalIgnoreCase)).Split(';');
                    manager.NewPiece(cpargs[0], cpargs[1]);
                }
                else if (input.StartsWith("createpart"))
                {
                    string[] cpargs = input.Substring(input.IndexOf(' ', StringComparison.OrdinalIgnoreCase) + 1, input.Length - 1 - input.IndexOf(' ', StringComparison.OrdinalIgnoreCase)).Split(';');
                    manager.NewPart(cpargs[0]);
                }
                else if (input.StartsWith("createsheet"))
                {
                    string[] cpargs = input.Substring(input.IndexOf(' ', StringComparison.OrdinalIgnoreCase) + 1, input.Length - 1 - input.IndexOf(' ', StringComparison.OrdinalIgnoreCase)).Split(';');
                    manager.NewSheet(Int16.Parse(cpargs[0]), Int16.Parse(cpargs[1]));
                }
                else if (input.StartsWith("exit")) return;
                else if (input.StartsWith("clear")) Console.Clear();
                else if (input.StartsWith("listpieces")) ListAlllPieces();
                else if (input.StartsWith("listparts")) ListAllParts();
                else if (input.StartsWith("listsheets")) ListAllSheets();
                else if (input.StartsWith("help")) ShowHelp();
                else if (input.StartsWith("findpiecebyname"))
                {
                    string searchstring = input.Substring("findpiecebyname".Length);

                    if (String.IsNullOrWhiteSpace(searchstring))
                    {
                        Console.WriteLine();
                        Console.WriteLine("String to find:");
                        searchstring = Console.ReadLine();
                    }

                    ListFoundPieces(manager.FindPiecesByName(searchstring), searchstring);
                }
                else if (input.StartsWith("piecedetail"))
                { 
                    
                    string[] pdargs = input.Split();
                    ShowPieceDetail(manager.GetPieceByID(Int32.Parse(pdargs[1])));
                        
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Command not found.");
                    Console.WriteLine("Type 'help' for a list of all commands.");
                }


            }
        }

        static void ListAlllPieces()
        {
            var pieces = manager.GetAllPieces();

            Console.WriteLine();
            Console.WriteLine("{0,-65}", "Table Pieces");
            Console.WriteLine();
            HLine();
            Console.WriteLine(
                String.Format("{0,-5}", "ID") +
                String.Format("{0,-30}", "Name") +
                String.Format("{0,-30}", "Arranger"));
            HLine();

            foreach (Piece ns in pieces)
            {
                Console.WriteLine(
                String.Format("{0,-5}", ns.PieceID) +
                String.Format("{0,-30}", ns.Name) +
                String.Format("{0,-30}", ns.Arranger));
            }

            HLine();
            Console.WriteLine("Number of Pieces: " + pieces.Count);


        }

        static void ListAllSheets()
        {
            var notenblattset = manager.GetAllSheets();

            Console.WriteLine();
            Console.WriteLine("{0,-65}", "Table Sheets");
            Console.WriteLine();
            HLine();
            Console.WriteLine(
                String.Format("{0,-5}", "ID") +
                String.Format("{0,-30}", "Piece") +
                String.Format("{0,-30}", "Part"));
            HLine();

            foreach (Sheet nb in notenblattset)
            {
                Console.WriteLine(
                String.Format("{0,-5}", nb.SheetID) +
                String.Format("{0,-30}", nb.Piece.Name) +
                String.Format("{0,-30}", nb.Part.Name));
            }

            HLine();
            Console.WriteLine("Number of Sheets: " + notenblattset.Count);
        }

        static void ListAllParts()
        {
            var stimmenset = manager.GetAllParts();

            Console.WriteLine();
            Console.WriteLine("{0,-65}", "Table Parts");
            Console.WriteLine();
            HLine();
            Console.WriteLine(
                String.Format("{0,-5}", "ID") +
                String.Format("{0,-30}", "Name"));
            HLine();

            foreach (Part pc in stimmenset)
            {
                Console.WriteLine(
                String.Format("{0,-5}", pc.PartID) +
                String.Format("{0,-30}", pc.Name));
            }

            HLine();
            Console.WriteLine("Number of Parts: " + stimmenset.Count);


        }

        static void ListFoundPieces(List<Piece> pieces, string searchstring)
        {
            Console.WriteLine();
            Console.WriteLine($"Found Pieces for '{searchstring}':");
            Console.WriteLine();
            Console.WriteLine("{0,-65}", "Table Pieces");
            Console.WriteLine();
            HLine();
            Console.WriteLine(
                String.Format("{0,-5}", "ID") +
                String.Format("{0,-30}", "Name") +
                String.Format("{0,-30}", "Arranger"));
            HLine();

            foreach (Piece ns in pieces)
            {
                Console.WriteLine(
                String.Format("{0,-5}", ns.PieceID) +
                String.Format("{0,-30}", ns.Name) +
                String.Format("{0,-30}", ns.Arranger));
            }

            HLine();
            Console.WriteLine("Number of Pieces: " + pieces.Count);
            Console.WriteLine();
        }

        static void ShowPieceDetail(Piece piece)
        {
            Console.WriteLine();
            Console.WriteLine("Show Details for Piece:");
            Console.WriteLine();
            HLine();
            Console.WriteLine(
                String.Format("{0,-5}", "ID") +
                String.Format("{0,-30}", "Name") +
                String.Format("{0,-30}", "Arranger"));
            HLine();
            Console.WriteLine(
                String.Format("{0,-5}", piece.PieceID) +
                String.Format("{0,-30}",piece.Name) +
                String.Format("{0,-30}",piece.Arranger));
            HLine();

            Console.WriteLine();
            Console.WriteLine("List of the Connected Sheets:");
            Console.WriteLine();
            HLine();
            Console.WriteLine(
                String.Format("{0,-5}", "ID") +
                String.Format("{0,-30}", "Piece") +
                String.Format("{0,-30}", "Part"));
            HLine();
            foreach (Sheet nb in piece.Sheet.ToList<Sheet>())
            {
                Console.WriteLine(
                String.Format("{0,-5}", nb.SheetID) +
                String.Format("{0,-30}", nb.Piece.Name) +
                String.Format("{0,-30}", nb.Part.Name));
            }
            HLine();
        }

        static void ShowHelp()
        {
            Console.WriteLine();
            HLine();
            Console.WriteLine("Valid Commands:");
            foreach (ZebraCommand cmd in cmdlist) Console.WriteLine($"{cmd.Command.PadRight(20, ' ')}{cmd.Helptext}");
            Console.WriteLine();
            Console.WriteLine("The arguments have to be seperated by a semicolon (;)");
            HLine();
            Console.WriteLine();
        }
    }
}
