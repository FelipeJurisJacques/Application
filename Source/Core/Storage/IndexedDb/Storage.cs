namespace Application.Source.Core.Storage.IndexedDb
{
    public class Storage
    {
        private readonly string _name;
        private readonly List<Index> _indexes;

        public Storage(string name)
        {
            _name = name;
            _indexes = [];
        }

        public Storage(string name, List<Index> indexes)
        {
            _name = name;
            _indexes = indexes;
        }

        public string Name => _name;

        public void Add() { }
    }
}