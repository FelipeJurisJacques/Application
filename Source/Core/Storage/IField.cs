namespace Application.Source.Core.Storage
{
    public interface IField
    {
        string Name { get; }
        List<FieldProperty> Properties { get; }
    }
}