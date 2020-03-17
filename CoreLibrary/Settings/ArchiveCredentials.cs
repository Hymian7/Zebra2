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
        public string Server { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Port { get; private set; }
        public string Path { get; private set; }

        public FTPCredentials(string server, string username, string password, string path, string port = "21")
        {
            Server = server;
            Username = username;
            Password = password;
            Path = path;
            Port = port;
        }
    }


    public class SFTPCredentials : ArchiveCredentials
    {
        public string Server { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Port { get; private set; }
        public string Path { get; private set; }

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
