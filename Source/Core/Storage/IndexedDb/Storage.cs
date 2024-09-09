namespace Application.Source.Core.Storage.IndexedDb
{
    public class Storage : IStorage
    {
        public readonly string _name;
        private readonly List<IField> _fields;
        private readonly Transaction? _transaction;

        public Storage(string name)
        {
            _name = name;
            _fields = [];
            _transaction = null;
        }

        public Storage(string name, List<IField> fields)
        {
            _name = name;
            _fields = fields;
            _transaction = null;
            var key = false;
            List<string> names = [];
            foreach (var field in fields)
            {
                if (field.Properties.Contains(FieldProperty.KEY))
                {
                    if (names.Contains(field.Name))
                    {
                        throw new InvalidOperationException(
                            "field " + field.Name + " is duplicated in storage " + name
                        );
                    }
                    names.Add(field.Name);
                    if (key)
                    {
                        throw new InvalidOperationException(
                            "primary key is duplicated in storage " + name
                        );
                    }
                    key = true;
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

        public List<IField> Fields => _fields;

        public void Add() { }
    }
}