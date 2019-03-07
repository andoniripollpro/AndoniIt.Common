using System;
using System.Runtime.Serialization;

namespace MovistarPlus.Common.Dto
{
	public class Envelope<T>
	{
		public T Content { get; set; }
		public bool Error { get; set; } = false;
		public Exception Exception { get; set; } = null;
		public Version Version { get; set; } = null;
	}
}
