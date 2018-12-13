using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Model
{
    [DataContract(IsReference = true)]
    public class MethodMetadata
    {
        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public string Name{get; private set;}
        [DataMember]
        public ICollection<TypeMetadata> GenericArguments{get; private set;}
        [DataMember]
        public Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> Modifiers{get; private set;}
        [DataMember]
        public TypeMetadata ReturnType{get; private set;}
        [DataMember]
        public bool Extension{get; private set;}
        [DataMember]
        public ICollection<ParameterMetadata> Parameters{get; private set;}
        [DataMember]
        public ICollection<TypeMetadata> AttributesMetadata{get; private set;}
        [DataMember]
        public TypeMetadata ReflectedType{get; private set;}

        private MethodMetadata(MethodBase method)
        {
            Id = ++counter;
            Name = method.Name;
            GenericArguments = !method.IsGenericMethodDefinition ? null : TypeMetadata.EmitGenericArguments(method.GetGenericArguments());
            ReturnType = EmitReturnType(method);
            Parameters = EmitParameters(method.GetParameters());
            Modifiers = EmitModifiers(method);
            Extension = EmitExtension(method);
            AttributesMetadata = TypeMetadata.EmitAttributes(method.GetCustomAttributes());
            ReflectedType = TypeMetadata.EmitReference(method.ReflectedType);
        }

        private MethodMetadata() { }
        private static int counter = 0;

        internal static ICollection<MethodMetadata> EmitMethods(ICollection<MethodBase> methods)
        {
            return (from MethodBase _currentMethod in methods
                   where _currentMethod.GetVisible()
                   select new MethodMetadata(_currentMethod)).ToList();
        }
        private static ICollection<ParameterMetadata> EmitParameters(ICollection<ParameterInfo> parms)
        {
            return (from parm in parms
                   select new ParameterMetadata(parm.Name, TypeMetadata.EmitReference(parm.ParameterType))).ToList();
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
            return method.IsDefined(typeof(ExtensionAttribute), true);
        }
        private static Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> EmitModifiers(MethodBase method)
        {
            AccessLevel _access = AccessLevel.IsPrivate;
            if (method.IsPublic)
                _access = AccessLevel.IsPublic;
            else if (method.IsFamily)
                _access = AccessLevel.IsProtected;
            else if (method.IsFamilyAndAssembly)
                _access = AccessLevel.IsProtectedInternal;
            AbstractEnum _abstract = AbstractEnum.NotAbstract;
            if (method.IsAbstract)
                _abstract = AbstractEnum.Abstract;
            StaticEnum _static = StaticEnum.NotStatic;
            if (method.IsStatic)
                _static = StaticEnum.Static;
            VirtualEnum _virtual = VirtualEnum.NotVirtual;
            if (method.IsVirtual)
                _virtual = VirtualEnum.Virtual;
            return new Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum>(_access, _abstract, _static, _virtual);
        }
    }
}
