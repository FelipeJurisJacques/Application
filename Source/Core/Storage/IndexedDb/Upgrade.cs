namespace Application.Source.Core.Storage.IndexedDb
{
    public class Upgrade : IUpgrade
    {
        private readonly int _version;
        private readonly List<Storage> _storages;

        public Upgrade(int version)
        {
            _version = version;
            _storages = [];
        }

        public Upgrade(int version, List<Storage> storages)
        {
            _version = version;
            _storages = storages;
        }

        public int Version => _version;

        public List<Storage> Storages => _storages;
    }
}