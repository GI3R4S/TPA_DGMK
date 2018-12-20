using BusinessLogic.Model;
using BusinessLogic.Model.Singleton;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace UnitTestModel
{
    [TestClass]
    public class ModelUnitTests
    {
        internal static Assembly assembly;
        internal static AssemblyMetadata assemblyMetadata;

        //To get Type from outside the Model
        internal static Type type;

        [TestInitialize]
        public void Initialize()
        {
            assembly = Assembly.LoadFrom("./../../../UnitTestModel/bin/Debug/BusinessLogic.dll");
            assemblyMetadata = new AssemblyMetadata(assembly);

            type = typeof(System.RuntimeArgumentHandle);
        }

        #region DictionaryTypeMetadataTests
        [TestMethod]
        public void EnteredTheSameValueTest()
        {
            TypeMetadata tm = TypeMetadata.EmitReference(assembly.GetTypes()[0]);
            int referencesCount = DictionarySingleton.Occurrence.Count();

            TypeMetadata tm2 = TypeMetadata.EmitReference(assembly.GetTypes()[0]);
            Assert.AreEqual(referencesCount, DictionarySingleton.Occurrence.Count());
            Assert.AreEqual(DictionarySingleton.Occurrence.Get(tm.FullTypeName), DictionarySingleton.Occurrence.Get(tm2.FullTypeName));
        }

        [TestMethod]
        public void EnteredOtherValueTest()
        {
            int referencesCount = DictionarySingleton.Occurrence.Count();
            TypeMetadata typeMetadata = TypeMetadata.EmitReference(type);
            Assert.AreEqual(++referencesCount, DictionarySingleton.Occurrence.Count());
            Assert.AreEqual(typeMetadata, DictionarySingleton.Occurrence.Get(type.ToString()));
        }
        #endregion

        #region ExtensionMethodsTests
        [TestMethod]
        public void GetNamespaceTest()
        {
            Assert.AreEqual(type.Namespace, type.GetNamespace());
        }
        [TestMethod]
        public void GetVisibleTypeTest()
        {
            Assert.AreEqual(type.IsVisible, type.GetVisible());
        }
        [TestMethod]
        public void GetVisibleMethodBaseTest()
        {
            MethodBase m = MethodBase.GetCurrentMethod();
            Assert.AreEqual(m.IsPublic, m.GetVisible());
        }
        [TestMethod]
        public void GetVisibleFieldTest()
        {
            FieldInfo[] fi;
            Type t = typeof(FieldMetadata);
            fi = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.AreEqual(fi[0].IsPrivate, !fi[0].GetVisible());
        }
        #endregion
    }
}