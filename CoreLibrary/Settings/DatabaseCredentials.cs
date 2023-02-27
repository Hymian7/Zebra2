using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Zebra.Library
{
    [XmlInclude(typeof(MySQLCredentials))]
    [XmlInclude(typeof(AccessCredentials))]
    [XmlInclude(typeof(SQLiteCredentials))]
    public class DatabaseCredentials
    {
        public DatabaseCredentials() { }
    }

    public class AccessCredentials : DatabaseCredentials
    {
        public AccessCredentials()
        {

        }
        
    }

    public class MySQLCredentials : DatabaseCredentials
    {
        public string Server { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }

        public string ConnectionString
        {
            get
            {
                return $"server={this.Server};port={this.Port};user id={this.Username};password={this.Password};database={this.DatabaseName};persistsecurityinfo=True;";
            }
        }

        public MySQLCredentials(string _server, string _port, string _username, string _password, string _databasename)
        {
            Server = _server;
            Port = _port;
            Username = _username;
            Password = _password;
            DatabaseName = _databasename;
        }

        public MySQLCredentials()
        {

        }
    }

    public class SQLiteCredentials : DatabaseCredentials
    {
        public string Path { get; set; }

        public SQLiteCredentials() { }

        public SQLiteCredentials(string _path)
        {
            Path = _path;
        }
    }
}
