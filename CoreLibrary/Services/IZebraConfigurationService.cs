using System;
using System.Collections.Generic;
using System.IO;

namespace Zebra.Library.Services
{
    public interface IZebraConfigurationService
    {
        event EventHandler ConfigurationLoaded;
        event EventHandler ConfigurationUnloaded;

        string GetArchiveDirectory();
        FileInfo GetConfigurationFilePath();
        List<string> GetConfigurationFiles();
        string GetDatabasePath();
        string GetRepositoryDirectory();
        RepositoryType GetRepositoryType();
        string GetTempDirectory();
        ZebraConfig GetZebraConfig();
        void LoadConfigurationFromFile(FileInfo configurationFilePath);
        void LoadConfigurationFromZebraConfig(ZebraConfig _config);
        void UnloadConfig();
    }
}