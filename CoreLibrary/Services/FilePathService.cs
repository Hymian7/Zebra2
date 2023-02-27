using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Zebra.Library.Services
{
    /// <summary>
    /// This class provides methods that return the path of the PDF file for a given ID or GUID
    /// </summary>
    public class FilePathService
    {

        private ZebraConfigurationService _configurationService;
        private FileNameService _fileNameService;

        public FilePathService(ZebraConfigurationService configurationService, FileNameService fileNameService)
        {
            _configurationService = configurationService;
            _fileNameService = fileNameService;
        }

        public string GetFilePath(FolderType type, int id)
        {
            switch (type)
            {
                case FolderType.Archive:
                    return Path.Combine(_configurationService.GetArchiveDirectory(), _fileNameService.GetFileName(id));
                   
                case FolderType.Temp:
                    return Path.Combine(_configurationService.GetTempDirectory(), _fileNameService.GetFileName(id));
                    
                default:
                    throw new Exception();
            }
        }

        public string GetFilePath(FolderType type, Guid guid)
        {
            switch (type)
            {
                case FolderType.Archive:
                    return Path.Combine(_configurationService.GetArchiveDirectory(), _fileNameService.GetFileName(guid));

                case FolderType.Temp:
                    return Path.Combine(_configurationService.GetTempDirectory(), _fileNameService.GetFileName(guid));

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
