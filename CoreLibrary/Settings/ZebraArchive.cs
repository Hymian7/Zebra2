using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Zebra.Library
{
    public abstract class ZebraArchive
    {        
        /// <summary>
        /// Indicates, if a (Remote) Archive can be reached
        /// </summary>
        public abstract bool IsConnected { get; }

        /// <summary>
        /// Gets the Fileinfo for a specific Sheet
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public abstract FileInfo GetFile(Sheet sheet);
        
        /// <summary>
        /// Pushes a File for a given Sheet into the Archive
        /// </summary>
        /// <param name="file"></param>
        public abstract void PushFile(FileInfo file, Sheet sheet, FileImportMode mode = FileImportMode.Copy, bool _override = false);
    }

    public enum FileImportMode 
    {
        Copy,
        Move
    }
}
