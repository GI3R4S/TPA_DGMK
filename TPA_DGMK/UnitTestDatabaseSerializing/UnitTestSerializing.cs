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
            #region Private fields
            private string databasePath;
            private string dllPath;
            private Reflector reflector;
            private AssemblyMetadata assemblyMetadata;
            private int namespacesCount;
            private List<NamespaceMetadata> namespaces;
            private int classesCount;
            private List<TypeMetadata> classes;
            #endregion

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
                dllPath = "./../../../DllForTests/ApplicationArchitecture/bin/Debug/TPA.ApplicationArchitecture.dll";
                reflector = new Reflector(dllPath);
                Service.ToList().FirstOrDefault()?.Serialize(reflector.AssemblyMetadata, databasePath);

                #region Fields neccessary to tests
                namespaces = reflector.AssemblyMetadata.Namespaces;
                namespacesCount = namespaces.Count;
                classes = reflector.AssemblyMetadata.Namespaces.FirstOrDefault().Types;
                classesCount = classes.Count;
                #endregion

                assemblyMetadata = Service.ToList().FirstOrDefault()?.Deserialize(databasePath);
            }
            [TestMethod]
            public void DbSerializerDeserializerTest()
            {
                Assert.AreEqual(namespacesCount, assemblyMetadata.Namespaces.Count);
                for (int i = 0; i < namespacesCount; i++)
                {
                    Assert.AreEqual(namespaces.ElementAt(i).NamespaceName,
                        assemblyMetadata.Namespaces.ElementAt(i).NamespaceName);
                }

                Assert.AreEqual(classesCount, assemblyMetadata.Namespaces.FirstOrDefault().Types.Count);
                for (int i = 0; i < classesCount; i++)
                {
                    Assert.AreEqual(classes.ElementAt(i).AssemblyName,
                        assemblyMetadata.Namespaces.FirstOrDefault().Types.ElementAt(i).AssemblyName);
                    Assert.AreEqual(classes.ElementAt(i).BaseType,
                        assemblyMetadata.Namespaces.FirstOrDefault().Types.ElementAt(i).BaseType);
                    Assert.AreEqual(classes.ElementAt(i).DeclaringType,
                        assemblyMetadata.Namespaces.FirstOrDefault().Types.ElementAt(i).DeclaringType);
                    Assert.AreEqual(classes.ElementAt(i).IsExternal,
                        assemblyMetadata.Namespaces.FirstOrDefault().Types.ElementAt(i).IsExternal);
                    Assert.AreEqual(classes.ElementAt(i).IsGeneric,
                        assemblyMetadata.Namespaces.FirstOrDefault().Types.ElementAt(i).IsGeneric);
                    Assert.AreEqual(classes.ElementAt(i).TypeName,
                        assemblyMetadata.Namespaces.FirstOrDefault().Types.ElementAt(i).TypeName);
                }
            }
        }
    }
}
