using Moq;
using AndoIt.Common.Common;
using AndoIt.Common.Interface;
using System.Diagnostics;
using AndoIt.Common.Infrastructure;
using System;
using System.Threading;

namespace AndoIt.Common.Test
{
    public class TestsHelper
	{
		private Mock<ILog> mockLog;
		private Mock<IEnqueuerClient> mockEnquerClient;
		private Mock<IEnqueable> mockEnqueable;
		private Mock<IHttpClientAdapter> mockHttpClientAdapter;
        private ILog log;
        private IHttpClientAdapter fakeHttpClientAdapter;

        public TestsHelper()
		{
			this.mockLog = new Mock<ILog>();
			this.log.Info("Starting", new StackTrace());
			this.mockEnquerClient = new Mock<IEnqueuerClient>();
			this.mockEnqueable = new Mock<IEnqueable>();
			this.mockHttpClientAdapter = new Mock<IHttpClientAdapter>();
			this.mockHttpClientAdapter.Setup(x => x.AllCookedUpGet(It.IsAny<string>(), null))
				.Returns("{\"log\":{\"forbiddenWords\":[]}, \"UnJSon\":\"Válido\"}");
			this.fakeHttpClientAdapter = new HttpClientAdapterFake(this.log, "{\"log\":{\"forbiddenWords\":[]}, \"UnJSon\":\"Válido\"}", 0);
		}

		public Mock<Interface.ILog> MockLog => this.mockLog;
		public Interface.ILog Log => this.log;
		public Mock<IEnqueuerClient> MockEnquerClient => this.mockEnquerClient;
		public Mock<IEnqueable> MockEnqueable => this.mockEnqueable;
		public Mock<IHttpClientAdapter> MockHttpClientAdapter => this.mockHttpClientAdapter;
		public IHttpClientAdapter FakeHttpClientAdapter => this.fakeHttpClientAdapter;
		public HttpClientAdapterFake FakeHttpClientAdapterToTune => (HttpClientAdapterFake)this.fakeHttpClientAdapter;

		public Mock<IIoCObjectContainer> InizializeIoCObjectContainer()
		{			
			var objectContainerMock = new Mock<IIoCObjectContainer>();
			objectContainerMock.Setup(x => x.Get<Interface.ILog>(null)).Returns(this.Log);
			IoCObjectContainer.Singleton = objectContainerMock.Object;
			return objectContainerMock;
		}

		public string WaitsMillsecondsThenReturnsString(int milliseconds, string returnStr)
		{
			this.log.Info("Start", new StackTrace());
			Thread.Sleep(milliseconds);
			this.log.Info("End", new StackTrace());
			return returnStr;
		}
	}
}
