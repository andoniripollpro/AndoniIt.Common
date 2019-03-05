using System.Collections.Generic;

namespace MovistarPlus.Common.Interface
{
	public interface IFileDestinationClientWrapper
	{
		string UploadCompleteAddress { get; }
		string RefreshUrl { get; set; }
		bool Exist(string cdnDestination);
		List<string> Dir(string cdnFolderLocation);
		void Send(string originFileCompletePath, string remoteDirectory);
        void Get(string cdnDirectory, string originFileName, string destinationFileCompletePath);        
        void Rename(string cdnDestination, string renamed);
		void Copy(string origin, string destinationDirectoryNFileNae);
		void Delete(string cdnDestination);
    }
}