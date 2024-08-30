using Microsoft.JSInterop;

namespace Application.Source.Core.Storage.IndexedDb
{
    public class Connection
    {
        private Upgrade? _upgrade;
        private readonly string _name;
        internal readonly Handle Handler;
        public readonly Connections Connections;
        private readonly List<Transaction> _transactions;

        public Connection(Connections connections, IJSRuntime js, string name)
        {
            _name = name;
            _upgrade = null;
            Handler = new Handle(this, js);
            Connections = connections;
            _transactions = [];
        }

        public string Name => _name;

        public bool Opened => Handler.Opened;

        public bool Closed => !Handler.Opened;

        public async void Open()
        {
            await Handler.Open(_name);
        }

        public async void Upgrade(Upgrade upgrade)
        {
            _upgrade = upgrade;
            await Handler.Upgrade(_name, upgrade);
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
            await Handler.Transaction(transaction.Handler);
        }

        public class Handle(Connection connection, IJSRuntime js)
        {
            private bool _opened = false;
            private bool _opening = false;
            private readonly IJSRuntime _js = js;
            public IJSObjectReference? Reference = null;
            public readonly Connection Connection = connection;

            public bool Opened => _opened;

            public async Task Open(string name)
            {
                _opening = true;
                Reference = await _js.InvokeAsync<IJSObjectReference>(
                    "window.interop.indexedDb.open",
                    DotNetObjectReference.Create(this),
                    name
                );
                _opened = true;
                _opening = false;
            }

            public async Task Upgrade(string name, Upgrade upgrade)
            {
                if (_opened || _opening)
                {
                    throw new InvalidOperationException("data base previous opened");
                }
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
                Reference = await _js.InvokeAsync<IJSObjectReference>(
                    "window.interop.indexedDb.open",
                    DotNetObjectReference.Create(this),
                    name,
                    new
                    {
                        stores = storages,
                        version = upgrade.Version,
                    }
                );
                _opened = true;
                _opening = false;
            }

            public async Task Transaction(Transaction.Handle Handle)
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
}