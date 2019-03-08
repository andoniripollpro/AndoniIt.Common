using Moq;
using MovistarPlus.Common.Common;
using MovistarPlus.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovistarPlus.Common.Test
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
