using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AndoIt.Common.Interface;

namespace AndoIt.Common.Test.Common.Integration
{
	[TestClass]
	public class GoodConfigOnDaoTest
	{   
        public GoodConfigOnDaoTest()
		{
			Mock<IIoCObjectContainer> inizializeIoCObjectContainer = new TestsHelper().InizializeIoCObjectContainer();
		}

		[TestMethod]
		public void GetJNodeByTagAddress_Normal_StringGiven()
		{
			//	Arrange
			Mock<IIoCObjectContainer> inizializeIoCObjectContainer = new TestsHelper().InizializeIoCObjectContainer();
			GoodConfigOnDao toTest = new GoodConfigOnDao(inizializeIoCObjectContainer.Object, CONNECTIONSTRING, "SoloParaTest");

			//	Act

			//	Assert
			Assert.AreEqual(toTest.GetJNodeByTagAddress("Deployment").ToString(), "TEST");
		}
		[TestMethod]
		public void GetJNodeByTagAddress_Normal_ComplexObjectGiven()
		{
			//	Arrange
			Mock<IIoCObjectContainer> inizializeIoCObjectContainer = new TestsHelper().InizializeIoCObjectContainer();
			GoodConfigOnDao toTest = new GoodConfigOnDao(inizializeIoCObjectContainer.Object, CONNECTIONSTRING, "SoloParaTest");

			//	Act

			//	Assert
			Assert.AreEqual(toTest.GetJNodeByTagAddress("complex").Value<string>("attribute"), "attributeContent");
		}
	}
}
