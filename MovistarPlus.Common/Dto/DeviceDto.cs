using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using static MovistarPlus.Common.Dto.CompleteSelection;

namespace MovistarPlus.Common.Dto
{
	public class DeviceDto : IEquatable<DeviceDto>
    {
        public event SelectionChanged SelectionChangedEvent;

        public DeviceDto()
        {
            this.FileNames.CollectionChanged += HandleAddingEvent;
        }

        public string Name { get; set; }
        public string PictureFile { get; set; }

        public ObservableCollection<string> FileNames { get; set; } = new ObservableCollection<string>();
		public List<string> Versions{ get; set; } = new List<string>();
		public List<ConfigFileTypesRelation> ConfigFileTypesRelations { get; set; } = new List<ConfigFileTypesRelation>();

		public bool Equals(DeviceDto other)
        {
            if (other == null) return false;
            return this.Name == other.Name;
        }

        private void HandleAddingEvent(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectionChangedEvent?.Invoke();
        }

		public DeviceDto Clone()
		{
			DeviceDto cloned = new DeviceDto()
			{
				Name = this.Name,
				PictureFile = this.PictureFile,				
				Versions = this.Versions.ToList(),
				ConfigFileTypesRelations = this.ConfigFileTypesRelations.ToList(),
				SelectionChangedEvent = this.SelectionChangedEvent
			};
			return cloned;
		}

		public ConfigFileTypesRelation GetConfigTypeRelationByConfigType(ConfigFileType configFileType)
		{
			return this.ConfigFileTypesRelations.Where(x => x.ConfigTypeId == configFileType.Id).FirstOrDefault();
		}
	}
}
