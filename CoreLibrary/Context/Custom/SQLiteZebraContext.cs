using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library
{
    class SQLiteZebraContext : ZebraContext
    {
        public SQLiteZebraContext(ZebraConfig config) : base(config)
        {

        }

        protected override void CustomInit(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(true).UseSqlite((Settings.DatabaseCredentials as SQLiteCredentials).Path);
        }

        protected override void OnModelCreatedImpl(ModelBuilder modelBuilder)
        {
            //throw new NotImplementedException("SQLite Default Values for Timestamps are not implemented yet.");

            ////Part
            //modelBuilder.Entity<Part>()
            //    .Property<DateTime?>(p => p.CreationDate)
            //    .HasDefaultValueSql("NOW()")
            //    .ValueGeneratedOnAdd();
            //modelBuilder.Entity<Part>()
            //    .Property<DateTime?>(p => p.LastUpdate)
            //    .HasDefaultValueSql("NOW() ON UPDATE NOW()")
            //    .ValueGeneratedOnAddOrUpdate();

            ////Piece
            //modelBuilder.Entity<Piece>()
            //    .Property<DateTime?>(p => p.CreationDate)
            //    .HasDefaultValueSql("NOW()")
            //    .ValueGeneratedOnAdd();
            //modelBuilder.Entity<Piece>()
            //    .Property<DateTime?>(p => p.LastUpdate)
            //    .HasDefaultValueSql("NOW() ON UPDATE NOW()")
            //    .ValueGeneratedOnAddOrUpdate();

            ////Setlist
            //modelBuilder.Entity<Setlist>()
            //    .Property<DateTime?>(p => p.CreationDate)
            //    .HasDefaultValueSql("NOW()")
            //    .ValueGeneratedOnAdd();
            //modelBuilder.Entity<Setlist>()
            //    .Property<DateTime?>(p => p.LastUpdate)
            //    .HasDefaultValueSql("NOW() ON UPDATE NOW()")
            //    .ValueGeneratedOnAddOrUpdate();

            ////SetlistItem
            //modelBuilder.Entity<SetlistItem>()
            //    .Property<DateTime?>(p => p.CreationDate)
            //    .HasDefaultValueSql("NOW()")
            //    .ValueGeneratedOnAdd();
            //modelBuilder.Entity<SetlistItem>()
            //    .Property<DateTime?>(p => p.LastUpdate)
            //    .HasDefaultValueSql("NOW() ON UPDATE NOW()")
            //    .ValueGeneratedOnAddOrUpdate();

            ////Sheet
            //modelBuilder.Entity<Sheet>()
            //    .Property<DateTime?>(p => p.CreationDate)
            //    .HasDefaultValueSql("NOW()")
            //    .ValueGeneratedOnAdd();
            //modelBuilder.Entity<Sheet>()
            //    .Property<DateTime?>(p => p.LastUpdate)
            //    .HasDefaultValueSql("NOW() ON UPDATE NOW()")
            //    .ValueGeneratedOnAddOrUpdate();
        }

        protected override void OnModelCreatingImpl(ModelBuilder modelBuilder)
        {
            
        }
    }
}
