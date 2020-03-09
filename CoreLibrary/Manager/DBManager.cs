using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Zebra.Library;

namespace Zebra.DatabaseAccess
{
    public class ZebraDBManager : IDisposable
    {

        private ZebraConfig ZebraConfig;
        private ZebraContext ctx;

        public ZebraDBManager()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                this.ZebraConfig = ZebraConfig.FromXML("Zebra2.zebraconfig");
            }
            else this.ZebraConfig = ZebraConfig.FromXML("G:\\GitHub\\Zebra2\\CoreLibrary\\Zebra2.zebraconfig");

            this.ctx = new ZebraContext(this.ZebraConfig);
            ctx.Database.EnsureCreated();
        }
        
        public ZebraDBManager(ZebraConfig altZebraConf)
        {
            ZebraConfig = altZebraConf;
            this.ctx = new ZebraContext(this.ZebraConfig);
            ctx.Database.EnsureCreated();
        }

        public List<Piece> GetAllPieces()
        {
            return ctx.Piece.ToList<Piece>();
        }
        public List<Sheet> GetAllSheets()
        {
            return ctx.Sheet.ToList<Sheet>();
        }
        public List<Part> GetAllParts()
        {
            return ctx.Part.ToList<Part>();
        }

        public void NewPiece(string newname, string newarranger)
        {
            ctx.Add<Piece>(Piece.Create(newname, newarranger));
            ctx.SaveChanges();
        }
        public void NewPiece(string newname)
        {
            ctx.Add<Piece>(Piece.Create(newname));
            ctx.SaveChanges();
        }

        public void NewPart(string partname)
        {
            ctx.Add<Part>(Part.Create(partname));
            ctx.SaveChanges();
        }

        public void NewSheet(int pieceid, int partid)
        {
            ctx.Add<Sheet>(Sheet.Create(ctx.Part.Find(partid), ctx.Piece.Find(pieceid)));
            ctx.SaveChanges();
        }

        #region LINQ Queries

        /// <summary>
        /// Find all Pieces that contain the searchstring in their Name
        /// </summary>
        /// <param name="searchstring">Name or substring for Name to look for</param>
        /// <returns></returns>
        public List<Piece> FindPiecesByName(string searchstring)
        {
            return (from piece in ctx.Piece
                    where piece.Name.ToLower().Contains(searchstring.ToLower())
                    orderby piece.PieceID
                    select piece).ToList<Piece>();
        }

        public Piece GetPieceByID(int id) => ctx.Find<Piece>(id);

        public Part GetPartByID(int id) => ctx.Find<Part>(id);

        public Sheet GetSheet(Piece piece, Part part)
        {
            return (from sheet in ctx.Sheet
                    where sheet.Piece.PieceID == piece.PieceID && sheet.Part.PartID == part.PartID
                    select sheet).SingleOrDefault();
        }

        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) => ctx.Dispose();

        ~ZebraDBManager() => Dispose(false);
    }
}
