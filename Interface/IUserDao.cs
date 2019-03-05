using MovistarPlus.Common.Dto;
using System.Collections.Generic;

namespace MovistarPlus.Common.Interface
{
	public interface IUserDao
    {
        UserDto GetUserByUserPass(string user, string pass);

		List<DeviceDto> GetConfigurationDevices();

		List<ConfigFileType> GetConfigurationConfigFileTypes();
	}
}
