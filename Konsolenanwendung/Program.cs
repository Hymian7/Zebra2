using System;
using Zebra.Library;
using Zebra.DatabaseAccess;
using System.Linq;
using System.Collections.Generic;

namespace Konsolenanwendung
{
    class Program
    {
        private static List<string> cmdlist = new List<string> {"createpiece", "createpart", "createsheet", "exit", "listpieces", "listparts", "listsheets", "clear", "help" };
        private static ZebraDBManager manager = new ZebraDBManager();

        static void Main(string[] args)
        {
            Console.WriteLine("Zebra Konsole");
            Console.WriteLine("Für alle Befehle help eingeben");
            Console.WriteLine("Parameter müssen durch ; getrennt sein.");
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
                else if (input.StartsWith("exit"))
                {
                    return;
                }
                else if (input.StartsWith("clear"))
                {
                    Console.Clear();
                }
                else if (input.StartsWith("listpieces"))
                {
                    ListAllPieces();
                }
                else if (input.StartsWith("listparts"))
                {
                    ListAllParts();
                }
                else if (input.StartsWith("listsheets"))
                {
                    ListAllSheets();
                }
                else if (input.StartsWith("help"))
                {
                    Console.WriteLine("Gültige Befehle:");
                    foreach (string cmd in cmdlist) Console.WriteLine(cmd);
                    Console.WriteLine("Die Argumente müssen durch ; getrennt sein");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Befehl nicht gefunden.");
                    Console.WriteLine("Gültige Befehle:");
                    foreach (string cmd in cmdlist) Console.WriteLine(cmd);
                    Console.WriteLine("Die Argumente müssen durch ; getrennt sein");

                }

                
            }
        }

        static void ListAllPieces()
        {
            var notensatzset = manager.GetAllPieces();

            Console.WriteLine();
            Console.WriteLine("{0,-65}", "Tabelle Notensätze");
            Console.WriteLine();
                Console.WriteLine(
                    String.Format("{0,-5}", "ID") +
                    String.Format("{0,-30}", "Name") +
                    String.Format("{0,-30}", "Arrangeur"));
                Console.WriteLine("{0,-65}", "-----------------------------------------------------------------------------------------------");

                foreach (Piece ns in notensatzset)
                {
                    Console.WriteLine(
                    String.Format("{0,-5}", ns.PieceID) +
                    String.Format("{0,-30}", ns.Name) +
                    String.Format("{0,-30}", ns.Arranger));
                }

                Console.WriteLine("{0,-65}", "-----------------------------------------------------------------------------------------------");
                Console.WriteLine("Anzahl Notensätze: " + notensatzset.Count);

            
        }

        static void ListAllSheets()
        {
            var notenblattset = manager.GetAllSheets();

            Console.WriteLine();
            Console.WriteLine("{0,-65}", "Tabelle Notenblätter");
            Console.WriteLine();
            Console.WriteLine(
                String.Format("{0,-5}", "ID") +
                String.Format("{0,-30}", "Notensatz") +
                String.Format("{0,-30}", "Stimme"));
            Console.WriteLine("{0,-65}", "-----------------------------------------------------------------------------------------------");

            foreach (Sheet nb in notenblattset)
            {
                Console.WriteLine(
                String.Format("{0,-5}", nb.SheetID) +
                String.Format("{0,-30}", nb.Piece.Name) +
                String.Format("{0,-30}", nb.Part.Name));
            }

            Console.WriteLine("{0,-65}", "-----------------------------------------------------------------------------------------------");
            Console.WriteLine("Anzahl Notenblätter: " + notenblattset.Count);
        }

        static void ListAllParts()
        {
            var stimmenset = manager.GetAllParts();

            Console.WriteLine();
            Console.WriteLine("{0,-65}", "Tabelle Stimmen");
            Console.WriteLine();
            Console.WriteLine(
                String.Format("{0,-5}", "ID") +
                String.Format("{0,-30}", "Name"));
            Console.WriteLine("{0,-65}", "-----------------------------------------------------------------------------------------------");

            foreach (Part pc in stimmenset)
            {
                Console.WriteLine(
                String.Format("{0,-5}", pc.PartID) +
                String.Format("{0,-30}", pc.Name));
            }

            Console.WriteLine("{0,-65}", "-----------------------------------------------------------------------------------------------");
            Console.WriteLine("Anzahl Stimmen: " + stimmenset.Count);


        }
    }
}
