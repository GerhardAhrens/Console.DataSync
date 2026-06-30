namespace Console.DataSync.Features.Models
{
    public interface ISyncEntity
    {
        Guid Id { get; }

        SyncMetadata Sync { get; }
    }
}
