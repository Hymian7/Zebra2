using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using Zebra.Library;

namespace Zebra.Library.Services
{
    public class ZebraConfigurationService
    {
        private ZebraConfig config;

        public bool IsConfigured = false;

        public ZebraConfigurationService()
        {
        }

        public void LoadConfiguration(ZebraConfig _config)
        {
            config = _config;

            if (!Directory.Exists(config.RepositoryDirectory)) Directory.CreateDirectory(config.RepositoryDirectory);
            if (!Directory.Exists(config.ArchiveDirectory)) Directory.CreateDirectory(config.ArchiveDirectory);
            if (!Directory.Exists(config.TempDirectory)) Directory.CreateDirectory(config.TempDirectory);


            Debug.Print($"Configuration path set to " + config.RepositoryDirectory);
            IsConfigured = true;
        }

        public string GetRepositoryDirectory()
        {
            if (IsConfigured)
            {
                return config.RepositoryDirectory;
            }
            else
            {
                throw new System.Exception("Configuration Service is not yet configured!");
            }
        }

        public string GetArchiveDirectory()
        {
            if (IsConfigured)
            {
                return config.ArchiveDirectory;
            }
            else
            {
                throw new System.Exception("Configuration Service is not yet configured!");
            }
        }

        public string GetTempDirectory()
        {
            if (IsConfigured)
            {
                return config.TempDirectory;
            }
            else
            {
                throw new System.Exception("Configuration Service is not yet configured!");
            }
        }

        public string GetDatabasePath()
        {
            if (IsConfigured)
            {
                return config.DatabasePath;
            }
            else
            {
                throw new System.Exception("Configuration Service is not yet configured!");
            }
        }




    }
}