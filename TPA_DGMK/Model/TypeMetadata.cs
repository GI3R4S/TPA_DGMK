using Model.Enums;
using Model.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Model
{
    [DataContract(IsReference = true)]
    public class TypeMetadata
    {
        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public string TypeName{get; private set;}
        [DataMember]
        public string NamespaceName{get; private set;}
        [DataMember]
        public string FullTypeName{get; private set;}
        [DataMember]
        public TypeMetadata BaseType{get; private set;}
        [DataMember]
        public ICollection<TypeMetadata> GenericArguments{get; private set;}
        [DataMember]
        public Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers{get; private set;}
        [DataMember]
        public TypeKind TypeKind{get; private set;}
        [DataMember]
        public ICollection<TypeMetadata> Attributes{get; private set;}
        [DataMember]
        public ICollection<TypeMetadata> ImplementedInterfaces{get; private set;}
        [DataMember]
        public ICollection<TypeMetadata> NestedTypes{get; private set;}
        [DataMember]
        public ICollection<PropertyMetadata> Properties{get; private set;}
        [DataMember]
        public ICollection<FieldMetadata> Fields{get; private set;}
        [DataMember]
        public TypeMetadata DeclaringType{get; private set;}
        [DataMember]
        public ICollection<MethodMetadata> Methods{get; private set;}
        [DataMember]
        public ICollection<MethodMetadata> Methods2{get; private set;}
        [DataMember]
        public ICollection<MethodMetadata> Constructors{get; private set;}
        [DataMember]
        public AccessLevel AccessLevel { get; private set; }
        [DataMember]
        public bool IsAbstract { get; private set; }
        [DataMember]
        public bool IsSealed { get; private set; }


        public TypeMetadata(Type type)
        {
            Id = ++counter;
            TypeName = type.Name;
            NamespaceName = type.Namespace;
            FullTypeName = type.ToString();
            if(!DictionarySingleton.Occurrence.ContainsKey(FullTypeName))
            {
                DictionarySingleton.Occurrence.Add(FullTypeName, this);
            }

            DeclaringType = EmitDeclaringType(type.DeclaringType);
            Constructors = MethodMetadata.EmitMethods(type.GetConstructors());
            Methods = MethodMetadata.EmitMethods(type.GetMethods());
            NestedTypes = EmitNestedTypes(type.GetNestedTypes());
            ImplementedInterfaces = EmitImplements(type.GetInterfaces());
            GenericArguments = !type.IsGenericTypeDefinition ? null : TypeMetadata.EmitGenericArguments(type.GetGenericArguments());
            Modifiers = EmitModifiers(type);
            BaseType = EmitExtends(type.BaseType);
            Properties = PropertyMetadata.EmitProperties(type.GetProperties());
            Fields = FieldMetadata.EmitFields(type.GetFields());
            TypeKind = GetTypeKind(type);
            Attributes = EmitAttributes(type.GetCustomAttributes(false).Cast<Attribute>());
        }

        private TypeMetadata() { }
        private static int counter = 0;

        public static TypeMetadata EmitReference(Type type)
        {
            string fullTypeName = type.ToString();
            if (DictionarySingleton.Occurrence.ContainsKey(fullTypeName))
                return DictionarySingleton.Occurrence.Get(fullTypeName);
            return new TypeMetadata(type);
        }
        internal static ICollection<TypeMetadata> EmitGenericArguments(ICollection<Type> arguments)
        {
            return (from Type _argument in arguments select EmitReference(_argument)).ToList();
        }
        private TypeMetadata EmitDeclaringType(Type declaringType)
        {
            if (declaringType == null)
                return null;
            return EmitReference(declaringType);
        }
        private ICollection<TypeMetadata> EmitNestedTypes(ICollection<Type> nestedTypes)
        {
            return (from _type in nestedTypes
                   where _type.GetVisible()
                   select new TypeMetadata(_type)).ToList();
        }
        private ICollection<TypeMetadata> EmitImplements(ICollection<Type> interfaces)
        {
            return (from currentInterface in interfaces
                   select EmitReference(currentInterface)).ToList();
        }
        private static TypeKind GetTypeKind(Type type)
        {
            return type.IsEnum ? TypeKind.EnumType :
                   type.IsValueType ? TypeKind.StructType :
                   type.IsInterface ? TypeKind.InterfaceType :
                   TypeKind.ClassType;
        }
        static Tuple<AccessLevel, SealedEnum, AbstractEnum> EmitModifiers(Type type)
        {
            AccessLevel _access = AccessLevel.IsPrivate;
            AbstractEnum _abstract = AbstractEnum.NotAbstract;
            SealedEnum _sealed = SealedEnum.NotSealed;
            if (type.IsPublic)
                _access = AccessLevel.IsPublic;
            else if (type.IsNestedPublic)
                _access = AccessLevel.IsPublic;
            else if (type.IsNestedFamily)
                _access = AccessLevel.IsProtected;
            else if (type.IsNestedFamANDAssem)
                _access = AccessLevel.IsProtectedInternal;
            if (type.IsSealed)
                _sealed = SealedEnum.Sealed;
            if (type.IsAbstract)
                _abstract = AbstractEnum.Abstract;
            return new Tuple<AccessLevel, SealedEnum, AbstractEnum>(_access, _sealed, _abstract);
        }
        private static TypeMetadata EmitExtends(Type baseType)
        {
            if (baseType == null || baseType == typeof(Object) || baseType == typeof(ValueType) || baseType == typeof(Enum))
                return null;
            return EmitReference(baseType);
        }
        internal static ICollection<TypeMetadata> EmitAttributes(IEnumerable<Attribute> attributes)
        {
            List<TypeMetadata> list = new List<TypeMetadata>();
            foreach (Attribute attribute in attributes)
            {
                string fullName = attribute.ToString();
                list.Add(EmitReference(attribute.GetType()));
            }
            return list;
        }
    }
}
