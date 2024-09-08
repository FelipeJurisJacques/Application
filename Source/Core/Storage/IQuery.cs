namespace Application.Source.Core.Storage
{
    public interface IQuery
    {
        void From(string name);
        Task<IStatement> FindAsync(int id);
        Task<IStatement> FindAsync(string id);
        Task<IStatement> FetchAsync();
        Task<List<IStatement>> FetchAllAsync();
        Task InsertAsync(IStatement statement);
        Task UpdateAsync(IStatement statement);
        Task DeleteAsync();
    }
}