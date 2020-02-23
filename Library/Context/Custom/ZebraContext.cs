using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.StandardLibrary
{
    public partial class ZebraContext
    {
        /// <summary>
        /// Zebra Settings for the Database Connection
        /// </summary>
        public ZebraConfig Settings { get; set; }

        public ZebraContext(ZebraConfig zebraConfig) {

            this.Settings = zebraConfig;

        }

        partial void OnModelCreatingImpl(ModelBuilder modelBuilder)
        {
            //throw new NotImplementedException();
            
            //muss in der generierten Datei geändert werden!!!
            modelBuilder.HasDefaultSchema(null);
        }

        partial void CustomInit(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(Settings.ConnectionString);            

        }

    }
}
