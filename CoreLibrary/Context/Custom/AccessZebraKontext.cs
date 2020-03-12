using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library
{
    class AccessZebraContext:ZebraContext
    {
        public AccessZebraContext(ZebraConfig config):base(config)
        {

        }

        protected override void CustomInit(DbContextOptionsBuilder optionsBuilder)
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreatedImpl(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException("Acces Default Values for Timestamps are not implemented yet.");
        }

        protected override void OnModelCreatingImpl(ModelBuilder modelBuilder)
        {
            
        }
    }
}
