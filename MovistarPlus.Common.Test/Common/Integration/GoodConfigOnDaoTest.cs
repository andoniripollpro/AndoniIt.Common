using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovistarPlus.Common.Interface;

namespace MovistarPlus.Common.Test.Common.Integration
{
	[TestClass]
	public class GoodConfigOnDaoTest
	{
        private const string CONNECTIONSTRING = "Data Source=TVIDES.CAT.ES;Persist Security Info=True;User ID=PVR_USER;Password=user347pvr";
        //private const string CONNECTIONSTRING = "Data Source=//TVIDES.SOGECABLE.COM:1525/TVIDES.SOGECABLE.COM;Persist Security Info=True;User ID=PVR_USER;Password=user347pvr";

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
