namespace Application.Source.Core.Storage.IndexedDb
{
    public class Upgrade(int version)
    {
        private readonly int _version = version;

        public int Version => _version;
    }
}