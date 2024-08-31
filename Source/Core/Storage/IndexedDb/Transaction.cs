using Microsoft.JSInterop;

namespace Application.Source.Core.Storage.IndexedDb
{
    public class Transaction
    {
        private bool _closed;
        private bool _opening;
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
            _closed = false;
            _opening = false;
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
            if (!_closed && !_opening && Reference == null)
            {
                _opening = true;
                await Connection.OpenAsync();
                var reference = await Connection.GetReferenceAsync();
                if (reference == null)
                {
                    _closed = true;
                }
                else
                {
                    List<string> names = [];
                    foreach (var storage in _storages)
                    {
                        names.Add(storage.Name);
                    }
                    Reference = await reference.InvokeAsync<IJSObjectReference>(
                        "transaction",
                        DotNetObjectReference.Create(this),
                        names,
                        Write ? "readwrite" : "readonly"
                    );
                }
                _opening = false;
            }
        }

        public void Abort() {
            _closed = true;
        }

        public void Commit() {
            _closed = true;
        }

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