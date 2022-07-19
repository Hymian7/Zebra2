using System;
using System.Collections.Generic;
using System.Text;

namespace Zebra.Library
{
    public static class ZebraDbManagerFactory
    {
        public static IZebraDBManager GetManager(ZebraConfig config)
        {
            switch (config.RepositoryType)
            {
                case RepositoryType.Local:
                    return new ZebraDBManager(config);
                    break;
                case RepositoryType.Remote:
                    return new RemoteZebraDbManager(config);
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            
        }
    }
}
