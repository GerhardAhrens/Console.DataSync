namespace Console.DataSync.Features.Models
{
    public class SyncMetadata
    {
        public Guid DeviceId { get; set; }

        public long ChangeNumber { get; set; }

        public DateTime LastModifiedUtc { get; set; }

        public bool Deleted { get; set; }
    }
}
