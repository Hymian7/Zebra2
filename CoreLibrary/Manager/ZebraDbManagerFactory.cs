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

            //switch (config.RepositoryType)
            //{
            //    case RepositoryType.Local:
            //        return new RemoteZebraDbManager(config);
            //        //return new ZebraDBManager(config);

            //    case RepositoryType.Remote:
            //        return new RemoteZebraDbManager(config);

            //    default:
            //        throw new NotImplementedException();

            //}


        }
    }
}
