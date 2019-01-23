using System.IO;
using LoggerBase;
using LoggerToFile;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestFileLogger
{
    [TestClass]
    public class FileLoggingUnitTests
    {
        private static string path;
        private static Logger logger;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            logger = new FileLogger();
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
            logger.Write(SeverityEnum.Information, "The program has been started");
            Assert.IsTrue(File.Exists(path));
        }
    }
}
