using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Mapping;
using BusinessLogic.Model;
using BusinessLogic.Reflection;
using Data;
using Data.DataMetadata;
using Data.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelXml;
using ModelXml.XmlMetadata;

namespace UnitTestSerializing
{
    [TestClass]
    public class SerializingUnitTests
    {
        private static string dllPath;
        private static string pathTarget;
        private static Reflector reflector;
        private static ISerializer serializer;
        private static AssemblyMetadataBase assemblyMetadataBase;
        private static AssemblyMetadata assemblyMetadata;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            dllPath = "./../../../DllForTests/ApplicationArchitecture/bin/Debug/TPA.ApplicationArchitecture.dll";
            pathTarget = "./../../../UnitTestXmlSerializing/bin/Debug/xmlTest.xml";
            serializer = new XMLSerializer();
            assemblyMetadataBase = new AssemblyMetadataXml();

            reflector = new Reflector(dllPath);
            serializer.Serialize(AssemblyMetadataMapper.MapToSerialize(reflector.AssemblyMetadata, assemblyMetadataBase.GetType()), pathTarget);
            assemblyMetadata = AssemblyMetadataMapper.MapToDeserialize(serializer.Deserialize(pathTarget));
        }

        [TestMethod]
        public void CheckingTheNumberOfNamespaces()
        {
            Assert.AreEqual(4, assemblyMetadata.Namespaces.Count);
        }

        [TestMethod]
        public void CheckingTheNumberOfClasses()
        {
            List<TypeMetadata> namespaceBusinessLogic = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.BusinessLogic").Types;
            List<TypeMetadata> namespaceData = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data").Types;
            List<TypeMetadata> namespaceDataCircularReference = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data.CircularReference").Types;
            List<TypeMetadata> namespacePresentation = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Presentation").Types;

            Assert.AreEqual(5, namespaceBusinessLogic.Count);
            Assert.AreEqual(12, namespaceData.Count);
            Assert.AreEqual(2, namespaceDataCircularReference.Count);
            Assert.AreEqual(1, namespacePresentation.Count);
        }

        [TestMethod]
        public void CheckingTheNumberOfStaticClasses()
        {
            List<TypeMetadata> staticClasses = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data").Types
                .Where(t => t.Modifiers.StaticEnum == StaticEnum.Static).ToList();
            Assert.AreEqual(1, staticClasses.Count());
        }

        [TestMethod]
        public void CheckingTheNumberOfAbstractClasses()
        {
            List<TypeMetadata> abstractClasses = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data").Types
                .Where(t => t.Modifiers.AbstractEnum == AbstractEnum.Abstract).ToList();
            Assert.AreEqual(3, abstractClasses.Count);
        }

        [TestMethod]
        public void CheckingTheNumberOfClassesWithGenericArguments()
        {
            List<TypeMetadata> genericClasses = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data").Types
                .Where(t => t.GenericArguments?.Count > 0).ToList();
            Assert.AreEqual(1, genericClasses.Count);
        }

        [TestMethod]
        public void CheckingTheNumberOfInterfaces()
        {
            List<TypeMetadata> interfaces = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data").Types
                .Where(t => t.TypeKind == TypeKind.Interface).ToList();
            Assert.AreEqual(1, interfaces.Count);
        }

        [TestMethod]
        public void CheckingTheNumberOfStructs()
        {
            List<TypeMetadata> structs = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data").Types
                .Where(t => t.TypeKind == TypeKind.Struct).ToList();
            Assert.AreEqual(1, structs.Count);
        }

        [TestMethod]
        public void CheckingTheNumberOfEnums()
        {
            List<TypeMetadata> enums = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data").Types
                .Where(t => t.TypeKind == TypeKind.Enum).ToList();
            Assert.AreEqual(1, enums.Count);
        }

        [TestMethod]
        public void CheckingTheNumberOfClassesWithBaseType()
        {
            List<TypeMetadata> classesWithBaseType = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data").Types
                .Where(t => t.BaseType != null).ToList();
            Assert.AreEqual(1, classesWithBaseType.Count);
        }

        [TestMethod]
        public void CheckingTheNumberOfPublicClasses()
        {
            List<TypeMetadata> publicClasses = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data").Types
                .Where(t => t.Modifiers.AccessLevel == AccessLevel.Public).ToList();
            Assert.AreEqual(9, publicClasses.Count);
        }

        [TestMethod]
        public void CheckingTheNumberOfClassesWithImplementedInterfaces()
        {
            List<TypeMetadata> classesWithImplementedInterfaces = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data").Types
                .Where(t => t.ImplementedInterfaces?.Count > 0).ToList();
            Assert.AreEqual(2, classesWithImplementedInterfaces.Count);
        }

        [TestMethod]
        public void CheckingTheNumberOfClassesWithNestedTypes()
        {
            List<TypeMetadata> classesWithNestedTypes = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data").Types
                .Where(t => t.NestedTypes?.Count > 0).ToList();
            Assert.AreEqual(1, classesWithNestedTypes.Count);
        }

        [TestMethod]
        public void CheckingTheNumberOfPropertiesInClass()
        {
            List<TypeMetadata> classes = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data").Types
                .Where(t => t.Modifiers.AccessLevel == AccessLevel.Public && t.TypeKind == TypeKind.Class).ToList();
            Assert.AreEqual(1, classes.ElementAt(0).Properties.Count);
        }

        [TestMethod]
        public void CheckingTheNumberOfMethodsInClass()
        {
            List<TypeMetadata> classes = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data").Types
                .Where(t => t.Modifiers.AccessLevel == AccessLevel.Public && t.TypeKind == TypeKind.Class).ToList();
            Assert.AreEqual(3, classes.ElementAt(0).Methods.Count);
        }

        [TestMethod]
        public void CheckingTheNumberOfConstructorsInClass()
        {
            List<TypeMetadata> classes = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data").Types
                .Where(t => t.Modifiers.AccessLevel == AccessLevel.Public && t.TypeKind == TypeKind.Class).ToList();
            Assert.AreEqual(1, classes.ElementAt(1).Constructors.Count);
        }

        [TestMethod]
        public void CheckingTheNumberOfFieldsInClass()
        {
            List<TypeMetadata> classes = assemblyMetadata.Namespaces
                .Find(t => t.NamespaceName == "TPA.ApplicationArchitecture.Data").Types
                .Where(t => t.Modifiers.AccessLevel == AccessLevel.Public && t.TypeKind == TypeKind.Class).ToList();
            Assert.AreEqual(1, classes.ElementAt(0).Fields.Count);
        }
    }
}