using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Model
{
    public class FieldMetadata
    {
        private string name;
        private TypeMetadata typeMetadata;
        private ICollection<TypeMetadata> attributesMetadata;

        public string Name { get => name; set => name = value; }
        public TypeMetadata TypeMetadata { get => typeMetadata; set => typeMetadata = value; }
        public ICollection<TypeMetadata> AttributesMetadata { get => attributesMetadata; set => attributesMetadata = value; }
        public AccessLevel AccessLevel { get; set; }
        public bool IsStatic { get; set; }

        public FieldMetadata(FieldInfo field)
        {
            Name = field.Name;
            TypeMetadata = TypeMetadata.EmitReference(field.FieldType);
            AttributesMetadata = Model.TypeMetadata.EmitAttributes(field.GetCustomAttributes());
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
