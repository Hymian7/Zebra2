using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Zebra.Library
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
            optionsBuilder.UseLazyLoadingProxies(true).UseMySql(Settings.ConnectionString);            
        }

        partial void OnModelCreatedImpl(ModelBuilder modelBuilder)
        {
            //Set the Update and Creation Timestamp behaviours for all Entities that implement ITimestamp
            //TODO: Distinguish between MySQL and other Databases
            
            //Part
            modelBuilder.Entity<Part>()
                .Property<DateTime?>(p => p.CreationDate)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Part>()
                .Property<DateTime?>(p => p.LastUpdate)
                .HasDefaultValueSql("NOW() ON UPDATE NOW()")
                .ValueGeneratedOnAddOrUpdate();

            //Piece
            modelBuilder.Entity<Piece>()
                .Property<DateTime?>(p => p.CreationDate)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Piece>()
                .Property<DateTime?>(p => p.LastUpdate)
                .HasDefaultValueSql("NOW() ON UPDATE NOW()")
                .ValueGeneratedOnAddOrUpdate();

            //Setlist
            modelBuilder.Entity<Setlist>()
                .Property<DateTime?>(p => p.CreationDate)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Setlist>()
                .Property<DateTime?>(p => p.LastUpdate)
                .HasDefaultValueSql("NOW() ON UPDATE NOW()")
                .ValueGeneratedOnAddOrUpdate();

            //SetlistItem
            modelBuilder.Entity<SetlistItem>()
                .Property<DateTime?>(p => p.CreationDate)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<SetlistItem>()
                .Property<DateTime?>(p => p.LastUpdate)
                .HasDefaultValueSql("NOW() ON UPDATE NOW()")
                .ValueGeneratedOnAddOrUpdate();

            //Sheet
            modelBuilder.Entity<Sheet>()
                .Property<DateTime?>(p => p.CreationDate)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Sheet>()
                .Property<DateTime?>(p => p.LastUpdate)
                .HasDefaultValueSql("NOW() ON UPDATE NOW()")
                .ValueGeneratedOnAddOrUpdate();
        }

    }
}
