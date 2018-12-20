using System.Collections.Generic;

namespace BusinessLogic.Model.Singleton
{
    public sealed class DictionarySingleton
    {
        private static DictionarySingleton occurrence = new DictionarySingleton();

        public static DictionarySingleton Occurrence
        {
            get { return occurrence; }
        }

        private Dictionary<string, TypeMetadata> dictionaryForTypes = new Dictionary<string, TypeMetadata>();
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
            TypeMetadata value;
            dictionaryForTypes.TryGetValue(key, out value);
            return value;
        }

        public int Count()
        {
            return dictionaryForTypes.Count;
        }
    }
}
