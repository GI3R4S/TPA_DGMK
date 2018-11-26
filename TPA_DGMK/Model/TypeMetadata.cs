using Model.Enums;
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
        public string TypeName{get; private set;}
        [DataMember]
        public string NamespaceName{get; private set;}
        [DataMember]
        public string FullTypeName{get; private set;}
        [DataMember]
        public TypeMetadata BaseType{get; private set;}
        [DataMember]
        public IEnumerable<TypeMetadata> GenericArguments{get; private set;}
        [DataMember]
        public Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers{get; private set;}
        [DataMember]
        public TypeKind TypeKind{get; private set;}
        [DataMember]
        public IEnumerable<TypeMetadata> Attributes{get; private set;}
        [DataMember]
        public IEnumerable<TypeMetadata> ImplementedInterfaces{get; private set;}
        [DataMember]
        public IEnumerable<TypeMetadata> NestedTypes{get; private set;}
        [DataMember]
        public IEnumerable<PropertyMetadata> Properties{get; private set;}
        [DataMember]
        public IEnumerable<FieldMetadata> Fields{get; private set;}
        [DataMember]
        public TypeMetadata DeclaringType{get; private set;}
        [DataMember]
        public IEnumerable<MethodMetadata> Methods{get; private set;}
        [DataMember]
        public IEnumerable<MethodMetadata> Constructors{get; private set;}
        [DataMember]
        public AccessLevel AccessLevel { get; private set; }
        [DataMember]
        public bool IsAbstract { get; private set; }
        [DataMember]
        public bool IsSealed { get; private set; }

        public static Dictionary<string, TypeMetadata> dictionary = new Dictionary<string, TypeMetadata>();

        public TypeMetadata(Type type)
        {
            TypeName = type.Name;
            NamespaceName = type.Namespace;
            FullTypeName = type.ToString();
            if (!dictionary.ContainsKey(FullTypeName))
            {
                dictionary.Add(FullTypeName, this);
                dictionary[FullTypeName].DeclaringType = EmitDeclaringType(type.DeclaringType);
                dictionary[FullTypeName].Constructors = MethodMetadata.EmitMethods(type.GetConstructors());
                dictionary[FullTypeName].Methods = MethodMetadata.EmitMethods(type.GetMethods());
                dictionary[FullTypeName].NestedTypes = EmitNestedTypes(type.GetNestedTypes());
                dictionary[FullTypeName].ImplementedInterfaces = EmitImplements(type.GetInterfaces());
                dictionary[FullTypeName].GenericArguments = !type.IsGenericTypeDefinition ? null : TypeMetadata.EmitGenericArguments(type.GetGenericArguments());
                dictionary[FullTypeName].Modifiers = EmitModifiers(type);
                dictionary[FullTypeName].BaseType = EmitExtends(type.BaseType);
                dictionary[FullTypeName].Properties = PropertyMetadata.EmitProperties(type.GetProperties());
                dictionary[FullTypeName].Fields = FieldMetadata.EmitFields(type.GetFields());
                dictionary[FullTypeName].TypeKind = GetTypeKind(type);
                dictionary[FullTypeName].Attributes = EmitAttributes(type.GetCustomAttributes(false).Cast<Attribute>());
            }

            DeclaringType = dictionary[FullTypeName].DeclaringType;
            Constructors = dictionary[FullTypeName].Constructors;
            Methods = dictionary[FullTypeName].Methods;
            NestedTypes = dictionary[FullTypeName].NestedTypes;
            ImplementedInterfaces = dictionary[FullTypeName].ImplementedInterfaces;
            GenericArguments = dictionary[FullTypeName].GenericArguments;
            AccessLevel = dictionary[FullTypeName].AccessLevel;
            IsSealed = dictionary[FullTypeName].IsSealed;
            IsAbstract = dictionary[FullTypeName].IsAbstract;
            BaseType = dictionary[FullTypeName].BaseType;
            Properties = dictionary[FullTypeName].Properties;
            Fields = dictionary[FullTypeName].Fields;
            TypeKind = dictionary[FullTypeName].TypeKind;
            Attributes = dictionary[FullTypeName].Attributes;
        }

        private TypeMetadata() { }

        public static TypeMetadata EmitReference(Type type)
        {
            string fullTypeName = type.ToString();
            if (dictionary.ContainsKey(fullTypeName))
                return dictionary[fullTypeName];
            return new TypeMetadata(type);
        }
        internal static IEnumerable<TypeMetadata> EmitGenericArguments(IEnumerable<Type> arguments)
        {
            return (from Type _argument in arguments select EmitReference(_argument)).ToList();
        }
        private TypeMetadata EmitDeclaringType(Type declaringType)
        {
            if (declaringType == null)
                return null;
            return EmitReference(declaringType);
        }
        private IEnumerable<TypeMetadata> EmitNestedTypes(IEnumerable<Type> nestedTypes)
        {
            return (from _type in nestedTypes
                   where _type.GetVisible()
                   select new TypeMetadata(_type)).ToList();
        }
        private IEnumerable<TypeMetadata> EmitImplements(IEnumerable<Type> interfaces)
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
