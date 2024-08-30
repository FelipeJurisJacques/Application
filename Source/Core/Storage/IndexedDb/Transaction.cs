namespace Application.Source.Core.Storage.IndexedDb
{
    public class Transaction
    {
        private readonly bool _write;
        private readonly Connection _connection;
        private readonly List<Storage> _storages;

        public Transaction(Connection connection, List<string> names, bool write)
        {
            if (names.Count == 0) {
                throw new InvalidOperationException("storage name list is empty");
            }
            _write = write;
            _storages = [];
            _connection = connection;
            foreach (var name in names)
            {
                _storages.Add(new Storage(name));
            }
        }

        public List<Storage> Storages => _storages;

        public Connection Connection => _connection;

        public void Abort() { }

        public void Commit() { }

        public Storage GetStorage(string name)
        {
            foreach (var storage in _storages)
            {
                if (name == storage.Name)
                {
                    return storage;
                }
            }
            throw new ArgumentOutOfRangeException(
                "undefined storage " + name
            );
        }
    }
}