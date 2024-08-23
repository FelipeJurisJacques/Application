namespace Application.Source.Core.Storage.IndexedDb
{
    public class Transaction
    {
        private readonly bool _write;
        private readonly List<Storage> _storages;

        public Transaction(string name)
        {
            _write = false;
            _storages = [new Storage(name)];
        }

        public Transaction(string name, bool write)
        {
            _write = write;
            _storages = [new Storage(name)];
        }

        public Transaction(List<string> names)
        {
            _write = false;
            _storages = [];
            foreach (var name in names)
            {
                _storages.Add(new Storage(name));
            }
        }

        public Transaction(List<string> names, bool write)
        {
            _write = write;
            _storages = [];
            foreach (var name in names)
            {
                _storages.Add(new Storage(name));
            }
        }

        public List<Storage> Storages => _storages;

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