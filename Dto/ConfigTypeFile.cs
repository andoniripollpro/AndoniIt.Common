using MovistarPlus.Common;
using System.Runtime.Serialization;

namespace MovistarPlus.Common.Dto
{
	[DataContract]
	public class ConfigFileType
    {
        public ConfigFileType(string nameId)
        {
            Id = nameId;
            Name = new StringExtender(nameId).FromPascalCaseToText();
        }
		[DataMember]
		public string Id { get; set; }
		[DataMember]
		public string Name { get; set; }
        public bool IsMultiFileByDevice { get { return this.RelativeLocalAddressExpression.Contains("*"); } }
		[DataMember]
		public string RelativeLocalAddressExpression { get; set; }
		[DataMember]
		public string TransformFileTo { get; set; }
		public bool TransformFile { get { return this.TransformFileTo != null; } }		
    }
}
