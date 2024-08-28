using Microsoft.JSInterop;

namespace Application.Source.Core.Storage.IndexedDb
{
    public class Connection(IJSRuntime js, string name)
    {
        private bool _opened = false;
        private bool _opening = false;
        private Upgrade? _upgrade = null;
        private readonly string _name = name;
        private readonly IJSRuntime _js = js;
        private IJSObjectReference? _connection = null;
        private readonly List<Transaction> _transactions = [];

        public string Name => _name;

        public bool Opened => _opened;

        public bool Closed => !_opened;

        public async Task Upgrade(Upgrade upgrade)
        {
            if (_opened || _opening)
            {
                throw new InvalidOperationException("data base previous opened");
            }
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
                            multi = index.MultiEntry,
                            unique = index.Unique,
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
            _opening = true;
            _upgrade = upgrade;
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

        public Storage GetStorage(string name)
        {
            return Transaction(name).GetStorage(name);
        }

        public Storage GetStorage(string name, bool write)
        {
            return Transaction(name, write).GetStorage(name);
        }

        [JSInvokable]
        public void OnEvent(string type)
        {
        }

        private async Task _open()
        {
            if (!_opened)
            {
                await _js.InvokeVoidAsync(
                    "window.interop.indexedDb.open",
                    DotNetObjectReference.Create(this),
                    _name
                );
            }
        }
    }
}