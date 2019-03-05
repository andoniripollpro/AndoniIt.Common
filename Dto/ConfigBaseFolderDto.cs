using System.Runtime.Serialization;

namespace MovistarPlus.Common.Dto
{
	[DataContract]
	public class ConfigBaseFolderDto
	{
		[DataMember]
		public string BaseLocalRepositoryAddress { get; set; }
		[DataMember]
		public string BaseLocalBackupAddress { get; set; }
	}
}
