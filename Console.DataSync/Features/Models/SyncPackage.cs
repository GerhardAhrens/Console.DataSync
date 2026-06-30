namespace Console.DataSync.Features.Models
{
    public class SyncPackage<T> where T : ISyncEntity
    {
        public Guid SourceDeviceId { get; set; }

        public long LastChangeNumber { get; set; }

        public DateTime CreatedUtc { get; set; }

        public List<T> Items { get; set; } = [];
    }
}
