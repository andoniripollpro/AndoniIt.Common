﻿using System.Net;

namespace AndoIt.Common
{
    public interface IFileSystemWithCredentials
    {
        bool FileExists(string fileAddress, NetworkCredential credentials);
        void DirectoryDelete(string toDelete, NetworkCredential credentials);
    }
}
