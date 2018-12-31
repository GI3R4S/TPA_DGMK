using System.Collections.Generic;

namespace BusinessLogic.Model.Singleton
{
    public sealed class DictionarySingleton
    {
        public static DictionarySingleton Occurrence { get; } = new DictionarySingleton();

        private readonly Dictionary<string, TypeMetadata> dictionaryForTypes = new Dictionary<string, TypeMetadata>();
        private DictionarySingleton()
        {
        }

        public void Add(string name, TypeMetadata type)
        {
            dictionaryForTypes.Add(name, type);
        }

        public bool ContainsKey(string name)
        {
            return dictionaryForTypes.ContainsKey(name);
        }

        public TypeMetadata Get(string key)
        {
            dictionaryForTypes.TryGetValue(key, out TypeMetadata value);
            return value;
        }

        public int Count()
        {
            return dictionaryForTypes.Count;
        }
    }
}
