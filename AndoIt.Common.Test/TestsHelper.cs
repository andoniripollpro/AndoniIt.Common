using Moq;
using AndoIt.Common.Common;
using AndoIt.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndoIt.Common.Test
{
	public class TestsHelper
	{
		public Mock<IIoCObjectContainer> InizializeIoCObjectContainer()
		{			
			var objectContainerMock = new Mock<IIoCObjectContainer>();
			objectContainerMock.Setup(x => x.Get<ILog>(null)).Returns(new Mock<ILog>().Object);
			IoCObjectContainer.Singleton = objectContainerMock.Object;
			return objectContainerMock;
		}
	}
}
