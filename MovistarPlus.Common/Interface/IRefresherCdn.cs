using System.Collections.Generic;
using MovistarPlus.Common.Dto;

namespace MovistarPlus.Common.Interface
{
	public interface IRefresherCdn
	{
		void RefreshCaches(List<DeviceDto> devices, ConfigFileType configFileType);
	}
}