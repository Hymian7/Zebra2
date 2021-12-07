using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library
{
    public static class ZebraDbManagerFactory
    {
        public static IZebraDBManager GetManager(ZebraConfig config)
        {
            return new RemoteZebraDbManager(config);
        }
    }
}
