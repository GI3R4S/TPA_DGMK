using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Model
{
    public class FieldMetadata
    {
        public string Name { get; private set; }
        public TypeMetadata TypeMetadata { get; private set; }
        public ICollection<TypeMetadata> AttributesMetadata { get; private set; }

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
            {
                access = AccessLevel.IsPublic;
            }
            else if (field.IsFamily)
            {
                access = AccessLevel.IsProtected;
            }
            else if (field.IsFamilyAndAssembly)
            {
                access = AccessLevel.IsProtectedInternal;
            }

            return new Tuple<AccessLevel, bool>(access, field.IsStatic);
        }
    }
}
