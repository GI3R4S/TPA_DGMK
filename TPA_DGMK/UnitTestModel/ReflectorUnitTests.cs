using System;
using BusinessLogic.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestModel
{
    [TestClass]
    public class ReflectorUnitTests
    {
        internal static Reflector reflector;
        internal static string path;

        [TestInitialize]
        public void Initialize()
        {
            path = "./../../../BusinessLogic/bin/Debug/BusinessLogic.dll";
        }

        [TestMethod]
        public void ConstructorTest()
        {
            reflector = new Reflector(path);
            Assert.AreEqual("BusinessLogic.dll", reflector.AssemblyMetadata.Name);
            Assert.ThrowsException<ArgumentNullException>(() => new Reflector(""));
        }
    }
}
