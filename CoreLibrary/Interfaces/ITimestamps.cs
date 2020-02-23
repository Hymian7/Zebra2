using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library
{
    interface ITimestamps
    {
        /// <summary>
        /// DateTime of Entity Creation
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// DateTime of Last Update
        /// </summary>
        public DateTime? LastUpdate { get; set; }

    }
}
