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
        public string TypeName { get; set; }
        public string AssemblyName { get; set; }
        public bool IsGeneric { get; set; }
        public bool IsExternal { get; set; }
        public TypeMetadata BaseType { get; set; }
        public TypeMetadata DeclaringType { get; set; }
        public TypeKind TypeKind { get; set; }
        public List<TypeMetadata> GenericArguments { get; set; }
        public Tuple<AccessLevel, SealedEnum, AbstractEnum, StaticEnum> Modifiers { get; set; }
        public List<TypeMetadata> ImplementedInterfaces { get; set; }
        public List<TypeMetadata> NestedTypes { get; set; }
        public List<PropertyMetadata> Properties { get; set; }
        public List<ParameterMetadata> Fields { get; set; }
        public List<MethodMetadata> Methods { get; set; }
        public List<MethodMetadata> Constructors { get; set; }


        public TypeMetadata(Type type)
        {
            TypeName = type.Name;
            AssemblyName = type.AssemblyQualifiedName;
            IsGeneric = type.IsGenericParameter;
        }
        public void EmitOtherCharacteristics(Type type)
        {
            TypeKind = GetTypeKind(type);
            BaseType = EmitExtends(type.BaseType);
            Modifiers = EmitModifiers(type);
            IsExternal = false;
            DeclaringType = EmitDeclaringType(type.DeclaringType);
            Constructors = MethodMetadata.EmitConstructors(type);
            Methods = MethodMetadata.EmitMethods(type);
            NestedTypes = EmitNestedTypes(type);
            ImplementedInterfaces = EmitImplements(type.GetInterfaces()).ToList();
            GenericArguments = !type.IsGenericTypeDefinition ? null : EmitGenericArguments(type);
            Properties = PropertyMetadata.EmitProperties(type);
            Fields = EmitFields(type);
            isExamined = true;
        }

        public TypeMetadata() { }

        #region OperationWithDictionary
        public static TypeMetadata EmitReference(Type type)
        {
            if (!DictionarySingleton.Occurrence.ContainsKey(type.Name))
                DictionarySingleton.Occurrence.Add(type.Name, new TypeMetadata(type));
            return DictionarySingleton.Occurrence.Get(type.Name);
        }

        public static TypeMetadata EmitType(Type type)
        {
            if (!DictionarySingleton.Occurrence.ContainsKey(type.Name))
            {
                DictionarySingleton.Occurrence.Add(type.Name, new TypeMetadata(type));
            }
            if (!DictionarySingleton.Occurrence.Get(type.Name).isExamined)
            {
                DictionarySingleton.Occurrence.Get(type.Name).EmitOtherCharacteristics(type);
            }
            return DictionarySingleton.Occurrence.Get(type.Name);
        }
        #endregion

        public static List<TypeMetadata> EmitGenericArguments(Type type)
        {
            List<Type> arguments = type.GetGenericArguments().ToList();
            return arguments.Select(EmitReference).ToList();
        }

        #region Other emmits
        private TypeMetadata EmitDeclaringType(Type declaringType)
        {
            if (declaringType == null)
                return null;
            return EmitReference(declaringType);
        }
        private static List<ParameterMetadata> EmitFields(Type type)
        {
            List<FieldInfo> fieldInfo = type.GetFields(BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Public |
                BindingFlags.Static | BindingFlags.Instance).ToList();
            List<ParameterMetadata> parameters = new List<ParameterMetadata>();
            foreach (FieldInfo field in fieldInfo)
                parameters.Add(new ParameterMetadata(field.Name, EmitReference(field.FieldType)));
            return parameters;
        }

        private List<TypeMetadata> EmitNestedTypes(Type type)
        {
            List<Type> nestedTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic).ToList();
            return nestedTypes.Select(EmitType).ToList();
        }

        private IEnumerable<TypeMetadata> EmitImplements(IEnumerable<Type> interfaces)
        {
            return from @interface in interfaces select EmitReference(@interface);
        }
        private static TypeKind GetTypeKind(Type type)
        {
            return type.IsEnum ? TypeKind.Enum :
                   type.IsValueType ? TypeKind.Struct :
                   type.IsInterface ? TypeKind.Interface :
                   type.IsClass ? TypeKind.Class :
                   TypeKind.None;
        }
        private static Tuple<AccessLevel, SealedEnum, AbstractEnum, StaticEnum> EmitModifiers(Type type)
        {
            AccessLevel _access = AccessLevel.Private;
            AbstractEnum _abstract = AbstractEnum.NotAbstract;
            SealedEnum _sealed = SealedEnum.NotSealed;
            StaticEnum _static = StaticEnum.NotStatic;
            if (type.IsPublic)
                _access = AccessLevel.Public;
            else if (type.IsNestedPublic)
                _access = AccessLevel.Public;
            else if (type.IsNestedFamily)
                _access = AccessLevel.Protected;
            else if (type.IsNestedFamANDAssem)
                _access = AccessLevel.ProtectedInternal;
            if (type.IsSealed)
                _sealed = SealedEnum.Sealed;
            if (type.IsAbstract)
                _abstract = AbstractEnum.Abstract;
            if (type.IsSealed && type.IsAbstract)
                _static = StaticEnum.Static;
            return new Tuple<AccessLevel, SealedEnum, AbstractEnum, StaticEnum>(_access, _sealed, _abstract, _static);
        }
        private static TypeMetadata EmitExtends(Type baseType)
        {
            if (baseType == null || baseType == typeof(Object) || baseType == typeof(ValueType) || baseType == typeof(Enum))
                return null;
            return EmitReference(baseType);
        }
        private bool isExamined = false;
        #endregion

        public override string ToString()
        {
            string type = string.Empty;
            if (Modifiers != null)
            {
                type += Modifiers.Item1.ToString() + " ";
                type += Modifiers.Item2 == SealedEnum.Sealed ? SealedEnum.Sealed.ToString() + " " : string.Empty;
                type += Modifiers.Item3 == AbstractEnum.Abstract ? AbstractEnum.Abstract.ToString() + " " : string.Empty;
                type += Modifiers.Item4 == StaticEnum.Static ? StaticEnum.Static.ToString() + " " : string.Empty;

            }
            if (TypeKind != TypeKind.None)
                type += TypeKind.ToString() + " ";
            type += TypeName;
            if (IsGeneric)
                type += " - generic type";
            else if (IsExternal)
                type += " - external assembly: " + AssemblyName;
            return type;
        }
    }
}
