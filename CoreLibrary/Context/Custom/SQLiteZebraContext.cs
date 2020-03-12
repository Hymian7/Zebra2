﻿using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreatedImpl(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException("SQLite Default Values for Timestamps are not implemented yet.");
        }

        protected override void OnModelCreatingImpl(ModelBuilder modelBuilder)
        {
            
        }
    }
}