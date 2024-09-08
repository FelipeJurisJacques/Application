namespace Application.Source.Core.Storage.IndexedDb
{
    public class Upgrade : IUpgrade
    {
        private readonly int _version;
        private readonly List<IStorage> _storages;

        public Upgrade(int version)
        {
            _version = version;
            _storages = [];
        }

        public Upgrade(int version, List<IStorage> storages)
        {
            _version = version;
            _storages = storages;
        }

        public int Version => _version;

        public List<IStorage> Storages => _storages;
    }
}