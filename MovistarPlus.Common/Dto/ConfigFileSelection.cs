using MovistarPlus.Common.Interface;
using System;
using System.IO;

namespace MovistarPlus.Common.Dto
{
	public class ConfigFileSelection : IEquatable<ConfigFileSelection>
    {
		public ConfigFileSelection(ConfigBaseFolderDto configBaseFolderDto, EnvironmentDestinationEnum environment, ConfigFileType configFileType, DeviceDto device, string fileName)
		{
			this.ConfigBaseFolderDto = configBaseFolderDto ?? throw new ArgumentNullException("configBaseFolderDto");
			this.Environment = environment;
			this.ConfigFileType = configFileType ?? throw new ArgumentNullException("configFileType");
			this.Device = device ?? throw new ArgumentNullException("device");
			this.FileName = fileName ?? string.Empty;
		}

		//	Id
		public EnvironmentDestinationEnum Environment { get; private set; }
        public ConfigFileType ConfigFileType { get; private set; }
        public DeviceDto Device { get; private set; }
        public string FileName { get; private set; }
		//	End Id

		public ConfigBaseFolderDto ConfigBaseFolderDto { get; set; }
		private ConfigFileTypesRelation ConfigFileTypesRelation
		{
			get
			{
				return this.Device.GetConfigTypeRelationByConfigType(this.ConfigFileType);
			}
		}
		//	This implies convert to Json
		public string TransformFileTo {
			get
			{
				return this.ConfigFileTypesRelation.ConfigFileType.TransformFileTo;
			}
		}
		//	This implies convert to Json
		public bool TransformFile {
			get {
				return this.ConfigFileTypesRelation.ConfigFileType.TransformFile && this.ConfigFileTypesRelation.TransformFile;
			}
		}
		public IFileDestinationClientWrapper FileDestinationClientWrapper {
			get
			{
				return this.ConfigFileTypesRelation.FileDestinationClientWrapper;
			}
		}

		#region Directories
		public string RelativeLocalAddressNFileName 
		{
			get
			{
				string configFileTypeExpresion = this.ConfigFileType.RelativeLocalAddressExpression;
				configFileTypeExpresion = FillUpExpression(configFileTypeExpresion, false);
				configFileTypeExpresion = configFileTypeExpresion.Replace("..", ".");
				if (configFileTypeExpresion.Contains("*"))
					configFileTypeExpresion = $@"{Path.GetDirectoryName(configFileTypeExpresion)}\{Path.GetFileName(this.FileName)}";
				return configFileTypeExpresion;
			}
		}

		private string FillUpExpression(string expression, bool isDestination)
		{
			if (expression == null)
				return null;
			string deviceFolderName = this.Device.Name;
			if (isDestination)			
				deviceFolderName = this.Device.GetConfigTypeRelationByConfigType(this.ConfigFileType).FolderNameJustForDestination ?? deviceFolderName;
			
			expression = expression.Replace("{Device}", deviceFolderName);
			expression = expression.Replace("{Environment}",
				new EnumDecorator<EnvironmentDestinationEnum>(this.Environment).GetDescription());
			if (expression.Contains("{FromFileNameVersionFolder}"))
				expression = expression.Replace("{FromFileNameVersionFolder}", this.FromFileNameVersionFolder);
			return expression;
		}

		private string FromFileNameVersionFolder
		{
			get
			{
				string fileNameOnly = Path.GetFileName(this.FileName);
				int start = fileNameOnly.IndexOf('.') + 1;
				int legth = fileNameOnly.LastIndexOf('.') - start;
				string fromFileNameVersionFolder;
				if (legth >= 0)
					fromFileNameVersionFolder = fileNameOnly.Substring(start, legth);
				else
					fromFileNameVersionFolder = string.Empty;
				return fromFileNameVersionFolder;
			}
		}
		
		public string CompleteLocalAddressNFileName 
		{			
			get
			{
				return Path.GetFullPath($"{this.ConfigBaseFolderDto.BaseLocalRepositoryAddress}\\{this.RelativeLocalAddressNFileName}");
			}
		}

		public string CompleteBackupAddressNFileName
		{
			get
			{
				return Path.GetFullPath($"{this.ConfigBaseFolderDto.BaseLocalBackupAddress}\\{this.RelativeLocalAddressNFileName}");
			}
		}

		public string CompleteDestinationAddress
		{
			get
			{
				return FillUpExpression(this.ConfigFileTypesRelation.UploadCompleteAddress, true);
			}
		}
		public string CompleteDestinationAddressNFileName
		{
			get
			{
				string fileName = this.TransformFile? this.TransformFileTo : this.FileName;
				string slashOrBackslash = CompleteDestinationAddress != null ? CompleteDestinationAddress.Contains(@"\") ? @"\" :"/" : null;
				return $@"{CompleteDestinationAddress}{slashOrBackslash}{Path.GetFileName(fileName)}";
			}
		}

		//public IConfiguration Config { get; set; }
		#endregion

		public override string ToString()
        {
            string result = "ERROR";            

            result = string.Format("{0}-{1}-{2}-{3}", this.Environment, this.ConfigFileType?.Name, this.Device.Name, Path.GetFileName(this.FileName));         

            return result;
        }

        public bool Equals(ConfigFileSelection other)
        {
            if (this.Environment == other.Environment
                && this.ConfigFileType.Id == other.ConfigFileType.Id
                && this.Device == other.Device
                && this.FileName == other.FileName)
                return true;
            else
                return false;
        }
    }
}
