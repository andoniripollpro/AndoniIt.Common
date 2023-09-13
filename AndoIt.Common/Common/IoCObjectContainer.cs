using AndoIt.Common.Interface;
using System.Configuration;

namespace AndoIt.Common.Common
{
	public static class IoCObjectContainer
	{
		private static IIoCObjectContainer singleton = null;
		private static readonly object theLock = new object();
		
		public static IIoCObjectContainer Singleton {
			get {
				lock (IoCObjectContainer.theLock)
				{
					return IoCObjectContainer.singleton ?? throw new ConfigurationErrorsException("IoCObjectContainer.Singleton sin asignar. Es necesario inicializarlo IoCObjectContainer.Singleton = tuIoCContainer");
				}
			}
			set {
				lock (IoCObjectContainer.theLock)
				{
					IoCObjectContainer.singleton = value;
				}
			}
		}
	}
}
