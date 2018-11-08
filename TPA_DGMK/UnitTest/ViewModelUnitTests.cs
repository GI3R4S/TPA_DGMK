using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using ViewModel;

namespace UnitTest
{
    [TestClass]
    public class ViewModelUnitTests
    {
        internal static Assembly assembly;
        internal static AssemblyViewModel assemblyViewModel;
        internal static Logger logger;

        [TestInitialize]
        public void Initialize()
        {
            assembly = Assembly.LoadFrom("./../../../BusinessLogic/bin/Debug/BusinessLogic.dll");
            assemblyViewModel = new AssemblyViewModel(new AssemblyMetadata(assembly), logger);
        }

        #region Expandable
        [TestMethod]
        public void IsExpandableTest()
        {
            if (assemblyViewModel.Children.Count == 0 || assemblyViewModel.Children == null)
            {
                Assert.AreEqual(false, assemblyViewModel.IsExpandable());
                return;
            }

            Assert.AreEqual(assembly.GetTypes().Any(), assemblyViewModel.IsExpandable());
            Type[] t = assembly.GetTypes();
            TypeMetadata tm = new TypeMetadata(t[5]);
            TypeViewModel tvm = new TypeViewModel(tm, logger);
            MethodMetadata mm = tm.Constructors.First();
            Assert.AreEqual((mm.Parameters != null && mm.AttributesMetadata != null), tvm.IsExpandable());
        }

        [TestMethod]
        public void IsExpandedTest()
        {
            if (assemblyViewModel.IsExpandable())
            {
                Assert.IsNotNull(assemblyViewModel.Children);
                Assert.AreNotEqual(0, assemblyViewModel.Children.Count);
            }

            Assert.AreEqual(assemblyViewModel.Children.Count != 0, assemblyViewModel.IsExpandable());
            Assert.AreEqual(assemblyViewModel.Children != null, assemblyViewModel.IsExpandable());
        }
        #endregion

        #region ExtensionMethods
        [TestMethod]
        public void CheckIfItIsNullOrEmptyTest()
        {
            IEnumerable<NamespaceMetadata> namespaces = null;
            Assert.AreEqual(namespaces == null, namespaces.CheckIfItIsNullOrEmpty());

            AssemblyMetadata assemblyMetadata = new AssemblyMetadata(assembly);
            Assert.AreEqual(!assemblyMetadata.Namespaces.Any(), assemblyMetadata.Namespaces.CheckIfItIsNullOrEmpty());
        }

        [TestMethod]
        public void ReturnEmptyIfItIsNullTest()
        {
            IEnumerable<NamespaceMetadata> namespaces = null;
            Assert.AreEqual(namespaces.ReturnEmptyIfItIsNull(), Enumerable.Empty<NamespaceMetadata>());
        }
        #endregion
    }
}
