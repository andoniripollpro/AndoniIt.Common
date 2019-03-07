using MovistarPlus.Common.Dto;
using System.Collections.Generic;

namespace MovistarPlus.Common.Interface
{
	public interface IIncidenceOperationDao
	{
		List<IncidenceOperationDto> GetNFromGroup(string groupName, int n);
		bool Record(IncidenceOperationDto incidenceOperation);
	}
}