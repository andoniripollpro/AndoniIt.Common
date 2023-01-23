using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AndoIt.Common.Test.Common
{
    [TestClass]
    public class GoodConfigOnResapiTest
    {
        [TestMethod]
        public void Ctor_HttpClientAdapterNormal_NormalLoad()
        {
            //	Arrange
            var helper = new TestsHelper();            
            string expected = "Válido";

            //	Act
            var toTest = new GoodConfigOnResapi(helper.InizializeIoCObjectContainer().Object, "http://urlvalida.test", 1, helper.MockHttpClientAdapter.Object);
            string actual = toTest.GetAsString("UnJSon");

            //	Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        /// En realidad testeamos GetRootJString
        public void GetAsString_HttpClientAdapterNormalException_NormalLoadOldValue()
        {
            //	Arrange
            var helper = new TestsHelper();
            string expected = "Válido";

            //	Act
            var toTest = new GoodConfigOnResapi(helper.InizializeIoCObjectContainer().Object, "http://urlvalida.test", 1, helper.MockHttpClientAdapter.Object);
            helper.MockHttpClientAdapter.Setup(x => x.AllCookedUpGet(It.IsAny<string>(), null)).Throws(new Exception("Por alguna razón no puede acceder"));
            Thread.Sleep(1001);
            string actual = toTest.GetAsString("UnJSon");

            //	Assert
            Assert.AreEqual(expected, actual);
            Thread.Sleep(100);
            helper.MockHttpClientAdapter.Verify(x => x.AllCookedUpGet(It.IsAny<string>(), null), Times.Exactly(3)); // La 1 al construir el objeto y la excepción lo intenta 2 veces porque así está hardcodeado
        }
                
        [TestMethod]
        public void GetAsString_HttpClientAdapterSlow_LoadOldValue()
        {
            //	Arrange
            var helper = new TestsHelper();
            helper.Log.Info("Arrange", new StackTrace());
            string expected = "Válido";            
            var toTest = new GoodConfigOnResapi(helper.InizializeIoCObjectContainer().Object, "http://urlvalida.test", 0, helper.FakeHttpClientAdapter);
            helper.FakeHttpClientAdapterToTune.FakeReturnValue = "{\"log\":{\"forbiddenWords\":[]}, \"UnJSon\":\"Nuevo valor\"}";
            helper.FakeHttpClientAdapterToTune.Freeze();
            toTest.SecondsToRefreshConfig = 1;
            Thread.Sleep(1001);
            
            //	Act
            helper.Log.Info("Before toTest.GetAsString");
            string actual = toTest.GetAsString("UnJSon");
            helper.Log.Info("After toTest.GetAsString");
            helper.FakeHttpClientAdapterToTune.Thaw();
            toTest.SecondsToRefreshConfig = 0;            

            //	Assert
            Assert.AreEqual(expected, actual);
            Thread.Sleep(1);
            Assert.AreEqual(2, helper.FakeHttpClientAdapterToTune.TimesCalled); // La 1 al construir el objeto y la segunda lectura lenta aunque no llegue a pillarlo
        }

        /// <summary>
        /// Este test no es válido en modo debug
        /// </summary>
        [TestMethod]
        public void GetAsStringEfficiency_HttpClientAdapterSlow_LoadOldValueInTime()
        {
            //	Arrange
            var helper = new TestsHelper();
            helper.Log.Info("Arrange", new StackTrace());
            string expected = "Válido";
            var toTest = new GoodConfigOnResapi(helper.InizializeIoCObjectContainer().Object, "http://urlvalida.test", 0, helper.FakeHttpClientAdapter);
            helper.FakeHttpClientAdapterToTune.FakeReturnValue = "{\"log\":{\"forbiddenWords\":[]}, \"UnJSon\":\"Nuevo valor\"}";
            helper.FakeHttpClientAdapterToTune.Freeze();
            toTest.SecondsToRefreshConfig = 1;
            Thread.Sleep(1001);

            //	Act
            helper.Log.Info("Before toTest.GetAsString");
            DateTime before = DateTime.Now;
            string actual = toTest.GetAsString("UnJSon");
            DateTime after= DateTime.Now;
            helper.Log.Info("After toTest.GetAsString");
            helper.FakeHttpClientAdapterToTune.Thaw();
            toTest.SecondsToRefreshConfig = 0;

            //	Assert
            helper.Log.Info($"Before time = {before:HH:mm:ss.ffff}. After time = {after:HH:mm:ss.ffff}");
            Assert.IsTrue((after - before).TotalMilliseconds < 20);            
            // Esto sobra para este test pero por asugurar
            Assert.AreEqual(expected, actual);            
            Assert.AreEqual(2, helper.FakeHttpClientAdapterToTune.TimesCalled); // La 1 al construir el objeto y la segunda lectura lenta aunque no llegue a pillarlo
        }

        [TestMethod]
        public void GetAsStringTwice_HttpClientAdapterSlow_NormalLoadNewValue()
        {
            //	Arrange
            var helper = new TestsHelper();
            string expected = "Nuevo valor";

            //	Act
            var toTest = new GoodConfigOnResapi(helper.InizializeIoCObjectContainer().Object, "http://urlvalida.test", 1, helper.MockHttpClientAdapter.Object);
            helper.MockHttpClientAdapter.Setup(x => x.AllCookedUpGet(It.IsAny<string>(), null))
                .Returns(helper.WaitsMillsecondsThenReturnsString(100, "{\"log\":{\"forbiddenWords\":[]}, \"UnJSon\":\"Nuevo valor\"}"));
            Thread.Sleep(1001);
            string actual = toTest.GetAsString("UnJSon");
            Thread.Sleep(101);
            actual = toTest.GetAsString("UnJSon");

            //	Assert
            Assert.AreEqual(expected, actual);            
        }

        [TestMethod]
        public void GetAsStringMultithread_HttpClientAdapterSlow_NormalLoadNewValue()
        {
            //	Arrange
            var helper = new TestsHelper();
            string expected = "Nuevo valor";

            //	Act
            var toTest = new GoodConfigOnResapi(helper.InizializeIoCObjectContainer().Object, "http://urlvalida.test", 1, helper.MockHttpClientAdapter.Object);
            helper.MockHttpClientAdapter.Setup(x => x.AllCookedUpGet(It.IsAny<string>(), null))
                .Returns(helper.WaitsMillsecondsThenReturnsString(100, "{\"log\":{\"forbiddenWords\":[]}, \"UnJSon\":\"Nuevo valor\"}"));
            Thread.Sleep(1001);
            var taskList = new List<Task>();
            for (int i = 0; i < 100; i++)
                taskList.Add(new Task(() => toTest.GetAsString("UnJSon")));
            taskList.ForEach(x => x.Start());
            Thread.Sleep(101);
            string actual = toTest.GetAsString("UnJSon");

            //	Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
