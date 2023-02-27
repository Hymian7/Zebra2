using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Xml.Serialization;

namespace Zebra.Library
{

    public class ZebraConfig
    {
        public string ConfigName { get; set; }

        public RepositoryType RepositoryType { get; set; }

        public DatabaseProvider DatabaseProvider { get; set; }
        public ArchiveType ArchiveType { get; set; }

        public DatabaseCredentials DatabaseCredentials { get; set; }

        public ArchiveCredentials ArchiveCredentials { get; set; }

        public String RepositoryDirectory { get; set; }

        [XmlIgnore]
        public String TempDirectory => Path.Combine(RepositoryDirectory, "temp");

        [XmlIgnore]
        public String DatabasePath => Path.Combine(RepositoryDirectory, "database.db");

        [XmlIgnore]
        public String ArchiveDirectory => Path.Combine(RepositoryDirectory, "archive");

        public string ServerIPAddress { get; set; }

        public string ServerPort { get; set; }

        /// <summary>
        /// Parameterless Constructor for Serialization
        /// </summary>
        public ZebraConfig() { }

        private void SetDefaults()
        {
            //ConfigName = "New Config";
            //RepositoryType = RepositoryType.Local;
            //DatabaseProvider = DatabaseProvider.MySQL;
            //ArchiveType = ArchiveType.Local;
            //DatabaseCredentials = null;
            //ArchiveCredentials = null;
            //TempDir = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
            //BarcodeSeperatorChar = '#';
            //DummyIndicator = "dummy";
        }
        
        /// <summary>
        /// Constructor for local SQLite repository
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_repositoryDirectory"></param>
        public ZebraConfig(string _name, DirectoryInfo _repositoryDirectory)
        {
            ConfigName = _name;
            RepositoryType = RepositoryType.Local;
            DatabaseProvider = DatabaseProvider.SQLite;
            RepositoryDirectory = _repositoryDirectory.FullName;
            ArchiveType = ArchiveType.Local;
            ArchiveCredentials = new LocalArchiveCredentials(Path.Combine(RepositoryDirectory, "archive"));
            DatabaseCredentials = new SQLiteCredentials(Path.Combine(RepositoryDirectory, "database.db"));
            ArchiveCredentials = new LocalArchiveCredentials(Path.Combine(RepositoryDirectory, "archive"));

            ServerIPAddress = "localhost";
            ServerPort = "44347";
        }

        /// <summary>
        /// Constructor for remote repository
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_localRepositoryDirectory"></param>
        /// <param name="_IPAddress"></param>
        /// <param name="_port"></param>
        public ZebraConfig(string _name, DirectoryInfo _localRepositoryDirectory, string _IPAddress, string _port)
        {
            ConfigName = _name;
            RepositoryDirectory = _localRepositoryDirectory.FullName;
            RepositoryType = RepositoryType.Remote;
            DatabaseProvider = DatabaseProvider.SQLite;
            ArchiveType = ArchiveType.Local;
            ArchiveCredentials = new LocalArchiveCredentials(Path.Combine(RepositoryDirectory, "archive"));
            ServerIPAddress = _IPAddress;
            ServerPort = _port;
        }

        /// <summary>
        /// Speichert die ZebraConfig-Instanz als .xml im angegebenen Pfad ab.
        /// </summary>
        /// <param name="_path">Pfad zum Speichern der Config. Der Name der Config und die Dateiendung werden automatisch angefügt.</param>
        /// <returns>Gibt bei Erfolg true und im Falle eine Exception false zurück.</returns>
        public bool Serialize(string _path)
        {
            try
            {
                string _fullpath = Path.Combine(_path, $"{this.ConfigName}.zebraconfig");
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                StreamWriter writer = new StreamWriter(_fullpath);

                serializer.Serialize(writer, this);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.InnerException);
                Debug.WriteLine(ex.Data);
                return false;
                throw;
            }



            return true;
        }

        /// <summary>
        /// Speichert die ZebraConfig-Instanz als .json im angegebenen Pfad ab.
        /// </summary>
        /// <param name="_path">Pfad zum Speichern der Config. Der Name der Config und die Dateiendung werden automatisch angefügt.</param>
        /// <returns>Gibt bei Erfolg true und im Falle eine Exception false zurück.</returns>
        public bool SerializeAsJSON(string _path)
        {
            try
            {
                string _fullpath = Path.Combine(_path, $"{this.ConfigName}.zebraconfig");
                var jsonString = JsonSerializer.Serialize<ZebraConfig>(this);
                File.WriteAllText(_fullpath, jsonString);
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.InnerException);
                Debug.WriteLine(ex.Data);
                return false;
                throw;
            }



            return true;
        }

        /// <summary>
        /// Deserialisiert die angegebene XML-Datei und liefert ein ZebraConfig-Objekt zurück
        /// </summary>
        /// <param name="xmlfile">XML-Datei, die deserialisiert werden soll</param>
        /// <returns>Gibt ein ZebraConfig-Objekt zurück.</returns>
        public static ZebraConfig FromXML(string xmlfile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ZebraConfig));

            using (FileStream reader = new FileStream(xmlfile, FileMode.Open))
            {

                return (ZebraConfig)serializer.Deserialize(reader);

            }
        }

    }



    /// <summary>
    /// Type of Database Provider
    /// </summary>
    public enum DatabaseProvider
    {
        MySQL,
        Acces,
        SQLite
    }

    /// <summary>
    /// Type of Archive
    /// </summary>
    public enum ArchiveType
    {
        FTP,
        SFTP,
        Local
    }

    /// <summary>
    /// Defines, if Repository is local or remote (via API)
    /// </summary>
    public enum RepositoryType
    {
        Local,
        Remote
    }
}
