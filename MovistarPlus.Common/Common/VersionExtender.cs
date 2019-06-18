using System;

namespace MovistarPlus.Common.Common
{
	public static class VersionExtender
	{
		public static DateTime GetCreationDate(this Version isThis)
		{
			return new DateTime(2000, 1, 1).AddDays(isThis.Build).AddSeconds(isThis.MinorRevision * 2);
		}
	}
}
