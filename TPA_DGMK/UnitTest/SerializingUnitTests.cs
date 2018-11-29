using Data_De_Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.IO;
using System.Linq;
using ViewModel;

namespace UnitTest
{
    [TestClass]
    public class SerializingUnitTests
    {
        internal static string path;
        internal static string path2;
        internal static string pathTarget;
        internal static Reflector reflector;
        internal static SerializerTemplate serializer;

        [TestInitialize]
        public void Initialize()
        {
            path = "./../../../UnitTest/bin/Debug/Model.dll";
            path2 = "./../../../UnitTest/bin/Debug/ViewModel.dll";
            pathTarget = "./../../../UnitTest/bin/Debug/xmlModel.xml";
            reflector = new Reflector(path);
            serializer = new XMLSerializer();
        }

        [TestMethod]
        public void XMLSerializerSerializeTest()
        {
            if (File.Exists(pathTarget))
            {
                File.Delete(pathTarget);
            }
            Assert.IsFalse(File.Exists(pathTarget));
            serializer.Serialize(reflector.AssemblyMetadata, pathTarget);
            Assert.IsTrue(File.Exists(pathTarget));
        }

        [TestMethod]
        public void XMLSerializerDeserializeTest()
        {
            Assert.AreEqual("Model.dll", reflector.AssemblyMetadata.Name);
            serializer.Serialize(reflector.AssemblyMetadata, pathTarget);
            reflector = new Reflector(path2);
            Assert.AreEqual("ViewModel.dll", reflector.AssemblyMetadata.Name);
            AssemblyMetadata assemblyMetadata = serializer.Deserialize<AssemblyMetadata>(pathTarget);
            reflector = new Reflector(assemblyMetadata);
            Assert.AreEqual(assemblyMetadata.Name, reflector.AssemblyMetadata.Name);
        }

        [TestMethod]
        public void XMLNamespacesTest()
        {
            serializer.Serialize(reflector.AssemblyMetadata, pathTarget);
            AssemblyMetadata assemblyMetadata = serializer.Deserialize<AssemblyMetadata>(pathTarget);
            Assert.AreEqual(reflector.AssemblyMetadata.Namespaces.Count(), assemblyMetadata.Namespaces.Count());
            for (int i = 0; i < reflector.AssemblyMetadata.Namespaces.Count(); i++)
            {
                Assert.AreEqual(reflector.AssemblyMetadata.Namespaces.ElementAt(i).NamespaceName,
                    assemblyMetadata.Namespaces.ElementAt(i).NamespaceName);
            }
        }

        [TestMethod]
        public void XMLTypesTest()
        {
            serializer.Serialize(reflector.AssemblyMetadata, pathTarget);
            AssemblyMetadata assemblyMetadata = serializer.Deserialize<AssemblyMetadata>(pathTarget);
            for (int i = 0; i < reflector.AssemblyMetadata.Namespaces.Count(); i++)
            {
                Assert.AreEqual(reflector.AssemblyMetadata.Namespaces.ElementAt(i).Types.Count(),
                    assemblyMetadata.Namespaces.ElementAt(i).Types.Count());
                Assert.AreEqual(reflector.AssemblyMetadata.Namespaces.ElementAt(i).GetType(),
                    assemblyMetadata.Namespaces.ElementAt(i).GetType());
            }
        }
    }
}
