using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Zebra.DatabaseAccess;
using Zebra.Library;

namespace Konsolenanwendung
{
    class Program
    {        
        private static Dictionary<string, ZebraCommand> cmdlist2 = new Dictionary<string, ZebraCommand>
        {

            ["createpiece"] = new ZebraCommand(
                "createpiece",
                    new ValueTuple<string, Type>[]
                    {
                        new ValueTuple<string, Type>("Name", typeof(String)),
                        new ValueTuple<string, Type>("Arranger", typeof(String))
                    },
                "Creates new Piece.",
                createpiece),

            ["help"] = new ZebraCommand(
                "help",
                null,
                "Shows all Commands",
                help),

            ["createpart"] = new ZebraCommand(
                "createpart",
                    new ValueTuple<string, Type>[]
                    {
                        new ValueTuple<string, Type>("Name", typeof(String))
                    },
                "Creates new Part.",
                createpart),

            ["createsheet"] = new ZebraCommand(
                "createsheet",
                    new ValueTuple<string, Type>[]
                    {
                        new ValueTuple<string, Type>("PieceID", typeof(int)),
                        new ValueTuple<string, Type>("PartID", typeof(int)),
                        new ValueTuple<string, Type>("PDF Path", typeof(FileInfo))
                    },
                "Creates new Sheet.",
                createsheet),

            ["exit"] = new ZebraCommand(
                "exit",
                 null,
                "Exits the Console",
                exit),

            ["listpieces"] = new ZebraCommand(
                "listpieces",
                 null,
                "Lists all Pieces",
                listpieces),

            ["listparts"] = new ZebraCommand(
                "listparts",
                 null,
                "Lists all Parts",
                listparts),

            ["listsheets"] = new ZebraCommand(
                "listsheets",
                 null,
                "Lists all Sheets",
                listsheets),

            ["clear"] = new ZebraCommand(
                "clear",
                 null,
                "Clears the Console Window",
                clear),

            ["findpiecebyname"] = new ZebraCommand(
                "findpiecebyname",
                    new ValueTuple<string, Type>[]
                    {
                        new ValueTuple<string, Type>("Searchstring", typeof(String)),
                    },
                "Lists all Pieces that contain the Searchstring",
                findpiecebyname),

            ["piecedetail"] = new ZebraCommand(
                "piecedetail",
                    new ValueTuple<string, Type>[]
                    {
                        new ValueTuple<string, Type>("PieceID", typeof(int)),
                    },
                "Shows Detail to specific piece",
                piecedetail),

            ["printstickersheet"] = new ZebraCommand(
                "printstickersheet",
                    new ValueTuple<string, Type>[]
                    {
                        new ValueTuple<string, Type>("PieceID", typeof(int)),
                    },
                "Prints a Stickersheet for Piece",
                printstickersheet),



            //["createpart"] = new ZebraCommand2("createpart", ("Name", String), );
            //["createpiece"] = new ZebraCommand2("createpiece", new Tuple<string, Type>[] { new Tuple<string, Type>("Name", typeof(String)), new Tuple<string, Type>("Arranger", typeof(String)) }, createpiece)
        };

        private static ZebraDBManager manager = new ZebraDBManager();
        private static ArchiveManager archiveManager = new Zebra.Library.ArchiveManager();

        private static void HLine() => Console.WriteLine("-".PadRight(Console.BufferWidth, '-'));

        static void Main(string[] args)
        {            
            var parser = new ZebraCommandParser(cmdlist2);

            Console.WriteLine("Zebra Console");
            Console.WriteLine("Type help for a list of all commands");
            Console.WriteLine("Parameters have to be seperated by a semicolon (;)");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Moin");

            while (true)
            {
                Console.Write("zebra>");
                var input = Console.ReadLine();

                if (parser.Parse(input, out ZebraCommand cmd, out string[] cmdargs))
                {
                    if (cmdargs == null) cmd.Execute(); else cmd.Execute(cmdargs);
                }

            }
            
        }

        #region Output Methods


        static void ListAllPieces()
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
                String.Format("{0,-30}", piece.Name) +
                String.Format("{0,-30}", piece.Arranger));
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

        static void PrintStickerSheet(Piece piece)
        {
            Console.WriteLine();
            Console.WriteLine($"Printing Stickersheet for '{piece.Name}'");
            Zebra.Archiving.Stickersheet stickersheet = new Zebra.Archiving.Stickersheet(Zebra.Archiving.StickersheetTemplates.AVERYL4732REV);

            stickersheet.PopulateWithZebraItems(piece, manager.GetAllParts());

            stickersheet.GeneratePDF();
            stickersheet.SavePDF(piece.PieceID + ".pdf");
        }

        #endregion

        #region Console Command Methods


        static void help()
        {
            Console.WriteLine();
            HLine();
            Console.WriteLine("Valid Commands:\n");

            List<ZebraCommand> commandlist = cmdlist2.Values.ToList<ZebraCommand>();
            commandlist.Sort();

            foreach (ZebraCommand item in commandlist)
            {
                string arguments = "";

                if (item.Arguments == null) arguments = "";
                else
                {
                    foreach (var arg in item.Arguments)
                    {
                        arguments += arg.Item1;
                        arguments += ", ";
                    }
                }
                Console.WriteLine($"{item.Command.PadRight(20, ' ')}{arguments.PadRight(30, ' ')}{item.Helptext}");
            }

            Console.WriteLine();
            Console.WriteLine("The arguments have to be seperated by a semicolon (;)");
            HLine();
            Console.WriteLine();
        }

        static void createpiece(object[] args) => manager.NewPiece(args[0].ToString(), args[1].ToString());

        static void createpart(object[] args) => manager.NewPart(args[0].ToString());

        static void createsheet(object[] args)
        {
            var pdf = new FileInfo(args[2].ToString());

            if (pdf.Exists && pdf.Extension == ".pdf")
            {
                manager.NewSheet(Int16.Parse(args[0].ToString()), Int16.Parse(args[1].ToString()));
                archiveManager.StoreSheet(pdf, manager.GetSheet(manager.GetPieceByID(Int16.Parse(args[0].ToString())), manager.GetPartByID(Int16.Parse(args[1].ToString()))));
                Console.WriteLine("PDF erfolgreich abgelegt!");
                Console.WriteLine();
            }
        }

        static void exit() => Environment.Exit(0);

        static void clear() => Console.Clear();

        static void findpiecebyname(object[] args) => ListFoundPieces(manager.FindPiecesByName(args[0].ToString()), args[0].ToString());

        static void piecedetail(object[] args) => ShowPieceDetail(manager.GetPieceByID(Int32.Parse(args[0].ToString())));

        static void printstickersheet(object[] args) => PrintStickerSheet(manager.GetPieceByID(Int32.Parse(args[0].ToString())));

        static void listpieces() => ListAllPieces();

        static void listparts() => ListAllParts();

        static void listsheets() => ListAllSheets();

        #endregion
    }
}
