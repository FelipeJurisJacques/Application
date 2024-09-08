namespace Application.Source.Core.Storage.IndexedDb
{
    public record class Field : IField
    {
        private readonly string _name;
        private readonly List<FieldProperty> _properties;

        public Field(string name, List<FieldProperty> properties)
        {
            _name = name;
            _properties = properties;
        }

        public string Name => _name;

        public List<FieldProperty> Properties => _properties;
    }
}