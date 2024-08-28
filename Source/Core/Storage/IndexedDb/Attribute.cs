namespace Application.Source.Core.Storage.IndexedDb
{
    public record class Attribute
    {
        public readonly string Name;
        public readonly bool Key;
        public readonly bool Unique;
        public readonly bool Indexable;
        public readonly bool MultiEntry;
        public readonly bool AutoIncrement;

        public static Attribute TypeKey(string name, bool autoIncrement)
        {
            return new Attribute(name, true, true, true, false, autoIncrement);
        }

        public static Attribute TypeIndex(string name)
        {
            return new Attribute(name, false, false, true, false, false);
        }

        public static Attribute TypeUnique(string name)
        {
            return new Attribute(name, false, true, true, false, false);
        }

        private Attribute(
            string name,
            bool key,
            bool unique,
            bool indexable,
            bool multiEntry,
            bool autoIncrement
        )
        {
            Name = name;
            Key = key;
            Unique = unique;
            Indexable = indexable;
            MultiEntry = multiEntry;
            AutoIncrement = autoIncrement;
        }
    }
}