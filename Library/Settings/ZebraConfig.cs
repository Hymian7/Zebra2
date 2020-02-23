using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Zebra.StandardLibrary
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

        public int Datenbankprovider { get; set; }
        public string Server { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }

        public string ArchiveFolder { get; set; }

        public int NotensatzIDLength { get; set; }
        public int NotenblattIDLength { get; set; }
        public int StimmeIDLength { get; set; }


        public ZebraConfig()
        {
        }
        public ZebraConfig(string _name, int _datenbankprovider, string _server, string _port, string _username, string _password, string _databasename, string _archivefolder = @"D:\Desktop\Archive", int _nsIDLen = 4, int _nbIDLen = 6, int _stimIDLen = 3)
        {

            ConfigName = _name;
            Datenbankprovider = _datenbankprovider;
            Server = _server;
            Port = _port;
            Username = _username;
            Password = _password;
            DatabaseName = _databasename;
            ArchiveFolder = _archivefolder;
            NotensatzIDLength = _nsIDLen;
            NotenblattIDLength = _nbIDLen;
            StimmeIDLength = _stimIDLen;


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




    public enum Provider
    {
        MySQL = 1,
        Acces = 2
    }
}
