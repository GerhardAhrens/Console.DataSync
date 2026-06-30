namespace Console.DataSync.Features.Service
{
    using Console.DataSync.Features.Models;
    using Console.DataSync.Features.Repository;

    using System.Text.Json;

    public class JsonExporter<T> where T : class, ISyncEntity
    {
        public SyncPackage<T> Export(IRepository<T> repository, long lastExportedChange)
        {
            return new SyncPackage<T>
            {
                SourceDeviceId = repository.Device.DeviceId,

                LastChangeNumber = repository.Device.CurrentChangeNumber,

                CreatedUtc = DateTime.UtcNow,

                Items = repository.Items
                    .Where(x => x.Sync.ChangeNumber > lastExportedChange)
                    .OrderBy(x => x.Sync.ChangeNumber)
                    .ToList()
            };
        }

        public void Save(SyncPackage<T> package, string fileName)
        {
            JsonSerializerOptions jsonSerializerOptions = new()
            {
                WriteIndented = true
            };

            var options = jsonSerializerOptions;

            File.WriteAllText(fileName, JsonSerializer.Serialize(package, options));
        }
    }
}
