using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Model
{
    public class FieldMetadata
    {
        private string m_Name;
        private TypeMetadata m_TypeMetadata;
        private ICollection<TypeMetadata> m_AttributesMetadata;

        public string Name { get => m_Name; private set => m_Name = value; }
        public TypeMetadata TypeMetadata { get => m_TypeMetadata; private set => m_TypeMetadata = value; }
        public ICollection<TypeMetadata> AttributesMetadata { get => m_AttributesMetadata; private set => m_AttributesMetadata = value; }
        public AccessLevel AccessLevel { get; private set; }
        public bool IsStatic { get; private set; }

        public FieldMetadata(FieldInfo field)
        {
            Name = field.Name;
            TypeMetadata = TypeMetadata.EmitReference(field.FieldType);
            AttributesMetadata = TypeMetadata.EmitAttributes(field.GetCustomAttributes());
            Tuple<AccessLevel, bool> modifiers = EmitModifiers(field);
            AccessLevel = modifiers.Item1;
            IsStatic = modifiers.Item2;
        }

        internal static ICollection<FieldMetadata> EmitFields(IEnumerable<FieldInfo> fields)
        {
            return (from field in fields
                    where field.GetVisible()
                    select new FieldMetadata(field)).ToList();
        }

        private static Tuple<AccessLevel, bool> EmitModifiers(FieldInfo field)
        {
            AccessLevel access = AccessLevel.IsPrivate;
            if (field.IsPublic)
                access = AccessLevel.IsPublic;
            else if (field.IsFamily)
                access = AccessLevel.IsProtected;
            else if (field.IsFamilyAndAssembly)
                access = AccessLevel.IsProtectedInternal;
            return new Tuple<AccessLevel, bool>(access, field.IsStatic );
        }
    }
}
