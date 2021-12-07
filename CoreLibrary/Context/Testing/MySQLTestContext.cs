using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library.ConnectionTesting
{
    public class MySQLTestContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public MySQLCredentials Credentials { get; set; }

        public MySQLTestContext(DbContextOptions<MySQLTestContext> options) : base(options)
        {
        }

        public MySQLTestContext()
        {

        }

        public MySQLTestContext(MySQLCredentials _credentials)
        {
            Credentials = _credentials;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(true).UseMySql(Credentials.ConnectionString, ServerVersion.AutoDetect(Credentials.ConnectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
