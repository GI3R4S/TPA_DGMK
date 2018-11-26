using System;
using System.Reflection;
using Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.Singleton;
using ViewModel;

namespace UnitTest
{
    [TestClass]
    public class ModelUnitTests
    {
        internal static Assembly assembly;
        internal static AssemblyMetadata assemblyModel;
        internal static Reflector reflector;

        //To get Type from outside the Model
        internal static SeverityEnum severityEnum;
        internal static Type type;

        [TestInitialize]
        public void Initialize()
        {
            reflector = new Reflector("./../../../Model/bin/Debug/netstandard2.0/Model.dll");
            assembly = reflector.Assembly;
            assemblyModel = reflector.AssemblyMetadata;

            severityEnum = SeverityEnum.Information;
            type = severityEnum.GetType();
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
