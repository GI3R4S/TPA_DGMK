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
            path = "./../../../DllForTests/ApplicationArchitecture/bin/Debug/TPA.ApplicationArchitecture.dll";
        }

        [TestMethod]
        public void ConstructorTest()
        {
            reflector = new Reflector(path);
            Assert.AreEqual("TPA.ApplicationArchitecture.dll", reflector.AssemblyMetadata.Name);
            Assert.ThrowsException<ArgumentNullException>(() => new Reflector(""));
        }
    }
}
