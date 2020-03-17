using System;
using System.Xml.Serialization;

namespace Zebra.Library
{
    [XmlInclude(typeof(FTPCredentials))]
    [XmlInclude(typeof(SFTPCredentials))]
    [XmlInclude(typeof(LocalArchiveCredentials))]
    public abstract class ArchiveCredentials
    {
        // Private Constructor for XML Serialization
        public ArchiveCredentials() { }
    }

    public class FTPCredentials : ArchiveCredentials
    {
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string Path { get; set; }

        public FTPCredentials(string server, string username, string password, string path, string port = "21")
        {
            Server = server;
            Username = username;
            Password = password;
            Path = path;
            Port = port;
        }

        // Private Constructor for XML Serialization
        private FTPCredentials() { }
    }


    public class SFTPCredentials : ArchiveCredentials
    {
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string Path { get; set; }

        public SFTPCredentials(string server, string username, string password, string path, string port = "22")
        {
            Server = server;
            Username = username;
            Password = password;
            Path = path;
            Port = port;
        }

        // Private Constructor for XML Serialization
        private SFTPCredentials() { }
    }

    public class LocalArchiveCredentials : ArchiveCredentials
    {
        public string Path { get; set; }

        public LocalArchiveCredentials(string path)
        {
            this.Path = path;
        }

        // Private Constructor for XML Serialization
        private LocalArchiveCredentials() { }
    }
}
