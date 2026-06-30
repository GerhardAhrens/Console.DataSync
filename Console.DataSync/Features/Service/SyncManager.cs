namespace Console.DataSync.Features.Service
{
    using Console.DataSync.Features.Models;
    using Console.DataSync.Features.Repository;

    public class SyncManager<T> where T : class, ISyncEntity
    {
        private readonly IRepository<T> repository;

        private readonly JsonExporter<T> exporter = new();

        private readonly JsonImporter<T> importer = new();

        private readonly SyncEngine<T> engine = new();

        public SyncManager(IRepository<T> repository)
        {
            this.repository = repository;
        }

        public void Export(string fileName, Guid remoteDeviceId)
        {
            var state = repository.GetSyncState(remoteDeviceId);

            var package = exporter.Export(repository, state.LastExportedChange);

            exporter.Save(package, fileName);

            state.LastExportedChange = repository.Device.CurrentChangeNumber;

            state.LastSyncUtc = DateTime.UtcNow;
        }

        public int Import(string fileName)
        {
            var package = importer.Load(fileName);

            var count = engine.Synchronize(repository, package);

            var state = repository.GetSyncState( package.SourceDeviceId);

            state.LastImportedChange = package.LastChangeNumber;

            state.LastSyncUtc = DateTime.UtcNow;

            return count;
        }
    }
}
