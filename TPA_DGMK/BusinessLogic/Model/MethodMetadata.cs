using Data.Enums;
using Data.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BusinessLogic.Model
{
    public class MethodMetadata
    {
        public string Name { get; set; }
        public bool Extension { get; set; }
        public TypeMetadata ReturnType { get; set; }
        public List<TypeMetadata> GenericArguments { get; set; }
        public List<ParameterMetadata> Parameters { get; set; }
        public MethodModifiers Modifiers { get; set; }

        private MethodMetadata(MethodBase method)
        {
            Name = method.Name;
            Extension = EmitExtension(method);
            ReturnType = EmitReturnType(method);
            GenericArguments = !method.IsGenericMethodDefinition ? null : EmitGenericArguments(method);
            Parameters = EmitParameters(method);
            Modifiers = EmitModifiers(method);
        }

        public MethodMetadata() { }

        public static List<MethodMetadata> EmitMethods(Type type)
        {
            return type.GetMethods(BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Public |
                        BindingFlags.Static | BindingFlags.Instance).Select(t => new MethodMetadata(t)).ToList();
        }
        public static List<MethodMetadata> EmitConstructors(Type type)
        {
            return type.GetConstructors().Select(t => new MethodMetadata(t)).ToList();
        }

        #region Other emmits
        private List<TypeMetadata> EmitGenericArguments(MethodBase method)
        {
            return method.GetGenericArguments().Select(TypeMetadata.EmitReference).ToList();
        }
        private static List<ParameterMetadata> EmitParameters(MethodBase method)
        {
            return method.GetParameters().Select(t => new ParameterMetadata(t.Name, TypeMetadata.EmitReference(t.ParameterType))).ToList();
        }
        private static TypeMetadata EmitReturnType(MethodBase method)
        {
            MethodInfo methodInfo = method as MethodInfo;
            if (methodInfo == null)
                return null;
            return TypeMetadata.EmitReference(methodInfo.ReturnType);
        }
        private static bool EmitExtension(MethodBase method)
        {
            return method.CustomAttributes.Where<CustomAttributeData>(x => x.AttributeType == typeof(ExtensionAttribute))
                .Count<CustomAttributeData>() == 1;
        }
        private static MethodModifiers EmitModifiers(MethodBase method)
        {
            AccessLevel _access = AccessLevel.Private;
            if (method.IsPublic)
                _access = AccessLevel.Public;
            else if (method.IsFamily)
                _access = AccessLevel.Protected;
            else if (method.IsFamilyAndAssembly)
                _access = AccessLevel.ProtectedInternal;
            AbstractEnum _abstract = AbstractEnum.NotAbstract;
            if (method.IsAbstract)
                _abstract = AbstractEnum.Abstract;
            StaticEnum _static = StaticEnum.NotStatic;
            if (method.IsStatic)
                _static = StaticEnum.Static;
            VirtualEnum _virtual = VirtualEnum.NotVirtual;
            if (method.IsVirtual)
                _virtual = VirtualEnum.Virtual;
            return new MethodModifiers()
            {
                AccessLevel = _access,
                AbstractEnum = _abstract,
                StaticEnum = _static,
                VirtualEnum = _virtual
            };
        }
        #endregion

        public override string ToString()
        {
            string type = string.Empty;
            if (Modifiers != null)
            {
                type += Modifiers.AccessLevel.ToString() + " ";
                type += Modifiers.AbstractEnum == AbstractEnum.Abstract ? AbstractEnum.Abstract.ToString() + " " : string.Empty;
                type += Modifiers.StaticEnum == StaticEnum.Static ? StaticEnum.Static.ToString() + " " : string.Empty;
                type += Modifiers.VirtualEnum == VirtualEnum.Virtual ? VirtualEnum.Virtual.ToString() + " " : string.Empty;
            }
            type += ReturnType != null ? ReturnType.TypeName + " " : string.Empty;
            type += Name;
            type += Extension ? " :Extension method" : string.Empty;
            return type;
        }
    }
}
