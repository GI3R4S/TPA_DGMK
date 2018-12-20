using BusinessLogic.Model.Singleton;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BusinessLogic.Model
{
    public class TypeMetadata
    {
        public string TypeName{get; set;}
        public string NamespaceName{get; set;}
        public string FullTypeName{get; set;}
        public TypeMetadata BaseType{get; set;}
        public List<TypeMetadata> GenericArguments{get; set;}
        public Tuple<AccessLevel, SealedEnum, AbstractEnum> Modifiers{get; set;}
        public TypeKind TypeKind{get; set;}
        public List<TypeMetadata> ImplementedInterfaces{get; set;}
        public List<TypeMetadata> NestedTypes{get; set;}
        public List<PropertyMetadata> Properties{get; set;}
        public List<FieldMetadata> Fields{get; set;}
        public TypeMetadata DeclaringType{get; set;}
        public List<MethodMetadata> Methods{get; set;}
        public List<MethodMetadata> Constructors{get; set;}
        public AccessLevel AccessLevel { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsSealed { get; set; }


        public TypeMetadata(Type type)
        {
            TypeName = type.Name;
            NamespaceName = type.Namespace;
            FullTypeName = type.ToString();
            if(!DictionarySingleton.Occurrence.ContainsKey(FullTypeName))
            {
                DictionarySingleton.Occurrence.Add(FullTypeName, this);
            }

            DeclaringType = EmitDeclaringType(type.DeclaringType);
            Constructors = MethodMetadata.EmitMethods(type);
            Methods = MethodMetadata.EmitMethods(type);
            NestedTypes = EmitNestedTypes(type);
            ImplementedInterfaces = EmitImplements(type);
            GenericArguments = !type.IsGenericTypeDefinition ? null : EmitGenericArguments(type);
            Modifiers = EmitModifiers(type);
            BaseType = EmitExtends(type.BaseType);
            Properties = PropertyMetadata.EmitProperties(type);
            Fields = FieldMetadata.EmitFields(type.GetFields());
            TypeKind = GetTypeKind(type);
        }

        public TypeMetadata() { }

        public static TypeMetadata EmitReference(Type type)
        {
            string fullTypeName = type.ToString();
            if (DictionarySingleton.Occurrence.ContainsKey(fullTypeName))
                return DictionarySingleton.Occurrence.Get(fullTypeName);
            return new TypeMetadata(type);
        }
        public static List<TypeMetadata> EmitGenericArguments(Type type)
        {
            List<Type> arguments = type.GetGenericArguments().ToList();
            return arguments.Select(EmitReference).ToList();
        }
        TypeMetadata EmitDeclaringType(Type declaringType)
        {
            if (declaringType == null)
                return null;
            return EmitReference(declaringType);
        }
        List<TypeMetadata> EmitNestedTypes(Type type)
        {
            List<Type> nestedTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic).ToList();
            return (from _type in nestedTypes
                   where _type.GetVisible()
                   select new TypeMetadata(_type)).ToList();
        }
        List<TypeMetadata> EmitImplements(Type type)
        {
            List<Type> interfaces = type.GetInterfaces().ToList();
            return (from currentInterface in interfaces
                   select EmitReference(currentInterface)).ToList();
        }
        static TypeKind GetTypeKind(Type type)
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
        static TypeMetadata EmitExtends(Type baseType)
        {
            if (baseType == null || baseType == typeof(Object) || baseType == typeof(ValueType) || baseType == typeof(Enum))
                return null;
            return EmitReference(baseType);
        }
        internal static List<TypeMetadata> EmitAttributes(IEnumerable<Attribute> attributes)
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
