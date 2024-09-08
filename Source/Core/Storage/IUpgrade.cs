namespace Application.Source.Core.Storage
{
    public interface IUpgrade
    {
        int Version { get; }
    }
}