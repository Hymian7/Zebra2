using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows;
using Zebra.Library;
using ZebraDesktop.Enums;
using ZebraDesktop.Views;

namespace ZebraDesktop.ViewModels
{
    public class NewConfigViewModel : ViewModelBase
    {
        #region Properties

        private String _configName;

        public String ConfigName
        {
            get { return _configName; }
            set { _configName = value; NotifyPropertyChanged(); }
        }

        private EnumSelection<DatabaseProvider> _dbProvider;

        public EnumSelection<DatabaseProvider> DBProvider
        {
            get { return _dbProvider; }
            set { _dbProvider = value; NotifyPropertyChanged(); }
        }

        private EnumSelection<ArchiveType> enumSelection;

        public EnumSelection<ArchiveType> ArchiveType
        {
            get { return enumSelection; }
            set { enumSelection = value; NotifyPropertyChanged(); }
        }

        private ZebraConfig zebraConfig;

        public ZebraConfig ConfigFile
        {
            get { return zebraConfig; }
            set { zebraConfig = value; NotifyPropertyChanged(); }
        }

        private SQLiteCredentials _sqLiteCredentials;

        public SQLiteCredentials SQLiteCredentials
        {
            get { return _sqLiteCredentials; }
            set { _sqLiteCredentials = value; NotifyPropertyChanged(); }
        }

        private MySQLCredentials _mySQLCredentials;

        public MySQLCredentials MySQLCredentials
        {
            get { return _mySQLCredentials; }
            set { _mySQLCredentials = value; NotifyPropertyChanged(); }
        }

        private LocalArchiveCredentials _localArchiveCredentials;

        public LocalArchiveCredentials LocalArchiveCredentials
        {
            get { return _localArchiveCredentials; }
            set { _localArchiveCredentials = value; NotifyPropertyChanged(); }
        }

        private FTPCredentials _ftpCredentials;

        public FTPCredentials FTPCredentials
        {
            get { return _ftpCredentials; }
            set { _ftpCredentials = value; NotifyPropertyChanged(); }
        }

        private String _tempDirPath;

        public String TempDirPath
        {
            get { return _tempDirPath; }
            set { _tempDirPath = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _browseLocalDBCommand;

        public DelegateCommand BrowseLocalDBCommand
        {
            get { return _browseLocalDBCommand; }
            set { _browseLocalDBCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _browseLocalArchiveCommand;

        public DelegateCommand BrowseLocalArchiveCommand
        {
            get { return _browseLocalArchiveCommand; }
            set { _browseLocalArchiveCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _browseTempDirCommand;

        public DelegateCommand BrowseTempDirCommand
        {
            get { return _browseTempDirCommand; }
            set { _browseTempDirCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _testMySQLServerCredentialsCommand;

        public DelegateCommand TestMySQLServerCredentialsCommand
        {
            get { return _testMySQLServerCredentialsCommand; }
            set { _testMySQLServerCredentialsCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _testFTPArchiveCredentialsCommand;

        public DelegateCommand TestFTPArchiveCredentialsCommand
        {
            get { return _testFTPArchiveCredentialsCommand; }
            set { _testFTPArchiveCredentialsCommand = value; NotifyPropertyChanged(); }
        }

        private DelegateCommand _saveNewConfigCommand;

        public DelegateCommand SaveNewConfigCommand
        {
            get { return _saveNewConfigCommand; }
            set { _saveNewConfigCommand = value; NotifyPropertyChanged(); }
        }


        #endregion

        public NewConfigViewModel()
        {
            DBProvider = new EnumSelection<DatabaseProvider>(DatabaseProvider.SQLite);
            ArchiveType = new EnumSelection<ArchiveType>(Zebra.Library.ArchiveType.Local);

            LocalArchiveCredentials = new LocalArchiveCredentials();
            FTPCredentials = new FTPCredentials();

            SQLiteCredentials = new SQLiteCredentials();
            MySQLCredentials = new MySQLCredentials();

            BrowseLocalDBCommand = new DelegateCommand(ExecuteBrowseLocalDBCommand, canExecuteBrowseLocalDBCommand);
            BrowseLocalArchiveCommand = new DelegateCommand(ExecuteBrowseLocalArchiveCommand, canExecuteBrowseLocalArchiveCommand);
            TestMySQLServerCredentialsCommand = new DelegateCommand(ExecuteTestMySQLServerCredentialsCommand, canExecuteTestMySQLServerCredentialsCommand);
            TestFTPArchiveCredentialsCommand = new DelegateCommand(ExecuteTestFTPArchiveCredentialsCommand, canExecuteFTPArchiveCredentialsCommand);
            SaveNewConfigCommand = new DelegateCommand(ExecuteSaveNewConfigCommand, canExecuteSaveNewConfigCommand);

            this.PropertyChanged += UpdateButtonStatus;
            
            //TODO: Implemet INotifyPropertyChanged for Credentials, so that Update can trigger the button state
            
        }

        #region Commands
        private bool canExecuteSaveNewConfigCommand(object obj)
        {
            if (ConfigName.IsNullOrEmpty() || TempDirPath.IsNullOrEmpty()) return false;

            switch (ArchiveType.Value)
            {
                case Zebra.Library.ArchiveType.FTP:
                    if (this.FTPCredentials.Server.IsNullOrEmpty() || FTPCredentials.Path.IsNullOrEmpty() || FTPCredentials.Username.IsNullOrEmpty() || FTPCredentials.Password.IsNullOrEmpty() || FTPCredentials.Port.IsNullOrEmpty()) return false;
                    break;
                case Zebra.Library.ArchiveType.SFTP:
                    return false;
                    break;
                case Zebra.Library.ArchiveType.Local:
                    if (LocalArchiveCredentials.Path.IsNullOrEmpty()) return false;
                    break;
                default:
                    break;
            }

            switch (DBProvider.Value)
            {
                case DatabaseProvider.MySQL:
                    if (MySQLCredentials.Server.IsNullOrEmpty() || MySQLCredentials.Port.IsNullOrEmpty() || MySQLCredentials.Username.IsNullOrEmpty() || MySQLCredentials.Password.IsNullOrEmpty() || MySQLCredentials.DatabaseName.IsNullOrEmpty()) return false;
                    break;
                case DatabaseProvider.Acces:
                    return false;
                    break;
                case DatabaseProvider.SQLite:
                    if (SQLiteCredentials.Path.IsNullOrEmpty()) return false;
                    break;
            }

            return true;

        }

        private void ExecuteSaveNewConfigCommand(object obj)
        {
            ZebraConfig conf = new ZebraConfig()
            {
                ConfigName = this.ConfigName,
                ArchiveType = this.ArchiveType.Value,
                DatabaseProvider = this.DBProvider.Value,
                TempDir = this.TempDirPath
            };

            switch (this.DBProvider.Value)
            {
                case DatabaseProvider.MySQL:
                    conf.DatabaseCredentials = MySQLCredentials;
                    break;
                case DatabaseProvider.Acces:
                    break;
                case DatabaseProvider.SQLite:
                    conf.DatabaseCredentials = SQLiteCredentials;
                    break;
            }

            switch (this.ArchiveType.Value)
            {
                case Zebra.Library.ArchiveType.FTP:
                    conf.ArchiveCredentials = FTPCredentials;
                    break;
                case Zebra.Library.ArchiveType.SFTP:
                    break;
                case Zebra.Library.ArchiveType.Local:
                    conf.ArchiveCredentials = LocalArchiveCredentials;
                    break;
            }

            try
            {
                conf.Serialize(@"F:\GitHub\Zebra2\ZebraDesktop\bin\Debug\netcoreapp3.1\configs");
                (obj as Window).Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private bool canExecuteFTPArchiveCredentialsCommand(object obj)
        {
            if (this.FTPCredentials.Server.IsNullOrEmpty() || FTPCredentials.Path.IsNullOrEmpty() || FTPCredentials.Username.IsNullOrEmpty() || FTPCredentials.Password.IsNullOrEmpty() || FTPCredentials.Port.IsNullOrEmpty()) return false;
            return true;
        }

        private void ExecuteTestFTPArchiveCredentialsCommand(object obj)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create($"ftp://{FTPCredentials.Server}/{FTPCredentials.Path}/");
                request.Credentials = new NetworkCredential(FTPCredentials.Username, FTPCredentials.Password);
                request.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                MessageBox.Show("Erfolgreich");
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool canExecuteTestMySQLServerCredentialsCommand(object obj)
        {
            if (MySQLCredentials.Server.IsNullOrEmpty() || MySQLCredentials.Port.IsNullOrEmpty() || MySQLCredentials.Username.IsNullOrEmpty() || MySQLCredentials.Password.IsNullOrEmpty() || MySQLCredentials.DatabaseName.IsNullOrEmpty()) return false;
            return true;
        }

        private void ExecuteTestMySQLServerCredentialsCommand(object obj)
        {
            using (Zebra.Library.ConnectionTesting.MySQLTestContext con = new Zebra.Library.ConnectionTesting.MySQLTestContext(this.MySQLCredentials))
            {
                try
                {
                    con.Database.CanConnect();
                    MessageBox.Show("Erfolgreich verbunden");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message ,"Fehler");
                }

            }
        }

        private bool canExecuteBrowseLocalArchiveCommand(object obj)
        {
            return true;
        }

        private void ExecuteBrowseLocalArchiveCommand(object obj)
        {
            MessageBox.Show("Einfach Pfad in die Textbox kopieren");
        }

        private bool canExecuteBrowseLocalDBCommand(object obj)
        {
            return true;
        }

        private void ExecuteBrowseLocalDBCommand(object obj)
        {
            MessageBox.Show("Einfach Pfad in die Textbox kopieren");
        } 
        #endregion
    }
}
