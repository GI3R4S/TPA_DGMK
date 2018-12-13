using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Model
{
    [DataContract(IsReference = true)]
    public class FieldMetadata
    {
        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public string Name { get; private set; }
        [DataMember]
        public TypeMetadata TypeMetadata { get; private set; }
        [DataMember]
        public ICollection<TypeMetadata> AttributesMetadata { get; private set; }
        [DataMember]
        public AccessLevel AccessLevel { get; private set; }
        [DataMember]
        public bool IsStatic { get; private set; }

        public FieldMetadata(FieldInfo field)
        {
            Id = ++counter;
            Name = field.Name;
            TypeMetadata = TypeMetadata.EmitReference(field.FieldType);
            AttributesMetadata = TypeMetadata.EmitAttributes(field.GetCustomAttributes());
            Tuple<AccessLevel, bool> modifiers = EmitModifiers(field);
            AccessLevel = modifiers.Item1;
            IsStatic = modifiers.Item2;
        }

        private FieldMetadata() { }
        private static int counter = 0;

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
