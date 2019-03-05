using System;

namespace MovistarPlus.Common.Dto
{
	public class IncidenceOperationDto
	{
		public DateTime GmtDateTime { get; set; }
		public string GroupName { get; set; }
		public string ToStateStr { get; set; }
		public DeviceGroupIncidenceState.IncidenceStateEnum ToState
		{
			get {
				switch (this.ToStateStr)
				{
					case "I":
						return DeviceGroupIncidenceState.IncidenceStateEnum.Incidencia;
					case "P":
						return DeviceGroupIncidenceState.IncidenceStateEnum.Produccion;
					default:
						return DeviceGroupIncidenceState.IncidenceStateEnum.Desconocido;
				}
			}
			set {
				switch (value)
				{
					case DeviceGroupIncidenceState.IncidenceStateEnum.Incidencia:
						this.ToStateStr = "I";
						break;
					case DeviceGroupIncidenceState.IncidenceStateEnum.Produccion:
						this.ToStateStr = "P";
						break;
					default:
						this.ToStateStr = string.Empty;
						break;
				}
			}
		}		
		public string UserName { get; set; }
		public string Description { get; set; }
		public string ErrorMessage { get; set; } = string.Empty;
		public override string ToString()
		{
			return string.Format("GroupName-GmtDate: {0}-{1:yyyy/MM/dd HH:mm:ss} State: {2} Desc: '{3}' Error: '{4}'", 
				this.GroupName, this.GmtDateTime, this.ToState, this.Description, this.ErrorMessage);
		}
		public string IdToString()
		{
			return string.Format("{0}-{1:yyyyMMdd HH:mm}", this.GroupName, this.GmtDateTime);
		}
	}
}
