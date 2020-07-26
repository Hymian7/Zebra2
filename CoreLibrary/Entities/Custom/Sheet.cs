using System;


namespace Zebra.Library
{
    partial class Sheet
    {
        /// <summary>
        /// Checks, if the PDF File exists using the passed DBManager
        /// </summary>
        public bool FileExists(ZebraDBManager manager)
        {
            try
            {
                manager.Archive.GetFile(this);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

       public String DocumentPath(ZebraDBManager manager)
        {
            return manager.Archive.GetFile(this).FullName;
        }

    }
}
