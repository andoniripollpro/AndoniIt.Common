using System.Collections.Generic;
using MovistarPlus.Common.Dto;

namespace MovistarPlus.Common.Interface
{
	public interface IIncidenceService
	{
		DeviceGroupIncidenceState GetDeviceFileState(DeviceDto device, DeviceGroupIncidenceState.IncidenceStateEnum previousRecordedStat, string fileSpecialName);
		List<IncidenceOperationDto> GetNFromGroup(string groupName, int n = 1);
		string SwitchToState(DeviceGroupDto deviceGroup, DeviceGroupIncidenceState.IncidenceStateEnum incidenceStateToSwichTo);
		DeviceGroupIncidenceState UnifyKaos(List<DeviceGroupIncidenceState> deviceGroupIncidenceStates);
	}
}