namespace Console.DataSync.Features.Models
{
    public class Kunde : ISyncEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public string Name { get; set; } = "";

        public string Ort { get; set; } = "";

        public SyncMetadata Sync { get; set; } = new();

        public override string ToString() => $"{Name,-15} {Ort,-15} Change={Sync.ChangeNumber} Deleted={Sync.Deleted}";
    }
}
