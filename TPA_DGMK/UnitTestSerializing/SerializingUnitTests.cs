using System.IO;
using System.Linq;
using System.Reflection;
using Data_De_Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace UnitTestSerializing
{
    [TestClass]
    public class SerializingUnitTests
    {
        internal static string path;
        internal static string path2;
        internal static string pathTarget;
        internal static Assembly assembly;
        internal static AssemblyMetadata assemblyMetadata;
        internal static SerializerTemplate serializer;

        [TestInitialize]
        public void Initialize()
        {
            path = "./../../../UnitTestSerializing/bin/Debug/Model.dll";
            path2 = "./../../../UnitTestSerializing/bin/Debug/Data(de)Serialization.dll";
            pathTarget = "./../../../UnitTestSerializing/bin/Debug/xmlModel.xml";
            assembly = Assembly.LoadFrom(path);
            assemblyMetadata = new AssemblyMetadata(assembly);
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
            serializer.Serialize(assemblyMetadata, pathTarget);
            Assert.IsTrue(File.Exists(pathTarget));
        }

        [TestMethod]
        public void XMLSerializerDeserializeTest()
        {
            Assert.AreEqual("Model.dll", assemblyMetadata.Name);
            serializer.Serialize(assemblyMetadata, pathTarget);
            assembly = Assembly.LoadFrom(path2);
            assemblyMetadata = new AssemblyMetadata(assembly);
            Assert.AreEqual("Data(de)Serialization.dll", assemblyMetadata.Name);
            AssemblyMetadata assemblyMetadata2 = serializer.Deserialize<AssemblyMetadata>(pathTarget);
            assemblyMetadata = assemblyMetadata2;
            Assert.AreEqual(assemblyMetadata2.Name, assemblyMetadata.Name);
        }

        [TestMethod]
        public void XMLNamespacesTest()
        {
            serializer.Serialize(assemblyMetadata, pathTarget);
            AssemblyMetadata assemblyMetadata2 = serializer.Deserialize<AssemblyMetadata>(pathTarget);
            Assert.AreEqual(assemblyMetadata.Namespaces.Count(), assemblyMetadata2.Namespaces.Count());
            for (int i = 0; i < assemblyMetadata.Namespaces.Count(); i++)
            {
                Assert.AreEqual(assemblyMetadata.Namespaces.ElementAt(i).NamespaceName,
                    assemblyMetadata2.Namespaces.ElementAt(i).NamespaceName);
            }
        }

        [TestMethod]
        public void XMLTypesTest()
        {
            serializer.Serialize(assemblyMetadata, pathTarget);
            AssemblyMetadata assemblyMetadata2 = serializer.Deserialize<AssemblyMetadata>(pathTarget);
            for (int i = 0; i < assemblyMetadata.Namespaces.Count(); i++)
            {
                Assert.AreEqual(assemblyMetadata.Namespaces.ElementAt(i).Types.Count(),
                    assemblyMetadata2.Namespaces.ElementAt(i).Types.Count());
                Assert.AreEqual(assemblyMetadata.Namespaces.ElementAt(i).GetType(),
                    assemblyMetadata2.Namespaces.ElementAt(i).GetType());
            }
        }
    }
}
