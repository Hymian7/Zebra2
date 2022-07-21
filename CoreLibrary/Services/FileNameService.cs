using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Zebra.Library.Services
{
    public class FileNameService
    {
        private const int FileNameLength = 8;
        private const string Extension = ".pdf";

        private ZebraConfigurationService _configurationService;

        public FileNameService(ZebraConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public string GetFilePath(FolderType type, int id)
        {
            switch (type)
            {
                case FolderType.Archive:
                    return Path.Combine(_configurationService.GetArchiveDirectory(), FileNameResolver.GetFileName(id));
                   
                case FolderType.Temp:
                    return Path.Combine(_configurationService.GetTempDirectory(), FileNameResolver.GetFileName(id));
                    
                default:
                    throw new Exception();
            }
        }

        public string GetFilePath(FolderType type, Guid guid)
        {
            switch (type)
            {
                case FolderType.Archive:
                    return Path.Combine(_configurationService.GetArchiveDirectory(), $"{guid}{Extension}");

                case FolderType.Temp:
                    return Path.Combine(_configurationService.GetTempDirectory(), $"{guid}{Extension}");

                default:
                    throw new Exception();
            }
        }

        public string GetFolderPath(FolderType type)
        {
            switch (type)
            {
                case FolderType.Archive:
                    return _configurationService.GetArchiveDirectory();
                case FolderType.Temp:
                    return _configurationService.GetTempDirectory();
                default:
                    throw new Exception();
            }
        }

    }

    public enum FolderType
    {
        Archive,
        Temp
    }

}
