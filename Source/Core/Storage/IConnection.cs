namespace Application.Source.Core.Storage
{
    public interface IConnection
    {
        string Name { get; }
        Connections Connections { get; }
        void Open();
        Task OpenAsync();
        void Upgrade(IUpgrade upgrade);
        Task UpgradeAsync(IUpgrade upgrade);
        ITransaction Transaction(string name);
        ITransaction Transaction(string name, bool write);
        ITransaction Transaction(List<string> names);
        ITransaction Transaction(List<string> names, bool write);
        Task<ITransaction> TransactionAsync(string name);
        Task<ITransaction> TransactionAsync(string name, bool write);
        Task<ITransaction> TransactionAsync(List<string> names);
        Task<ITransaction> TransactionAsync(List<string> names, bool write);
    }
}