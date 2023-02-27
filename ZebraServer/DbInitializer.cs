using System.Collections.Generic;
using System.Linq;
using Zebra.Library;

namespace ZebraServer
{
    public static class DbInitializer
    {
        public static void Initialize(ZebraContext context)
        {
            // Inizialize here
            // https://docs.microsoft.com/de-de/aspnet/core/data/ef-mvc/intro?view=aspnetcore-6.0

            context.Database.EnsureCreated();

            // Return if there are already entries in the database
            if (context.Piece.Any()) return;

            var parts = new Part[]
            {
                new Part("Piccolo in C"),
                new Part("1. Flöte in C"),
                new Part("2. Flöte in C"),
                new Part("1. Oboe"),
                new Part("2. Oboe"),
                new Part("1. Fagott"),
                new Part("2. Fagott"),
                new Part("Klarinette in Es"),
                new Part("1. Klarinette in B"),
                new Part("2. Klarientte in B"),
                new Part("3. Klarinette in B"),
                new Part("1. Altsaxophon in Es"),
                new Part("2. Altsaxophon in Es"),
                new Part("1. Tenorsaxophon in B"),
                new Part("2. Tenorsaxophon in B"),
                new Part("Baritonsaxophon in Es"),
                new Part("1. Trompete in B"),
                new Part("2. Trompete in B"),
                new Part("3. Trompete in B"),
                new Part("1. Flügelhorn in B"),
                new Part("2. Flügelhorn in B"),
                new Part("1. Tenorhorn in B"),
                new Part("2. Tenorhorn in B"),
                new Part("Bariton in B"),
                new Part("1. Posaune in C"),
                new Part("2. Posaune in C"),
                new Part("3. Posaune in C"),
                new Part("Tuba in C"),
                new Part("Tuba in Es"),
                new Part("Schlagzeug"),
                new Part("Percussion")

            };

            context.Part.AddRange(parts);

            var pieces = new Piece[]
            {
                new Piece("In der Weinschenke"),
                new Piece("Südböhmische Polka"),
                new Piece("Wir Musikanten"),
                new Piece("Abel Tasman"),
                new Piece("Gruß an Böhmen"),
                new Piece("Ein halbes Jahrhundert"),
                new Piece("Sorgenbrecher"),
                new Piece("Morgenblüten"),
                new Piece("Auf der Vogelwiese"),
                new Piece("Böhmischer Traum"),
                new Piece("Böhmische Liebe"),
                new Piece("Die Kapelle hat gewonnen"),
                new Piece("Von Freund zu Freund"),
                new Piece("Castaldo-Marsch")

            };

            context.Piece.AddRange(pieces);

            var setlists = new Setlist[]
            {
                new Setlist("Schwarze Mappe"),
                new Setlist("Blaue Mappe"),
                new Setlist("Grüne Mappe")
            };

            context.Setlist.AddRange(setlists);

            context.SaveChanges();



        }
    }
}