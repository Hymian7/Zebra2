﻿using System;
using System.Collections.Generic;
using System.IO;

namespace PdfiumLight
{
    internal static class StreamManager
    {
        private static readonly object _syncRoot = new object();
        private static int _nextId = 1;
        private static readonly Dictionary<int, Stream> _files = new Dictionary<int, Stream>();

        public static int Register(Stream stream)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            lock (_syncRoot)
            {
                int id = _nextId++;
                _files.Add(id, stream);
                return id;
            }
        }

        public static void Unregister(int id)
        {
            lock (_syncRoot)
            {
                _files.Remove(id);
            }
        }

        public static Stream Get(int id)
        {
            lock (_syncRoot)
            {
                _files.TryGetValue(id, out Stream stream);
                return stream;
            }
        }
    }
}