using Microsoft.JSInterop;

namespace Application.Source.Core.Storage.IndexedDb
{
    public class Transaction
    {
        public readonly bool Write;
        public readonly Connection Connection;
        internal IJSObjectReference? Reference;
        private readonly List<Storage> _storages;

        internal Transaction(Connection connection, List<string> names, bool write)
        {
            if (names.Count == 0)
            {
                throw new InvalidOperationException("storage name list is empty");
            }
            Write = write;
            _storages = [];
            Reference = null;
            Connection = connection;
            foreach (var name in names)
            {
                _storages.Add(new Storage(name));
            }
        }

        public List<Storage> Storages => _storages;

        internal async void Start()
        {
            await StartAsync();
        }

        internal async Task StartAsync()
        {
            if (
                Reference == null
                || Connection.Opened
                || Connection.Reference == null
            )
            {
                throw new InvalidOperationException("data base not opened");
            }
            var mode = Write ? "readwrite" : "readonly";
            List<string> names = [];
            foreach (var storage in _storages)
            {
                names.Add(storage.Name);
            }
            Reference = await Connection.Reference.InvokeAsync<IJSObjectReference>(
                "transaction",
                DotNetObjectReference.Create(this),
                names,
                mode
            );
        }

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