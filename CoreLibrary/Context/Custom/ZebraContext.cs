﻿using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Zebra.Library.Services;

namespace Zebra.Library
{
    public abstract partial class ZebraContext : DbContext
    {
        /// <summary>
        /// Zebra Settings for the Database Connection
        /// </summary>
        public ZebraConfig Settings { get; set; }

        public ZebraContext(ZebraConfig zebraConfig) {

            this.Settings = zebraConfig;

        }

        public ZebraContext(ZebraConfigurationService configurationService)
        {
            this.Settings = new ZebraConfig("Server Configuration", new System.IO.DirectoryInfo(configurationService.GetRepositoryDirectory()));
        }

        /// <summary>
        /// Only for EF Core Migrations
        /// </summary>
        //public ZebraContext()
        //{
        //}
    }
}
