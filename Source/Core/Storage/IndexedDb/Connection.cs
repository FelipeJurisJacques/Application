using Microsoft.JSInterop;

namespace Application.Source.Core.Storage.IndexedDb
{
    public class Connection
    {
        private bool _opened;
        private Upgrade? _upgrade;
        private readonly string _name;
        private readonly IJSRuntime _js;
        private readonly List<Transaction> _transactions;

        public Connection(IJSRuntime js, string name)
        {
            _js = js;
            _name = name;
            _opened = false;
            _upgrade = null;
            _transactions = [];
            _open();
        }

        public Connection(IJSRuntime js, string name, Upgrade upgrade)
        {
            _js = js;
            _name = name;
            _opened = false;
            _upgrade = upgrade;
            _transactions = [];
            _open();
        }

        public string Name => _name;

        public bool Opened => _opened;

        public bool Closed => !_opened;

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

        private async void _open()
        {
            var tcs = new TaskCompletionSource<object>();
            if (_upgrade == null)
            {
                await _js.InvokeVoidAsync(
                    "interop.indexDb.open",
                    DotNetObjectReference.Create(this),
                    tcs,
                    _name
                );
            }
            else
            {
                await _js.InvokeVoidAsync(
                    "interop.indexDb.open",
                    DotNetObjectReference.Create(this),
                    tcs,
                    _name
                );
            }
            await tcs.Task;
            _opened = tcs.Task.IsCompletedSuccessfully;
        }
    }
}