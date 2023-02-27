using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zebra.Library.Services;

namespace Zebra.Library
{
    public class SQLiteZebraContext : ZebraContext
    {
        /// <summary>
        /// Only for EF Core Migrations
        /// </summary>
        //public SQLiteZebraContext() : base() 
        //{

        //}

        public SQLiteZebraContext(ZebraConfigurationService configurationService) : base(configurationService)
        {

        }

        
        public SQLiteZebraContext(ZebraConfig config) : base(config)
        {

        }

        protected override void CustomInit(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(true).UseSqlite("Data Source="+(Settings.DatabaseCredentials as SQLiteCredentials).Path);
        }

        protected override void OnModelCreatedImpl(ModelBuilder modelBuilder)
        {
            //TODO Update Timestamp on Change

            //Part
            modelBuilder.Entity<Part>()
                .Property<DateTime?>(p => p.CreationDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Part>()
                .Property<DateTime?>(p => p.LastUpdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();

            //Piece
            modelBuilder.Entity<Piece>()
                .Property<DateTime?>(p => p.CreationDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Piece>()
                .Property<DateTime?>(p => p.LastUpdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();

            //Setlist
            modelBuilder.Entity<Setlist>()
                .Property<DateTime?>(p => p.CreationDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Setlist>()
                .Property<DateTime?>(p => p.LastUpdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();

            //SetlistItem
            modelBuilder.Entity<SetlistItem>()
                .Property<DateTime?>(p => p.CreationDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<SetlistItem>()
                .Property<DateTime?>(p => p.LastUpdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();

            //Sheet
            modelBuilder.Entity<Sheet>()
                .Property<DateTime?>(p => p.CreationDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Sheet>()
                .Property<DateTime?>(p => p.LastUpdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnAddOrUpdate();
        }

        protected override void OnModelCreatingImpl(ModelBuilder modelBuilder)
        {
            
        }
    }
}
