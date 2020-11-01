using Renci.SshNet;
using System;
using System.IO;

namespace Zebra.Library
{
    public class SFTPArchive : ZebraArchive
    {
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }

        private string _Path;
        public string Path
        {
            get { return _Path; }
            set
            {                    
                _Path = value.TrimEnd('/');
            }
        }

        public override bool IsConnected
        {
            get
            {
                using (var client = new SftpClient(new PasswordConnectionInfo(Server, Int32.Parse(Port), Username, Password)))
                {
                    client.Connect();
                    return client.IsConnected;                    
                }
            }
        }

        public override FileInfo GetFile(Sheet sheet)
        {
            if (File.Exists($"\\temp\\{sheet.Part.PartID}\\{sheet.SheetID}.pdf")) return new FileInfo($"\\temp\\{sheet.Part.PartID}\\{sheet.SheetID}.pdf");

            if (!Directory.Exists($"temp\\{sheet.Part.PartID}")) Directory.CreateDirectory($"temp\\{sheet.Part.PartID}");
            var fs = new FileStream($"temp\\{sheet.Part.PartID}\\{sheet.SheetID}.pdf", FileMode.Create);
            
            using (var client = new SftpClient(new PasswordConnectionInfo(Server, Int32.Parse(Port), Username, Password)))
            {
                client.Connect();
                client.DownloadFile(Path + "/" + sheet.Part.PartID + "/" + sheet.SheetID + ".pdf", fs);
                fs.Dispose();
                return new FileInfo($"temp\\{sheet.Part.PartID}\\{sheet.SheetID}.pdf");
            }
        }

        public override void PushFile(FileInfo file, Sheet sheet, FileImportMode mode = FileImportMode.Copy, bool _override = false)
        {
            switch (mode)
            {
                case FileImportMode.Copy:
                    push(file, sheet);
                    break;
                case FileImportMode.Move:
                    push(file, sheet);
                    file.Delete();
                    break;
            }

            void push(FileInfo file, Sheet sheet)
            {
                var fs = new System.IO.FileStream(file.FullName, FileMode.Open);

                using (var client = new SftpClient(new PasswordConnectionInfo(Server, Int32.Parse(Port), Username, Password)))
                {
                    client.Connect();                    
                    if (!client.Exists(Path + "/" + sheet.Part.PartID)) client.CreateDirectory(Path + "/" + sheet.Part.PartID);
                    client.UploadFile(fs, Path + "/" + sheet.Part.PartID + "/" + sheet.SheetID + ".pdf");
                }
            }
        }

        public SFTPArchive(SFTPCredentials credentials)
        {
            Server = credentials.Server;
            Username = credentials.Username;
            Password = credentials.Password;
            Path = credentials.Path;
            Port = credentials.Port;
        }
    }
}
