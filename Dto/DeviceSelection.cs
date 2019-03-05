using System;

namespace MovistarPlus.Common.Dto
{
    public class DeviceSelection : IEquatable<DeviceSelection>
    {
        public EnvironmentDestinationEnum Environment { get; set; }
        public ConfigFileType ConfigFileType { get; set; }
        public DeviceDto Device { get; set; }
        public string FileName { get; set; }    

        public override string ToString()
        {
            return string.Format("{0}-{1}-{2}", this.Environment, this.ConfigFileType?.Name, this.Device.Name); ;
        }

        public bool Equals(DeviceSelection other)
        {
            if (this.Environment == other.Environment
                && this.ConfigFileType.Id == other.ConfigFileType.Id
                && this.Device.Equals(other))
                return true;
            else
                return false;
        }
    }
}
