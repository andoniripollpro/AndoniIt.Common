using System.Net;

namespace MovistarPlus.Common
{
    public interface IFileSystemWithCredentials
    {
        bool FileExists(string fileAddress, NetworkCredential credentials);
    }
}
