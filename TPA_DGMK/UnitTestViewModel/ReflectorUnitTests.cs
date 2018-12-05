using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViewModel;

namespace UnitTestViewModel
{
    [TestClass]
    public class ReflectorUnitTests
    {
        internal static Reflector reflector;
        internal static string path;

        [TestInitialize]
        public void Initialize()
        {
            path = "./../../../Model/bin/Debug/netstandard2.0/Model.dll";
        }

        [TestMethod]
        public void ConstructorTest()
        {
            reflector = new Reflector(path);
            Assert.AreEqual("Model.dll", reflector.AssemblyMetadata.Name);
            Assert.ThrowsException<ArgumentNullException>(() => new Reflector(""));
        }
    }
}
