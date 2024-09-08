namespace Application.Source.Core.Storage.IndexedDb
{
    public class Storage : IStorage
    {
        public readonly string _name;
        private readonly Field? _key;
        private readonly List<IField> _fields;
        private readonly Transaction? _transaction;

        public Storage(string name)
        {
            _key = null;
            _name = name;
            _fields = [];
            _transaction = null;
        }

        public Storage(string name, List<Field> fields)
        {
            _key = null;
            _name = name;
            _fields = [];
            _transaction = null;
            foreach (var field in fields)
            {
                if (field.Key)
                {
                    if (_key == null)
                    {
                        _key = field;
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            "primary key is duplicated in storage " + _name
                        );
                    }
                }
                else
                {
                    _fields.Add(field);
                }
            }
        }

        internal Storage(Transaction transaction, string name)
        {
            _name = name;
            _fields = [];
            _transaction = transaction;
        }

        public string Name => _name;

        public Field? Key => _key;

        public List<IField> Fields => _fields;

        public void Add() { }
    }
}