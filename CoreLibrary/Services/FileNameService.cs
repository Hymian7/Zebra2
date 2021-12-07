using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library.Services
{
    public class FileNameService
    {
        private const int FileNameLength = 8;
        private const string Extension = ".pdf";

        public string GetFilePath(FolderType type, string filename)
        {
            return $"D:\\Desktop\\ZebraTemp\\{GetSubfolder(type)}\\{filename.PadLeft(8, '0')}{Extension}"; 
        }

        public string GetFilePath(FolderType type, Guid guid)
        {
            return $"D:\\Desktop\\ZebraTemp\\{GetSubfolder(type)}\\{guid}{Extension}";
        }

        public string GetFolderPath(FolderType type)
        {
            return $"D:\\Desktop\\ZebraTemp\\{GetSubfolder(type)}";
        }

        private string GetSubfolder(FolderType type)
        {
            return type switch
            {
                FolderType.Archive => "ServerArchive",
                FolderType.Temp => "ServerTemp",
                _ => throw new ArgumentException($"Invalid FolderType {type}")

            };
        }

    }

    public enum FolderType
    {
        Archive,
        Temp
    }

}
