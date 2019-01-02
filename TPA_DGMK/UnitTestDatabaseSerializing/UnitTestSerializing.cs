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

namespace UnitTestDatabaseSerializing
{
    [TestClass]
    public class UnitTestSerializing
    {
        [TestClass]
        public class SerializerTest
        {
            private string databasePath;
            private string dllPath;
            private Reflector reflector;
            private AssemblyMetadata assemblyMetadata;
            private int namespaces;
            private List<NamespaceMetadata> namespacess;
            private int classes;
            private List<TypeMetadata> types;

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

                databasePath = "Data source=.;Initial catalog=Test;integrated security=true;persist security info=True;";
                dllPath = "./../../../UnitTestDatabaseSerializing/bin/Debug/Data.dll";
                reflector = new Reflector(dllPath);
                Service.ToList().FirstOrDefault()?.Serialize(reflector.AssemblyMetadata, databasePath);


                #region Fields needed to tests
                namespacess = reflector.AssemblyMetadata.Namespaces;
                namespaces = namespacess.Count;
                types = reflector.AssemblyMetadata.Namespaces.FirstOrDefault().Types;
                classes = types.Count;
                #endregion

                assemblyMetadata = Service.ToList().FirstOrDefault()?.Deserialize(databasePath);
            }
            [TestMethod]
            public void DbSerializerDeserializerTest()
            {
                Assert.AreEqual(namespaces, assemblyMetadata.Namespaces.Count);
                for (int i = 0; i < namespaces; i++)
                {
                    Assert.AreEqual(namespacess.ElementAt(i).NamespaceName,
                        assemblyMetadata.Namespaces.ElementAt(i).NamespaceName);
                }

                Assert.AreEqual(classes, assemblyMetadata.Namespaces.FirstOrDefault().Types.Count);
                for (int i = 0; i < classes; i++)
                {
                    Assert.AreEqual(types.ElementAt(i).AssemblyName,
                        assemblyMetadata.Namespaces.FirstOrDefault().Types.ElementAt(i).AssemblyName);
                    Assert.AreEqual(types.ElementAt(i).BaseType,
                        assemblyMetadata.Namespaces.FirstOrDefault().Types.ElementAt(i).BaseType);
                    Assert.AreEqual(types.ElementAt(i).DeclaringType,
                        assemblyMetadata.Namespaces.FirstOrDefault().Types.ElementAt(i).DeclaringType);
                    Assert.AreEqual(types.ElementAt(i).IsExternal,
                        assemblyMetadata.Namespaces.FirstOrDefault().Types.ElementAt(i).IsExternal);
                    Assert.AreEqual(types.ElementAt(i).IsGeneric,
                        assemblyMetadata.Namespaces.FirstOrDefault().Types.ElementAt(i).IsGeneric);
                    Assert.AreEqual(types.ElementAt(i).TypeName,
                        assemblyMetadata.Namespaces.FirstOrDefault().Types.ElementAt(i).TypeName);
                }
            }
        }
    }
}
