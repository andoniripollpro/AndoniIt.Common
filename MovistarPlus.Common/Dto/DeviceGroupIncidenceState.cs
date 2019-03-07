namespace MovistarPlus.Common.Dto
{
	public class DeviceGroupIncidenceState
	{
		public DeviceGroupIncidenceState(IncidenceStateEnum incidenceState, IncidenceStateEnum? incidenceStateToGoTo, FilesStateEnum filesState, string description = "", bool completeSwichThroughErrors = false)
		{			
			this.Description = description;

			if (filesState == FilesStateEnum.NoOperativo)
				incidenceState = IncidenceStateEnum.Desconocido;
			else if (!string.IsNullOrWhiteSpace(description))
				filesState = FilesStateEnum.Sucio;			
			
			this.FilesState = filesState;
			this.IncidenceState = incidenceState;
			this.IncidenceStateToGoTo = incidenceStateToGoTo;
			this.CompleteSwichThroughErrors = completeSwichThroughErrors;
		}
		public IncidenceStateEnum IncidenceState { get; set; } = IncidenceStateEnum.Desconocido;

		public IncidenceStateEnum? IncidenceStateToGoTo { get; set; } = null;

		public bool CompleteSwichThroughErrors { get; set; }

		public FilesStateEnum FilesState { get; set; } = FilesStateEnum.Sucio;

		public string Description { get; set; } = string.Empty;

		public enum IncidenceStateEnum
		{
			Produccion,
			Incidencia,
			Desconocido,
			DispositivoCaido
		}
		public enum FilesStateEnum
		{
			TodoBien,
			Sucio,
			NoOperativo
		}
	}
}