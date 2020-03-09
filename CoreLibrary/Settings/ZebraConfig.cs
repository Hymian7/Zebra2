using System;
using System.IO;
using System.Xml.Serialization;

namespace Zebra.Library
{
    public class ZebraConfig
    {
        public string ConfigName { get; set; }


        public string ConnectionString
        {
            get
            {

                return $"server={this.Server};port={this.Port};user id={this.Username};password={this.Password};database={this.DatabaseName};persistsecurityinfo=True;";
            }
        }

        public Provider DatabaseProvider { get; set; }
        public string Server { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }

        public string ArchiveFolder { get; set; }

        public char BarcodeSeperatorChar { get; set; }
        public string DummyIndicator { get; set; }


        public ZebraConfig()
        {
            this.DatabaseProvider = Provider.MySQL;
            this.ArchiveFolder = @"D:\Desktop\Archive";
            this.BarcodeSeperatorChar = '#';
            this.DummyIndicator = "dummy";
        }
        public ZebraConfig(string _name, Provider _databaseprovider, string _server, string _port, string _username, string _password, string _databasename, string _archivefolder = @"D:\Desktop\Archive", char _seperator = '#', string _dummyind = "dummy")
        {

            ConfigName = _name;
            DatabaseProvider = _databaseprovider;
            Server = _server;
            Port = _port;
            Username = _username;
            Password = _password;
            DatabaseName = _databasename;
            ArchiveFolder = _archivefolder;
            BarcodeSeperatorChar = _seperator;
            DummyIndicator = _dummyind;


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


        public override string ToString()
        {

            return $"Konfiguration: {this.ConfigName} mit Connectionstring: {this.ConnectionString}";

        }

    }



    /// <summary>
    /// Type of Database Provider
    /// </summary>
    public enum Provider
    {
        MySQL = 1,
        Acces = 2
    }
}
