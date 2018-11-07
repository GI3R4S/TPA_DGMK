using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class TypeMetadata
    {
        private string m_typeName;
        private string m_NamespaceName;
        private string m_fullTypeName;
        private TypeMetadata m_BaseType;
        private IEnumerable<TypeMetadata> m_GenericArguments;
        private Tuple<AccessLevel, SealedEnum, AbstractEnum> m_Modifiers;
        private TypeKind m_TypeKind;
        private IEnumerable<TypeMetadata> m_Attributes;
        private IEnumerable<TypeMetadata> m_ImplementedInterfaces;
        private IEnumerable<TypeMetadata> m_NestedTypes;
        private IEnumerable<PropertyMetadata> m_Properties;
        private IEnumerable<FieldMetadata> m_Fields;
        private TypeMetadata m_DeclaringType;
        private IEnumerable<MethodMetadata> m_Methods;
        private IEnumerable<MethodMetadata> m_Constructors;

        public string TypeName { get => m_typeName; set => m_typeName = value; }
        public string NamespaceName { get => m_NamespaceName; set => m_NamespaceName = value; }
        public string FullTypeName { get => m_fullTypeName; set => m_fullTypeName = value; }
        public TypeMetadata BaseType { get => m_BaseType; set => m_BaseType = value; }
        public IEnumerable<TypeMetadata> GenericArguments { get => m_GenericArguments; set => m_GenericArguments = value; }
        internal Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers { get => m_Modifiers; set => m_Modifiers = value; }
        public TypeKind TypeKind1 { get => m_TypeKind; set => m_TypeKind = value; }
        public IEnumerable<TypeMetadata> Attributes { get => m_Attributes; set => m_Attributes = value; }
        public IEnumerable<TypeMetadata> ImplementedInterfaces { get => m_ImplementedInterfaces; set => m_ImplementedInterfaces = value; }
        public IEnumerable<TypeMetadata> NestedTypes { get => m_NestedTypes; set => m_NestedTypes = value; }
        public IEnumerable<PropertyMetadata> Properties { get => m_Properties; set => m_Properties = value; }
        public IEnumerable<FieldMetadata> Fields { get => m_Fields; set => m_Fields = value; }
        public TypeMetadata DeclaringType { get => m_DeclaringType; set => m_DeclaringType = value; }
        public IEnumerable<MethodMetadata> Methods { get => m_Methods; set => m_Methods = value; }
        public IEnumerable<MethodMetadata> Constructors { get => m_Constructors; set => m_Constructors = value; }
        public AccessLevel AccessLevel { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsSealed { get; set; }

        public static Dictionary<string, TypeMetadata> dictionary = new Dictionary<string, TypeMetadata>();

        internal TypeMetadata(Type type)
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
                dictionary[FullTypeName].TypeKind1 = GetTypeKind(type);
                dictionary[FullTypeName].Attributes = EmitAttributes(type.GetCustomAttributes(false).Cast<Attribute>());
            }

            m_DeclaringType = dictionary[FullTypeName].DeclaringType;
            m_Constructors = dictionary[FullTypeName].Constructors;
            m_Methods = dictionary[FullTypeName].Methods;
            m_NestedTypes = dictionary[FullTypeName].NestedTypes;
            m_ImplementedInterfaces = dictionary[FullTypeName].ImplementedInterfaces;
            m_GenericArguments = dictionary[FullTypeName].GenericArguments;
            AccessLevel = dictionary[FullTypeName].AccessLevel;
            IsSealed = dictionary[FullTypeName].IsSealed;
            IsAbstract = dictionary[FullTypeName].IsAbstract;
            m_BaseType = dictionary[FullTypeName].BaseType;
            m_Properties = dictionary[FullTypeName].Properties;
            m_Fields = dictionary[FullTypeName].Fields;
            m_TypeKind = dictionary[FullTypeName].TypeKind1;
            m_Attributes = dictionary[FullTypeName].Attributes;
        }
        internal static TypeMetadata EmitReference(Type type)
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
