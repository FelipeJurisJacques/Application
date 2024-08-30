using Microsoft.JSInterop;

namespace Application.Source.Core.Storage.IndexedDb
{
    public class Connection
    {
        private bool _opening;
        internal Upgrade? _upgrade;
        public readonly string Name;
        public IJSObjectReference? Reference;
        public readonly Connections Connections;
        internal readonly List<Transaction> _transactions;

        internal Connection(Connections connections, string name)
        {
            Name = name;
            _opening = false;
            _upgrade = null;
            Reference = null;
            Connections = connections;
            _transactions = [];
        }

        public bool Opened => Reference != null;

        public bool Closed => Reference == null;

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
            _opening = false;
        }

        public async void Upgrade(Upgrade upgrade)
        {
            await UpgradeAsync(upgrade);
        }

        public async Task UpgradeAsync(Upgrade upgrade)
        {
            if (_opening || Reference != null)
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
            _opening = false;
        }

        public Transaction Transaction(string name)
        {
            return Transaction([name], false);
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
            transaction.Start();
            return transaction;
        }
        public async Task<Transaction> TransactionAsync(string name)
        {
            return await TransactionAsync([name], false);
        }

        public async Task<Transaction> TransactionAsync(string name, bool write)
        {
            return await TransactionAsync([name], write);
        }

        public async Task<Transaction> TransactionAsync(List<string> names)
        {
            return await TransactionAsync(names, false);
        }

        public async Task<Transaction> TransactionAsync(List<string> names, bool write)
        {
            var transaction = new Transaction(this, names, write);
            _transactions.Add(transaction);
            await transaction.StartAsync();
            return transaction;
        }

        [JSInvokable]
        public void OnEvent(string type)
        {
        }
    }
}