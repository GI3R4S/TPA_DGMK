using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.IO;
using LoggerBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestFileLogger
{
    [TestClass]
    public class FileLoggingUnitTests
    {
        internal static string path;

        [Import(typeof(Logger))]
        public Logger Logger { get; set; }

        [TestInitialize]
        public void Initialize()
        {
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
            path = "./../../../UnitTestLogging/bin/Debug/AppLog.txt";
        }

        [TestMethod]
        public void CreateFileAndWrite()
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            Assert.IsFalse(File.Exists(path));
            Logger.Write(SeverityEnum.Information, "The program has been started");
            Assert.IsTrue(File.Exists(path));
        }
    }
}
