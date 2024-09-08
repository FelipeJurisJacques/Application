using Microsoft.JSInterop;

namespace Application.Source.Core.Storage.IndexedDb
{
    public class Connection : IConnection
    {
        internal IUpgrade? _upgrade;
        private readonly string _name;
        private readonly Connections _connections;
        internal readonly List<Transaction> _transactions;
        private TaskCompletionSource<IJSObjectReference>? _tcs;

        internal Connection(Connections connections, string name)
        {
            _name = name;
            _tcs = null;
            _upgrade = null;
            _connections = connections;
            _transactions = [];
        }

        public string Name => _name;

        public Connections Connections => _connections;

        public async void Open()
        {
            await OpenAsync();
        }

        public async Task OpenAsync()
        {
            if (_tcs == null)
            {
                _tcs = new();
                try
                {
                    _tcs.SetResult(await Connections.JS.InvokeAsync<IJSObjectReference>(
                        "window.interop.indexedDb.open",
                        DotNetObjectReference.Create(this),
                        Name
                    ));
                }
                catch (Exception error)
                {
                    _tcs.SetException(error);
                }
            }
            else
            {
                await _tcs.Task;
            }
        }

        public async void Upgrade(IUpgrade upgrade)
        {
            await UpgradeAsync(upgrade);
        }

        public async Task UpgradeAsync(IUpgrade upgrade)
        {
            if (_tcs != null)
            {
                throw new InvalidOperationException("data base previous opened");
            }
            _upgrade = upgrade;
            _tcs = new();
            List<object> storages = [];
            foreach (var storage in upgrade.Storages)
            {
                IField? key = null;
                List<object> fields = [];
                foreach (var index in storage.Fields)
                {
                    if (index.Properties.Contains(FieldProperty.KEY))
                    {
                        key = index;
                    }
                    else if (index.Properties.Contains(FieldProperty.INDEXABLE))
                    {
                        fields.Add(new
                        {
                            name = index.Name,
                            unique = index.Properties.Contains(
                                FieldProperty.UNIQUE
                            ),
                            multiEntry = index.Properties.Contains(
                                FieldProperty.MULTI_ENTRY
                            ),
                        });
                    }
                }
                storages.Add(new
                {
                    name = storage.Name,
                    keyPath = key == null ? "" : key.Name,
                    autoIncrement = key != null && key.Properties.Contains(
                        FieldProperty.DEFAULT_VALUE_AUTO_INCREMENT
                    ),
                    indexes = fields,
                });
            }
            try
            {
                _tcs.SetResult(await Connections.JS.InvokeAsync<IJSObjectReference>(
                    "window.interop.indexedDb.open",
                    DotNetObjectReference.Create(this),
                    Name,
                    new
                    {
                        stores = storages,
                        version = upgrade.Version,
                    }
                ));
            }
            catch (Exception error)
            {
                _tcs.SetException(error);
            }
        }

        public ITransaction Transaction(string name)
        {
            return Transaction([name], false);
        }

        public ITransaction Transaction(string name, bool write)
        {
            return Transaction([name], write);
        }

        public ITransaction Transaction(List<string> names)
        {
            return Transaction(names, false);
        }

        public ITransaction Transaction(List<string> names, bool write)
        {
            var transaction = new Transaction(this, names, write);
            _transactions.Add(transaction);
            transaction.Start();
            return transaction;
        }

        public async Task<ITransaction> TransactionAsync(string name)
        {
            return await TransactionAsync([name], false);
        }

        public async Task<ITransaction> TransactionAsync(string name, bool write)
        {
            return await TransactionAsync([name], write);
        }

        public async Task<ITransaction> TransactionAsync(List<string> names)
        {
            return await TransactionAsync(names, false);
        }

        public async Task<ITransaction> TransactionAsync(List<string> names, bool write)
        {
            var transaction = new Transaction(this, names, write);
            _transactions.Add(transaction);
            await transaction.StartAsync();
            return transaction;
        }

        internal async Task<IJSObjectReference?> GetReferenceAsync()
        {
            return _tcs == null ? null : await _tcs.Task;
        }

        [JSInvokable]
        public void OnEvent(string type)
        {
        }
    }
}