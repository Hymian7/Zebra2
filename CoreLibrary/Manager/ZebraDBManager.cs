using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Zebra.Library;
using Zebra.Library.Mapping;
using Zebra.Library.PdfHandling;

namespace Zebra.Library
{
    public class ZebraDBManager : IDisposable, IZebraDBManager
    {

        public ZebraConfig ZebraConfig { get; private set; }
        public ZebraContext Context { get; private set; }
        public ZebraArchive Archive { get; private set; }

        //public ZebraDBManager()
        //{
        //    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        //    {
        //        this.ZebraConfig = ZebraConfig.FromXML("Zebra2.zebraconfig");
        //    }
        //    else this.ZebraConfig = ZebraConfig.FromXML("..\\..\\..\\..\\CoreLibrary\\Zebra2.zebraconfig");



        //    switch (this.ZebraConfig.DatabaseProvider)
        //    {
        //        case DatabaseProvider.MySQL:
        //            this.Context = new MySQLZebraContext(this.ZebraConfig);
        //            break;
        //        case DatabaseProvider.Acces:
        //            throw new NotImplementedException();
        //            break;
        //        case DatabaseProvider.SQLite:
        //            this.Context = new SQLiteZebraContext(this.ZebraConfig);
        //            break;
        //        default:
        //            break;
        //    }

        //    this.Archive = new LocalArchive(ZebraConfig.ArchiveCredentials as LocalArchiveCredentials);
        //    Context.Database.EnsureCreated();
        //}

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

            // Make sure that all Directories exist
            if (!Directory.Exists(ZebraConfig.RepositoryDirectory)) Directory.CreateDirectory(ZebraConfig.RepositoryDirectory);
            if (!Directory.Exists(ZebraConfig.TempDirectory)) Directory.CreateDirectory(ZebraConfig.TempDirectory);
            if (!Directory.Exists((ZebraConfig.ArchiveCredentials as LocalArchiveCredentials).Path)) Directory.CreateDirectory((ZebraConfig.ArchiveCredentials as LocalArchiveCredentials).Path);
            if (ZebraConfig.RepositoryType == RepositoryType.Local && !File.Exists((ZebraConfig.DatabaseCredentials as SQLiteCredentials).Path)) File.Create((ZebraConfig.DatabaseCredentials as SQLiteCredentials).Path);
        }

        public async Task<bool> EnsureDatabaseCreatedAsync()
        {
            return await Context.Database.EnsureCreatedAsync();
        }

        //public List<Piece> GetAllPieces()
        //{
        //    return Context.Piece.ToList<Piece>();
        //}
        public async Task<List<PieceDTO>> GetAllPiecesAsync()
        {
            var allpieces = await Context.Piece.ToListAsync();
            var allpiecesDTO = new List<PieceDTO>();

            foreach (var piece in allpieces)
            {
                var pieceDTO = piece.ToDTO();
                pieceDTO.Sheet = new List<SheetDTO>();
                pieceDTO.Setlist = new List<SetlistDTO>();

                allpiecesDTO.Add(pieceDTO);
            }

            return allpiecesDTO;
        }

        public async Task<PieceDTO> GetPieceAsync(int id)
        {
            var piece = await Context.FindAsync<Piece>(id);

            var pieceDTO = piece.ToDTO();
            pieceDTO.Sheet = new List<SheetDTO>();
            pieceDTO.Setlist = new List<SetlistDTO>();

            return pieceDTO;
        }

        public List<Sheet> GetAllSheets()
        {
            return Context.Sheet.ToList<Sheet>();
        }
        public List<Part> GetAllParts()
        {
            return Context.Part.ToList<Part>();
        }

        public virtual ObservableCollection<Setlist> GetAllSetlists()
        {
            Context.Setlist.LoadAsync<Setlist>();
            return Context.Setlist.Local.ToObservableCollection();
        }

        public Piece NewPiece(string newname, string newarranger)
        {
            var _newpiece = Context.Add<Piece>(Piece.Create(newname, newarranger));
            Context.SaveChanges();
            return _newpiece.Entity;
        }
        public Piece NewPiece(string newname)
        {
            var _newpiece = Context.Add<Piece>(Piece.Create(newname));
            Context.SaveChanges();
            return _newpiece.Entity;
        }

        public Part NewPart(string partname)
        {
            var newpart = Context.Add<Part>(Part.Create(partname));
            Context.SaveChanges();
            return newpart.Entity;
        }

        public Sheet NewSheet(int pieceid, int partid)
        {
            var newsheet = Context.Add<Sheet>(Sheet.Create(Context.Part.Find(partid), Context.Piece.Find(pieceid)));
            Context.SaveChanges();
            return newsheet.Entity;
        }


        

        public async Task<List<PartDTO>> GetAllPartsAsync()
        {
            var allparts = await Context.Part.ToListAsync();
            var allpartsDTO = new List<PartDTO>();

            foreach (var part in allparts)
            {
                allpartsDTO.Add(part.ToDTO());
            }

            return allpartsDTO;
        }

        public async Task<PartDTO> GetPartAsync(int id)
        {
            var part = await Context.Part.FindAsync(id);
            return part.ToDTO();
        }

        public async Task<PartDTO> PostPartAsync(PartDTO newPart)
        {
            var part = new Part(newPart.Name) { Position = newPart.Position };

            Context.Part.Add(part);
            await Context.SaveChangesAsync();

            var changedentity = await Context.Part.FindAsync(part.PartID);
            return changedentity.ToDTO();
        }

        public async Task<List<SetlistDTO>> GetAllSetlistsAsync()
        {
            var SetlistList = await Context.Setlist.ToListAsync();
            List<SetlistDTO> SetlistDTOList = new List<SetlistDTO>();

            foreach (var sl in SetlistList)
            {
                var newSetlistDTO = sl.ToDTO();
                newSetlistDTO.SetlistItems = new List<SetlistItemDTO>();

                foreach (var item in sl.SetlistItem)
                {
                    newSetlistDTO.SetlistItems.Add(new SetlistItemDTO { SetlistItemID = item.SetlistItemID, PieceName = item.Piece.Name, Position = item.Position });
                }

                SetlistDTOList.Add(newSetlistDTO);

            }
            return SetlistDTOList;
        }

        public async Task<SetlistDTO> GetSetlistAsync(int id)
        {
            return (await Context.Setlist.FindAsync(id)).ToDTO();
        }

        public async Task<SheetDTO> GetSheetAsync(int id)
        {
            return (await Context.Sheet.FindAsync(id)).ToDTO();
        }

        public Task<string> GetPDFPathAsync(int id)
        {
            return Task.FromResult(Path.Combine((ZebraConfig.ArchiveCredentials as LocalArchiveCredentials).Path, FileNameResolver.GetFileName(id)));
        }

        public Task<ImportCandidate> GetImportCandidateAsync(string filepath)
        {
            throw new NotImplementedException();
        }

        public Task ImportImportCandidateAsync(ImportCandidate ic)
        {
            throw new NotImplementedException();
        }



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
