using Microsoft.JSInterop;

namespace Application.Source.Core.Storage.IndexedDb
{
    public class Transaction : ITransaction
    {
        private bool _closed;
        private readonly bool _write;
        private readonly Connection _connection;
        internal IJSObjectReference? Reference;
        private readonly List<Storage> _storages;
        private TaskCompletionSource<IJSObjectReference>? _tcs;

        internal Transaction(Connection connection, List<string> names, bool write)
        {
            if (names.Count == 0)
            {
                throw new InvalidOperationException("storage name list is empty");
            }
            _tcs = null;
            _write = write;
            _closed = false;
            _storages = [];
            Reference = null;
            _connection = connection;
            foreach (var name in names)
            {
                _storages.Add(new Storage(name));
            }
        }

        public bool Write => _write;

        public IConnection Connection => _connection;

        public List<Storage> Storages => _storages;

        internal async void Start()
        {
            await StartAsync();
        }

        internal async Task StartAsync()
        {
            if (_tcs == null)
            {
                _tcs = new();
                try
                {
                    await Connection.OpenAsync();
                    var reference = await _connection.GetReferenceAsync();
                    if (reference == null)
                    {
                        _closed = true;
                        _tcs.SetCanceled();
                    }
                    else
                    {
                        List<string> names = [];
                        foreach (var storage in _storages)
                        {
                            names.Add(storage.Name);
                        }
                        _tcs.SetResult(await reference.InvokeAsync<IJSObjectReference>(
                            "transaction",
                            DotNetObjectReference.Create(this),
                            names,
                            Write ? "readwrite" : "readonly"
                        ));
                    }
                }
                catch (Exception error)
                {
                    _closed = true;
                    _tcs.SetException(error);
                }
            }
            else
            {
                await _tcs.Task;
            }
        }

        public void Abort()
        {
            _closed = true;
        }

        public void Commit()
        {
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

        internal async Task<IJSObjectReference?> GetReferenceAsync()
        {
            return _tcs == null ? null : await _tcs.Task;
        }
    }
}