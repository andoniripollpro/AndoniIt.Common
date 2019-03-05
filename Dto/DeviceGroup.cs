using MovistarPlus.Common.Interface;
using System.Collections.Generic;

namespace MovistarPlus.Common.Dto
{
	public class DeviceGroupDto
	{
		public const string TODOS = "TODOS";

		public string Name { get; set; }
		public string PictureFile { get; set; }
		public List<DeviceDto> Devices { get; private set; } = new List<DeviceDto>();
		public string FileSpecialName { get; set; } 
		public IIncidenceService IncidenceService { get; set; }
		public List<DeviceGroupIncidenceState> DeviceGroupStates { get; private set; } = new List<DeviceGroupIncidenceState>();
		public DeviceGroupIncidenceState UnifiedDeviceGroupState {
			get {
				return this.IncidenceService.UnifyKaos(this.DeviceGroupStates);
			}
		}
		public void RefreshGroupState()
		{
			List<DeviceGroupIncidenceState> result = new List<DeviceGroupIncidenceState>();
			var historicStates = this.IncidenceService.GetNFromGroup(this.Name);
			DeviceGroupIncidenceState.IncidenceStateEnum previousRecordedState = historicStates.Count > 0 ? historicStates[0].ToState : DeviceGroupIncidenceState.IncidenceStateEnum.Produccion;

			//TODO: Meter devicesDto con sus carpetas de versión
			this.Devices.ForEach(x => result.Add(this.IncidenceService.GetDeviceFileState(x, previousRecordedState, this.FileSpecialName)));

			this.DeviceGroupStates = result;
		}
	}
}
