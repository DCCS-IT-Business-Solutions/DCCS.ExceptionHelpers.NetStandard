using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DCCS.ExceptionHelpers.NetStandard.Tests
{
    [TestClass]
    public class ExceptionExtensionTests
    {

        [TestMethod]
        public void TestHirachy()
        {
            try
            {
                TestFunction();
                Assert.Fail("No exception in test function thrown");
            }
            catch(Exception e)
            {
                var result = e.GetAllExceptionsInHirachy();
                Assert.IsNotNull(result);
                var exceptions = result.ToArray();
                Assert.AreEqual(3, exceptions.Length);
                Assert.IsTrue(exceptions[0] is ApplicationException);
                Assert.IsTrue(exceptions[1] is Exception);
                Assert.IsTrue(exceptions[2] is FileNotFoundException);
            }
        }


        [TestMethod]
        public void TestMessage()
        {
            try
            {
                TestFunction();
                Assert.Fail("No exception in test function thrown");
            }
            catch (Exception e)
            {
                var message = e.BuildCompleteMessage(false);
                Assert.IsNotNull(message);
                Assert.IsTrue(message.StartsWith($"Start program failed{Environment.NewLine}Read configuration failed{Environment.NewLine}")); // We check only the first part of the message, because the last part is depending on the installation an framework version
            }
        }

        [TestMethod]
        public void TestMessageWithSpecialSeparator()
        {
            try
            {
                TestFunction();
                Assert.Fail("No exception in test function thrown");
            }
            catch (Exception e)
            {
                var message = e.BuildCompleteMessage(false, "-");
                Assert.IsNotNull(message);
                Assert.IsTrue(message.StartsWith($"Start program failed-Read configuration failed-")); // We check only the first part of the message, because the last part is depending on the installation an framework version
            }
        }

        void TestFunction()
        {
            try
            {
                ReadConfig(); // Will throw an exception
            }
            catch (Exception e)
            {
                throw new ApplicationException("Start program failed", e);
            }
        }
        void ReadConfig()
        {
            try
            {
                // This will raise an FileNotFound exception
                using (var configFile = File.OpenRead(Path.Combine(Path.GetTempPath(), @"NOTEXISTINGCONFIGFILE.XXX")))
                {

                }
            }
            catch (Exception e)
            {
                throw new Exception("Read configuration failed", e);
            }
        }
    }
}