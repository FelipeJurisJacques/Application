namespace Application.Source.Core.Storage.IndexedDb
{
    public class Storage
    {
        private readonly string _name;
        private readonly Attribute? _key;
        private readonly List<Attribute> _attributes;

        public Storage(string name)
        {
            _key = null;
            _name = name;
            _attributes = [];
        }

        public Storage(string name, List<Attribute> indexes)
        {
            _key = null;
            _name = name;
            _attributes = [];
            foreach (var attribute in indexes)
            {
                if (attribute.Key)
                {
                    if (_key == null)
                    {
                        _key = attribute;
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            "primary key is duplicated in storage " + _name
                        );
                    }
                }
                else
                {
                    _attributes.Add(attribute);
                }
            }
        }

        public string Name => _name;

        public Attribute? Key => _key;

        public List<Attribute> Attributes => _attributes;

        public void Add() { }
    }
}