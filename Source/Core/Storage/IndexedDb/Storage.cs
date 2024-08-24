namespace Application.Source.Core.Storage.IndexedDb
{
    public class Storage(string name)
    {
        private readonly string _name = name;

        public string Name => _name;

        public void Add() { }
    }
}