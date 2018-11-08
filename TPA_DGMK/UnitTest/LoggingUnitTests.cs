﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logging;
using System.IO;

namespace UnitTest
{
    [TestClass]
    public class LoggingUnitTests
    {
        internal static Logger logger;
        internal static string path;

        [TestInitialize]
        public void Initialize()
        {
            logger = new FileLogger();
            path = "./../../../UnitTest/bin/Debug/AppLog.txt";
        }

        [TestMethod]
        public void LoggerWriteTest()
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