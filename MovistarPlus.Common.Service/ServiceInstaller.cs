using System.ComponentModel;

namespace MovistarPlus.Common.Service
{
	/// <summary>
	/// Instalador genérico que obtiene en nombre y la descripción del servicio en App.config, en appSettings, en las variables ServiceName y ServiceDescription
	/// </summary>
	[RunInstaller(true)]
	public class CodificacionInstaller : GenericInstaller
	{
	}
}