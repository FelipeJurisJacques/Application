namespace Application.Source.Core.Storage.IndexedDb
{
    public class Index
    {
        private readonly string _name;
        private readonly bool _option;
        private readonly IndexType _type;
        public Index(string name, IndexType type)
        {
            _name = name;
            _type = type;
            _option = false;
        }

        public Index(string name, IndexType type, bool uniqueOrAutoIncrement)
        {
            _name = name;
            _type = type;
            _option = uniqueOrAutoIncrement;
        }

        public string Name => _name;
        public bool Unique
        {
            get
            {
                return _type switch
                {
                    IndexType.INDEXABLE => _option,
                    IndexType.PRIMARY_KEY => true,
                    IndexType.MULTI_ENTRY_INDEXABLE => _option,
                    _ => false,
                };
            }
        }
        public bool Indexable => _type != IndexType.COMMON;
        public bool PrimaryKey => _type == IndexType.PRIMARY_KEY;
        public bool AutoIncrement => _type == IndexType.PRIMARY_KEY && _option;
        public bool MultiIndexable => _type == IndexType.MULTI_ENTRY_INDEXABLE;
    }
}