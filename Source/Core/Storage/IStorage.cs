namespace Application.Source.Core.Storage
{
    public interface IStorage
    {
        string Name { get; }
        List<IField> Fields { get; }
    }
}