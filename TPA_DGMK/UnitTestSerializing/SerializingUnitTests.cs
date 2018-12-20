using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.IO;
using System.Linq;
using BusinessLogic;
using BusinessLogic.Model;
using BusinessLogic.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestSerializing
{
    [TestClass]
    public class SerializingUnitTests
    {
        private string path;
        private string path2;
        private string pathTarget;
        private Reflector reflector;

        [ImportMany(typeof(LogicService))]
        public IEnumerable<LogicService> Service { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            #region MEF
            NameValueCollection paths = (NameValueCollection)ConfigurationManager.GetSection("paths");
            string[] pathsCatalogs = paths.AllKeys;
            List<DirectoryCatalog> directoryCatalogs = new List<DirectoryCatalog>();
            foreach (string pathsCatalog in pathsCatalogs)
            {
                if (Directory.Exists(pathsCatalog))
                    directoryCatalogs.Add(new DirectoryCatalog(pathsCatalog));
            }

            AggregateCatalog catalog = new AggregateCatalog(directoryCatalogs);
            CompositionContainer container = new CompositionContainer(catalog);
            container.ComposeParts(this);
            #endregion

            path = "./../../../UnitTestSerializing/bin/Debug/BusinessLogic.dll";
            path2 = "./../../../UnitTestSerializing/bin/Debug/Data.dll";
            pathTarget = "./../../../UnitTestSerializing/bin/Debug/xmlTest.xml";
            reflector = new Reflector(path);
        }

        [TestMethod]
        public void XMLSerializerSerializeTest()
        {
            if (File.Exists(pathTarget))
            {
                File.Delete(pathTarget);
            }
            Assert.IsFalse(File.Exists(pathTarget));
            Service.ToList().FirstOrDefault()?.Serialize(reflector.AssemblyMetadata, pathTarget);
            Assert.IsTrue(File.Exists(pathTarget));
        }

        [TestMethod]
        public void XMLSerializerDeserializeTest()
        {
            Assert.AreEqual("BusinessLogic.dll", reflector.AssemblyMetadata.Name);
            Service.ToList().FirstOrDefault()?.Serialize(reflector.AssemblyMetadata, pathTarget);
            reflector = new Reflector(path2);
            Assert.AreEqual("Data.dll", reflector.AssemblyMetadata.Name);
            AssemblyMetadata assemblyMetadata2 = Service.ToList().FirstOrDefault()?.Deserialize(pathTarget);
            reflector = new Reflector(assemblyMetadata2);
            Assert.AreEqual(assemblyMetadata2.Name, reflector.AssemblyMetadata.Name);
        }

        [TestMethod]
        public void XMLNamespacesTest()
        {
            Service.ToList().FirstOrDefault()?.Serialize(reflector.AssemblyMetadata, pathTarget);
            AssemblyMetadata assemblyMetadata2 = Service.ToList().FirstOrDefault()?.Deserialize(pathTarget);
            Assert.AreEqual(reflector.AssemblyMetadata.Namespaces.Count(), assemblyMetadata2.Namespaces.Count());
            for (int i = 0; i < reflector.AssemblyMetadata.Namespaces.Count(); i++)
            {
                Assert.AreEqual(reflector.AssemblyMetadata.Namespaces.ElementAt(i).NamespaceName,
                    assemblyMetadata2.Namespaces.ElementAt(i).NamespaceName);
            }
        }

        [TestMethod]
        public void XMLTypesTest()
        {
            Service.ToList().FirstOrDefault()?.Serialize(reflector.AssemblyMetadata, pathTarget);
            AssemblyMetadata assemblyMetadata2 = Service.ToList().FirstOrDefault()?.Deserialize(pathTarget);
            for (int i = 0; i < reflector.AssemblyMetadata.Namespaces.Count(); i++)
            {
                Assert.AreEqual(reflector.AssemblyMetadata.Namespaces.ElementAt(i).Types.Count(),
                    assemblyMetadata2.Namespaces.ElementAt(i).Types.Count());
                Assert.AreEqual(reflector.AssemblyMetadata.Namespaces.ElementAt(i).GetType(),
                    assemblyMetadata2.Namespaces.ElementAt(i).GetType());
            }
        }
    }
}