using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using Zebra.Library;

namespace Zebra.Library.Services
{
    public class ZebraConfigurationService
    {
        private FileInfo ConfigurationFilePath { get; set; }

        private ZebraConfig config = null;

        public bool IsConfigured = false;

        public ZebraConfigurationService()
        {
        }

        public static void CreateConfigurationFile(ZebraConfig config)
        {
            try
            {
                if (!Directory.Exists(@"configs"))
                {
                    Directory.CreateDirectory(@"configs");
                }
                config.SerializeAsJSON(@"configs");
                
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }
        

        public void LoadConfigurationFromZebraConfig(ZebraConfig _config)
        {
            config = _config;

            if (!Directory.Exists(config.RepositoryDirectory)) Directory.CreateDirectory(config.RepositoryDirectory);
            if (!Directory.Exists(config.ArchiveDirectory)) Directory.CreateDirectory(config.ArchiveDirectory);
            if (!Directory.Exists(config.TempDirectory)) Directory.CreateDirectory(config.TempDirectory);


            Debug.Print($"Configuration path set to " + Path.GetFullPath(config.RepositoryDirectory));
            IsConfigured = true;
            ConfigurationLoaded?.Invoke(this, EventArgs.Empty);
        }

        public void LoadConfigurationFromFile(FileInfo configurationFilePath)
        {
            //config = ZebraConfig.FromXML(configurationFilePath.FullName);

            // Check if JSON or XML

            if(File.ReadAllText(configurationFilePath.FullName).Trim().StartsWith('<'))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ZebraConfig));
                using (FileStream reader = new FileStream(configurationFilePath.FullName, FileMode.Open))
                {

                    config = (ZebraConfig)serializer.Deserialize(reader);

                }
            }

            if (File.ReadAllText(configurationFilePath.FullName).Trim().StartsWith('{'))
            {
                config = JsonSerializer.Deserialize<ZebraConfig>(File.ReadAllText(configurationFilePath.FullName));
            }          

            this.ConfigurationFilePath= configurationFilePath;

            ConfigurationLoaded?.Invoke(this, EventArgs.Empty);
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

        public void UnloadConfig()
        {
            config = null;
            IsConfigured = false;
            ConfigurationUnloaded?.Invoke(this, EventArgs.Empty);
        }

        public FileInfo GetConfigurationFilePath()
        {
            return ConfigurationFilePath ?? null;
            
        }

        public RepositoryType GetRepositoryType()
        {
            return config.RepositoryType;
        }

        public ZebraConfig GetZebraConfig()
        { return config; }

        public event EventHandler ConfigurationLoaded;
        public event EventHandler ConfigurationUnloaded;


    }
}