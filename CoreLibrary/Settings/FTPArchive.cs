using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Zebra.Library
{
    public class FTPArchive : ZebraArchive
    {
        public string Server { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Port { get; private set; }

        public string Path { get; private set; }

        public override bool IsConnected => throw new NotImplementedException();
                
        public override FileInfo GetFile(Sheet sheet)
        {
            throw new NotImplementedException();
        }

        public override void PushFile(FileInfo file, Sheet sheet, FileImportMode mode = FileImportMode.Copy)
        {
            throw new NotImplementedException();
        }

        public FTPArchive(FTPCredentials credentials)
        {
            Server = credentials.Server;
            Username = credentials.Username;
            Password = credentials.Password;
            Path = credentials.Path;
            Port = credentials.Port;
        }
    }
}
