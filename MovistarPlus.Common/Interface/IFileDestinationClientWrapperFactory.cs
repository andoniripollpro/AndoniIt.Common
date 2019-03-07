using System.Collections.Generic;
using MovistarPlus.Common.Dto;

namespace MovistarPlus.Common.Interface
{
	public interface IFileDestinationClientWrapperFactory
	{
		IFileDestinationClientWrapper GetFileDestinationClientByConfigTypeFile(string fileDestinationTypeName, Dictionary<string, object> fileDetinationsConfigValues);

		void LoadDeviceDtoWithFileDestinationClientWrappers(DeviceDto deviceDto, Dictionary<string, object> fileDetinationsConfigValues);

		CompleteSelection LoadCompleteSelectionWithFileDestinationClientWrappers(CompleteSelection completeSelection, Dictionary<string, object> fileDetinationsConfigValues);
	}
}