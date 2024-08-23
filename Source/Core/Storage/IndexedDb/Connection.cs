namespace Application.Source.Core.Storage.IndexedDb
{
    public class Connection
    {
        private Upgrade? _upgrade;
        private readonly string _name;
        private readonly List<Transaction> _transactions;

        public Connection(string name)
        {
            _name = name;
            _upgrade = null;
            _transactions = [];
        }

        public Connection(string name, Upgrade upgrade)
        {
            _name = name;
            _upgrade = upgrade;
            _transactions = [];
        }

        public string Name => _name;

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
    }
}