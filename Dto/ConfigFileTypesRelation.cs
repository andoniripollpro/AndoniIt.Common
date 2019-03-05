using MovistarPlus.Common.Interface;
using System.Runtime.Serialization;

namespace MovistarPlus.Common.Dto
{
	[DataContract]
	public class ConfigFileTypesRelation
	{
		[DataMember]
		public string ConfigTypeId { get; set; }
		[DataMember]		
		public string FileDestinationTypeName { get; set; }
		[DataMember]
		public string FolderNameJustForDestination { get; set; }
		[DataMember]
		public bool Disabled { get; set; }
		[DataMember]
		public bool TransformFile { get; set; } = true;
		[DataMember]
		public ConfigFileType ConfigFileType { get; set; }
		public IFileDestinationClientWrapper fileDestinationClientWrapper { get; set; }
		public IFileDestinationClientWrapper FileDestinationClientWrapper {
			get { return this.fileDestinationClientWrapper; }
			set
			{
				this.fileDestinationClientWrapper = value;
				this.UploadCompleteAddress = value.UploadCompleteAddress;
			}
		}
		[DataMember]
		public string UploadCompleteAddress { get; set; }
	}
}