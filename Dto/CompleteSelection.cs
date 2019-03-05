using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MovistarPlus.Common.Dto
{
	public class CompleteSelection
    {
        public delegate void SelectionChanged();
        public event SelectionChanged SelectionChangedEvent;
        
        public CompleteSelection(ConfigBaseFolderDto configBaseFolderDto)
        {
            this.ConfigBaseFolderDto = configBaseFolderDto;

            this.Devices.CollectionChanged += HandleAddingEvent;
        }

        private EnvironmentDestinationEnum? environment = null;
        public EnvironmentDestinationEnum? Environment
        {
            get
            {
                return this.environment;
            }
            set
            {
                this.environment = value;
                CallSelectionChangedEvent();
            }
        }
        private ConfigFileType configFileType = null;

        public ConfigFileType ConfigFileType
        {
            get
            {
                return this.configFileType;
            }
            set
            {
                this.configFileType = value;
                this.Devices.Clear();
                CallSelectionChangedEvent();
            }
        }

        private void CallSelectionChangedEvent()
        {
            SelectionChangedEvent?.Invoke();
        }

        public bool IsValid
        {
            get
            {
                return this.ConfigBaseFolderDto != null && IsDeviceValid && this.GetSelectionList().Count > 0;
            }
        }
        public bool IsDeviceValid
        {
            get
            {
                return this.ConfigBaseFolderDto != null && this.Environment.HasValue && this.configFileType != null && this.Devices.Count > 0;
            }
        }
        public bool IsSpecialAllProfiles
        {
            get
            {
                return this.Environment == EnvironmentDestinationEnum.Production || this.Environment == EnvironmentDestinationEnum.Laboratory;
            }
        }
        public bool MultiFileByDevice
        {
            get
            {
                return this.ConfigFileType != null ? this.ConfigFileType.IsMultiFileByDevice : false;
            }
        }
        public override string ToString()
        {
            string result = "ERROR";
            if (this.IsValid)
            {
                DeviceDto device = this.Devices.Count == 1 ? this.Devices[0] : new DeviceDto() { Name = "Varios" };
                var selectionList = this.GetSelectionList();
                string profileName = selectionList.Count > 1 ? "Varios" : selectionList[0].FileName;
                if (MultiFileByDevice)
                    result = string.Format("{0}-{1}-{2}-{3}", this.Environment, this.ConfigFileType?.Name, device.Name, profileName);
                else
                    result = string.Format("{0}-{1}-{2}", this.Environment, this.ConfigFileType?.Name, device.Name);
            }
            return result;
        }

        public ObservableCollection<DeviceDto> Devices { get; set; } = new ObservableCollection<DeviceDto>();

        public List<DeploymentHistoryEvent> DeploymentHistory { get; set; } = new List<DeploymentHistoryEvent>();

        private string comments = string.Empty;        
        public string Comments {
            get
            {
                return this.comments;
            }
            set
            {
                this.comments = value;
                CallSelectionChangedEvent();
            }
        }

        public DeviceDto LastSelectedDevice { get; set; }
		public ConfigBaseFolderDto ConfigBaseFolderDto { get; set; }

		private void HandleAddingEvent(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is ObservableCollection<DeviceDto>)
            {
                var senderT = (ObservableCollection<DeviceDto>)sender;
                if (e.NewItems?.Count > 0 && e.NewItems[0] is DeviceDto)
                {
                    var newItem = (DeviceDto)e.NewItems[0];
                    newItem.SelectionChangedEvent += CallSelectionChangedEvent;
                    this.LastSelectedDevice = newItem;
                }
                else
                {
                    if (senderT.Count > 0)
                        this.LastSelectedDevice = senderT[0];
                    else
                        this.LastSelectedDevice = null;
                }
            }

            CallSelectionChangedEvent();
        }

        public List<ConfigFileSelection> GetSelectionList()
        {
            if (this.environment == null || this.configFileType == null)
                return new List<ConfigFileSelection>();

            List<ConfigFileSelection> selectionList = new List<ConfigFileSelection>();

            foreach (var oneDevice in this.Devices)
            {
                List<string> fileNames = new List<string>();
                if (this.ConfigFileType.IsMultiFileByDevice)                
                    fileNames.AddRange(oneDevice.FileNames);                
                else
                    fileNames.Add(string.Empty);

                foreach (string oneFileName in fileNames)
                {
					var configFileSelection = new ConfigFileSelection(this.ConfigBaseFolderDto, this.Environment.Value, this.ConfigFileType, oneDevice.Clone(), oneFileName);                    
                    selectionList.Add(configFileSelection);
                }                    
            }

            return selectionList;
        }
    }
}
