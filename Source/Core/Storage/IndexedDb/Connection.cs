using Microsoft.JSInterop;

namespace Application.Source.Core.Storage.IndexedDb
{
    public class Connection
    {
        private bool _opened;
        private bool _opening;
        internal Upgrade? _upgrade;
        public readonly string Name;
        public IJSObjectReference? Reference;
        public readonly Connections Connections;
        internal readonly List<Transaction> _transactions;

        internal Connection(Connections connections, string name)
        {
            Name = name;
            _opened = false;
            _opening = false;
            _upgrade = null;
            Reference = null;
            Connections = connections;
            _transactions = [];
        }

        public bool Opened => _opened;

        public bool Closed => !_opened;

        public async void Open()
        {
            await OpenAsync();
        }

        public async Task OpenAsync()
        {
            _opening = true;
            Reference = await Connections.JS.InvokeAsync<IJSObjectReference>(
                "window.interop.indexedDb.open",
                DotNetObjectReference.Create(this),
                Name
            );
            _opened = true;
            _opening = false;
        }

        public async void Upgrade(Upgrade upgrade)
        {
            await UpgradeAsync(upgrade);
        }

        public async Task UpgradeAsync(Upgrade upgrade)
        {
            if (_opened || _opening)
            {
                throw new InvalidOperationException("data base previous opened");
            }
            _upgrade = upgrade;
            _opening = true;
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
            Reference = await Connections.JS.InvokeAsync<IJSObjectReference>(
                "window.interop.indexedDb.open",
                DotNetObjectReference.Create(this),
                Name,
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
            return Transaction(name, false);
        }

        public Transaction Transaction(string name, bool write)
        {
            return Transaction([name], write);
        }

        public Transaction Transaction(List<string> names)
        {
            return Transaction(names, false);
        }

        public Transaction Transaction(List<string> names, bool write)
        {
            var transaction = new Transaction(this, names, write);
            _transactions.Add(transaction);
            _transaction(transaction);
            return transaction;
        }

        private async void _transaction(Transaction transaction)
        {
            if (!_opened || Reference == null)
            {
                throw new InvalidOperationException("data base not opened");
            }
            var mode = Handle.Transaction.Writable ? "readwrite" : "readonly";
            List<string> names = [];
            foreach (var storage in Handle.Transaction.Storages)
            {
                names.Add(storage.Name);
            }
            Handle.Reference = await Reference.InvokeAsync<IJSObjectReference>(
                "transaction",
                DotNetObjectReference.Create(Handle),
                names,
                mode
            );
        }

        [JSInvokable]
        public void OnEvent(string type)
        {
        }
    }
}