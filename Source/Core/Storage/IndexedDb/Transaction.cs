using Microsoft.JSInterop;

namespace Application.Source.Core.Storage.IndexedDb
{
    public class Transaction
    {
        private bool _closed;
        public readonly bool Write;
        public readonly Connection Connection;
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
            Write = write;
            _closed = false;
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
            if (_tcs == null)
            {
                _tcs = new();
                try
                {
                    await Connection.OpenAsync();
                    var reference = await Connection.GetReferenceAsync();
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