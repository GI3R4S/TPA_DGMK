using System;
using System.Reflection;
using Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace UnitTest
{
    [TestClass]
    public class ModelUnitTests
    {
        internal static Assembly assembly;
        internal static AssemblyMetadata assemblyModel;

        //To get Type from outside the Model
        internal static SeverityEnum severityEnum;
        internal static Type t;

        [TestInitialize]
        public void Initialize()
        {
            assembly = Assembly.LoadFrom("./../../../BusinessLogic/bin/Debug/BusinessLogic.dll");
            assemblyModel = new AssemblyMetadata(assembly);

            severityEnum = SeverityEnum.Information;
            t = severityEnum.GetType();
        }

        #region DictionaryTypeMetadataTests
        [TestMethod]
        public void EnteredTheSameValue()
        {
            TypeMetadata tm = TypeMetadata.EmitReference(assembly.GetTypes()[0]);
            int referencesCount = TypeMetadata.dictionary.Count;

            TypeMetadata tm2 = TypeMetadata.EmitReference(assembly.GetTypes()[0]);
            Assert.AreEqual(referencesCount, TypeMetadata.dictionary.Count);
            Assert.AreEqual(TypeMetadata.dictionary[tm.FullTypeName], TypeMetadata.dictionary[tm2.FullTypeName]);
        }

        [TestMethod]
        public void EnteredOtherValue()
        {
            int referencesCount = TypeMetadata.dictionary.Count;
            TypeMetadata tm = TypeMetadata.EmitReference(t);
            Assert.AreEqual(++referencesCount, TypeMetadata.dictionary.Count);
            Assert.AreEqual(tm, TypeMetadata.dictionary[t.ToString()]);
        }
        #endregion

        #region ExtensionMethodsTests
        [TestMethod]
        public void GetNamespaceTest()
        {
            Assert.AreEqual(t.Namespace, t.GetNamespace());
        }
        [TestMethod]
        public void GetVisibleTypeTest()
        {
            Assert.AreEqual(t.IsVisible, t.GetVisible());
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
