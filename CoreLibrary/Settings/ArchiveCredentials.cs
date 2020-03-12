using System;
using System.Xml.Serialization;

namespace Zebra.Library
{
    [XmlInclude(typeof(FTPCredentials))]
    [XmlInclude(typeof(SFTPCredentials))]
    [XmlInclude(typeof(LocalArchiveCredentials))]
    public abstract class ArchiveCredentials
    {
        public ArchiveCredentials()
        {

        }
    }

    public class FTPCredentials : ArchiveCredentials
    {
        public FTPCredentials()
        {
            throw new NotImplementedException();
        }
    }


    public class SFTPCredentials : ArchiveCredentials
    {
        public SFTPCredentials()
        {
            throw new NotImplementedException();
        }
    }

    public class LocalArchiveCredentials : ArchiveCredentials
    {
        public string Path { get; set; }

        public LocalArchiveCredentials(string path)
        {
            this.Path = path;
        }

        private LocalArchiveCredentials()
        {

        }
    }
}
