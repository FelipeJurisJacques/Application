namespace Application.Source.Core.Storage
{
    public interface IUpgrade
    {
        int Version { get; }
        public List<IStorage> Storages { get; }
    }
}