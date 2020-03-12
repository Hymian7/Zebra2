using System;
using System.IO;
using System.Xml.Serialization;

namespace Zebra.Library
{
    
    public class ZebraConfig
    {
        public string ConfigName { get; set; }        

        public DatabaseProvider DatabaseProvider { get; set; }
        public ArchiveType ArchiveType { get; set; }

        public DatabaseCredentials DatabaseCredentials { get; set; }

        public ArchiveCredentials ArchiveCredentials { get; set; }

        public char BarcodeSeperatorChar { get; set; }
        public string DummyIndicator { get; set; }

        /// <summary>
        /// Parameterless Constructor for Serialization
        /// </summary>
        public ZebraConfig() { }

        private void SetDefaults()
        {
            ConfigName = "New Config";
            DatabaseProvider = DatabaseProvider.MySQL;
            ArchiveType = ArchiveType.Local;
            DatabaseCredentials = null;
            ArchiveCredentials = null;
            BarcodeSeperatorChar = '#';
            DummyIndicator = "dummy";
        }
        
        public ZebraConfig(string _name)
        {
            SetDefaults();
            ConfigName = _name;
        }
        public ZebraConfig(string _name, DatabaseProvider _databaseprovider, DatabaseCredentials _databaseCredentials, ArchiveType archiveType, ArchiveCredentials archiveCredentials)
        {
            SetDefaults();
            ConfigName = _name;
            DatabaseProvider = _databaseprovider;
            DatabaseCredentials = _databaseCredentials;
            ArchiveType = archiveType;
            ArchiveCredentials = ArchiveCredentials;
                       
        }

        /// <summary>
        /// Speichert die ZebraConfig-Instanz als .xml im angegebenen Pfad ab
        /// </summary>
        /// <returns>Gibt bei Erfolg true und im Falle eine Exception false zurück.</returns>
        public bool Serialize(string _path)
        {
            try
            {
                string _fullpath = _path + @"\" + $"{this.ConfigName}.zebraconfig";
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                StreamWriter writer = new StreamWriter(_fullpath);

                serializer.Serialize(writer, this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.Data);
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
}
