namespace Application.Source.Core.Storage.IndexedDb
{
    public record class Field : IField
    {
        public readonly bool Key;
        public readonly bool Unique;
        private readonly string _name;
        public readonly bool Indexable;
        public readonly bool MultiEntry;
        public readonly bool AutoIncrement;

        public static Field TypeKey(string name, bool autoIncrement)
        {
            return new Field(name, true, true, true, false, autoIncrement);
        }

        public static Field TypeIndex(string name)
        {
            return new Field(name, false, false, true, false, false);
        }

        public static Field TypeUnique(string name)
        {
            return new Field(name, false, true, true, false, false);
        }

        private Field(
            string name,
            bool key,
            bool unique,
            bool indexable,
            bool multiEntry,
            bool autoIncrement
        )
        {
            _name = name;
            Key = key;
            Unique = unique;
            Indexable = indexable;
            MultiEntry = multiEntry;
            AutoIncrement = autoIncrement;
        }

        public string Name => _name;
    }
}