using Moq;
using AndoIt.Common.Common;
using AndoIt.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndoIt.Common.Infrastructure;
using AndoIt.Common;
using System.Threading;

namespace AndoIt.Common.Test
{
	public class TestsHelper
	{
		private Mock<ILog> mockLog;
		private Mock<IEnqueuerClient> mockEnquerClient;
		private Mock<IEnqueable> mockEnqueable;
		private Mock<IHttpClientAdapter> mockHttpClientAdapter;

        public TestsHelper()
		{
			this.mockLog = new Mock<ILog>();
			this.mockEnquerClient = new Mock<IEnqueuerClient>();
			this.mockEnqueable = new Mock<IEnqueable>();
			this.mockHttpClientAdapter = new Mock<IHttpClientAdapter>();
			this.mockHttpClientAdapter.Setup(x => x.AllCookedUpGet(It.IsAny<string>(), null))
				.Returns("{\"log\":{\"forbiddenWords\":[]}, \"UnJSon\":\"Válido\"}");
		}

		public Mock<ILog> MockLog => this.mockLog;
		public Mock<IEnqueuerClient> MockEnquerClient => this.mockEnquerClient;
		public Mock<IEnqueable> MockEnqueable => this.mockEnqueable;
		public Mock<IHttpClientAdapter> MockHttpClientAdapter => this.mockHttpClientAdapter;

		public Mock<IIoCObjectContainer> InizializeIoCObjectContainer()
		{			
			var objectContainerMock = new Mock<IIoCObjectContainer>();
			objectContainerMock.Setup(x => x.Get<ILog>(null)).Returns(new Mock<ILog>().Object);
			IoCObjectContainer.Singleton = objectContainerMock.Object;
			return objectContainerMock;
		}

		public string WaitsMillsecondsThenReturnsString(int milliseconds, string returnStr)
		{
			Thread.Sleep(milliseconds);
			return returnStr;
		}
	}
}
