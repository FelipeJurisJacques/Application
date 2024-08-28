using Microsoft.JSInterop;

namespace Application.Source.Core.Storage.IndexedDb
{
    public class Connection( Connections connections, IJSRuntime js, string name)
    {
        private bool _opened = false;
        private bool _opening = false;
        private Upgrade? _upgrade = null;
        private readonly string _name = name;
        private readonly IJSRuntime _js = js;
        private IJSObjectReference? _connection = null;
        private readonly List<Transaction> _transactions = [];
        private readonly Connections _connections = connections;

        public string Name => _name;

        public bool Opened => _opened;

        public bool Closed => !_opened;

        public Connections Connections => _connections;

        public async Task Open()
        {
            if (!_opened)
            {
                _opening = true;
                _connection = await _js.InvokeAsync<IJSObjectReference>(
                    "window.interop.indexedDb.open",
                    DotNetObjectReference.Create(this),
                    _name
                );
                _opened = true;
                _opening = false;
            }
        }

        public async Task Upgrade(Upgrade upgrade)
        {
            if (_opened || _opening)
            {
                throw new InvalidOperationException("data base previous opened");
            }
            _opening = true;
            _upgrade = upgrade;
            List<object> storages = [];
            foreach (var storage in upgrade.Storages)
            {
                List<object> attributes = [];
                foreach (var index in storage.Attributes)
                {
                    if (index.Indexable)
                    {
                        attributes.Add(new
                        {
                            name = index.Name,
                            unique = index.Unique,
                            multiEntry = index.MultiEntry,
                        });
                    }
                }
                storages.Add(new
                {
                    name = storage.Name,
                    keyPath = storage.Key == null ? "" : storage.Key.Name,
                    autoIncrement = storage.Key != null && storage.Key.AutoIncrement,
                    indexes = attributes,
                });
            }
            _connection = await _js.InvokeAsync<IJSObjectReference>(
                "window.interop.indexedDb.open",
                DotNetObjectReference.Create(this),
                _name,
                new
                {
                    stores = storages,
                    version = upgrade.Version,
                }
            );
            _opened = true;
            _opening = false;
        }

        public Transaction Transaction(string name)
        {
            var transaction = new Transaction(name);
            _transactions.Add(transaction);
            return transaction;
        }

        public Transaction Transaction(string name, bool write)
        {
            var transaction = new Transaction(name, write);
            _transactions.Add(transaction);
            return transaction;
        }

        public Transaction Transaction(List<string> names)
        {
            var transaction = new Transaction(names);
            _transactions.Add(transaction);
            return transaction;
        }

        public Transaction Transaction(List<string> names, bool write)
        {
            var transaction = new Transaction(names, write);
            _transactions.Add(transaction);
            return transaction;
        }

        [JSInvokable]
        public void OnEvent(string type)
        {
        }
    }
}