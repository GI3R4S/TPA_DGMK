﻿using System.Collections.Generic;

namespace Model
{
    public class ParameterMetadata
    {
        private string m_Name;
        private TypeMetadata m_TypeMetadata;
        private IEnumerable<TypeMetadata> m_Attributes;

        public string Name { get => m_Name; private set => m_Name = value; }
        public TypeMetadata TypeMetadata { get => m_TypeMetadata; private set => m_TypeMetadata = value; }
        public IEnumerable<TypeMetadata> Attributes { get => m_Attributes; private set => m_Attributes = value; }

        public ParameterMetadata(string name, TypeMetadata typeMetadata)
        {
            this.Name = name;
            this.TypeMetadata = typeMetadata;
        }
    }
}
