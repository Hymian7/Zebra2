using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zebra.Library;

namespace Zebra.DatabaseAccess
{
    public class ZebraDBManager : IDisposable
    {
        private ZebraConfig ZebraConfig = ZebraConfig.FromXML("G:\\GitHub\\EFDesignerTest\\CoreLibrary\\EFDesignerTest.zebraconfig");
        private ZebraContext ctx;

        public ZebraDBManager()
        {
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

        public void Dispose() => ctx.Dispose();
    }
}
