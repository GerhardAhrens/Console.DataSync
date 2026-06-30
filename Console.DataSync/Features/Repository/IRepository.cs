namespace Console.DataSync.Features.Repository
{
    using Console.DataSync.Features.Models;

    public interface IRepository<T> where T : ISyncEntity
    {
        DeviceInfo Device { get; }

        IReadOnlyCollection<T> Items { get; }

        bool Contains(Guid id);

        T Find(Guid id);

        void Add(T item);

        void Remove(Guid id);

        void Clear();

        SyncState GetSyncState(Guid remoteDeviceId);
    }
}
