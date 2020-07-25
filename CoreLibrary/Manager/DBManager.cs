using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Zebra.Library;

namespace Zebra.Library
{
    public class ZebraDBManager : IDisposable
    {

        public ZebraConfig ZebraConfig { get; private set; }
        public ZebraContext Context { get; private set; }
        public ZebraArchive Archive { get; private set; }

        public ZebraDBManager()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                this.ZebraConfig = ZebraConfig.FromXML("Zebra2.zebraconfig");
            }
            else this.ZebraConfig = ZebraConfig.FromXML("..\\..\\..\\..\\CoreLibrary\\Zebra2.zebraconfig");



            switch (this.ZebraConfig.DatabaseProvider)
            {
                case DatabaseProvider.MySQL:
                    this.Context = new MySQLZebraContext(this.ZebraConfig);
                    break;
                case DatabaseProvider.Acces:
                    throw new NotImplementedException();
                    break;
                case DatabaseProvider.SQLite:
                    this.Context = new SQLiteZebraContext(this.ZebraConfig);
                    break;
                default:
                    break;
            }
            
            this.Archive = new LocalArchive(ZebraConfig.ArchiveCredentials as LocalArchiveCredentials);
            Context.Database.EnsureCreated();
        }
        
        public ZebraDBManager(ZebraConfig ZebraConf)
        {
            ZebraConfig = ZebraConf;
            

            // Setup Context
            switch (ZebraConf.DatabaseProvider)
            {
                case DatabaseProvider.MySQL:
                    this.Context = new MySQLZebraContext(ZebraConf);
                    break;
                case DatabaseProvider.Acces:
                    this.Context = new AccessZebraContext(ZebraConf);
                    break;
                case DatabaseProvider.SQLite:
                    this.Context = new SQLiteZebraContext(ZebraConf);
                    break;
                default:
                    throw new Exception("Invalid Database Type in Config File");
            }

            Context.Database.EnsureCreated();

            // Setup Archive
            switch (ZebraConf.ArchiveType)
            {
                case ArchiveType.FTP:
                    this.Archive = new FTPArchive(ZebraConf.ArchiveCredentials as FTPCredentials); 
                    break;
                case ArchiveType.SFTP:
                    this.Archive = new SFTPArchive(ZebraConf.ArchiveCredentials as SFTPCredentials);
                    break;
                case ArchiveType.Local:
                    this.Archive = new LocalArchive(ZebraConf.ArchiveCredentials as LocalArchiveCredentials);
                    break;
                default:
                    throw new Exception("Invalid Archive Type in Config File");
            }
        }

        public List<Piece> GetAllPieces()
        {
            return Context.Piece.ToList<Piece>();
        }
        public List<Sheet> GetAllSheets()
        {
            return Context.Sheet.ToList<Sheet>();
        }
        public List<Part> GetAllParts()
        {
            return Context.Part.ToList<Part>();
        }

        public void NewPiece(string newname, string newarranger)
        {
            Context.Add<Piece>(Piece.Create(newname, newarranger));
            Context.SaveChanges();
        }
        public void NewPiece(string newname)
        {
            Context.Add<Piece>(Piece.Create(newname));
            Context.SaveChanges();
        }

        public void NewPart(string partname)
        {
            Context.Add<Part>(Part.Create(partname));
            Context.SaveChanges();
        }

        public void NewSheet(int pieceid, int partid)
        {
            Context.Add<Sheet>(Sheet.Create(Context.Part.Find(partid), Context.Piece.Find(pieceid)));
            Context.SaveChanges();
        }

        public void StorePDF(FileInfo file, Sheet sheet) => Archive.PushFile(file, sheet);

        public FileInfo GetPDF(Sheet sheet) => Archive.GetFile(sheet);

        #region LINQ Queries

        /// <summary>
        /// Find all Pieces that contain the searchstring in their Name
        /// </summary>
        /// <param name="searchstring">Name or substring for Name to look for</param>
        /// <returns></returns>
        public List<Piece> FindPiecesByName(string searchstring)
        {
            return (from piece in Context.Piece
                    where piece.Name.ToLower().Contains(searchstring.ToLower())
                    orderby piece.PieceID
                    select piece).ToList<Piece>();
        }

        public Piece GetPieceByID(int id) => Context.Find<Piece>(id);

        public Part GetPartByID(int id) => Context.Find<Part>(id);

        public Sheet GetSheet(Piece piece, Part part)
        {
            return (from sheet in Context.Sheet
                    where sheet.Piece.PieceID == piece.PieceID && sheet.Part.PartID == part.PartID
                    select sheet).SingleOrDefault();
        }

        public Sheet GetSheet(int id) => Context.Sheet.Find(id);
        

        #endregion

        #region Finalization
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) => Context.Dispose();

        ~ZebraDBManager() => Dispose(false); 
        #endregion
    }
}
