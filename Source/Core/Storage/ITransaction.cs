namespace Application.Source.Core.Storage
{
    public interface ITransaction
    {
        bool Write { get; }
        IConnection Connection { get; }
        void Abort();
        void Commit();
    }
}