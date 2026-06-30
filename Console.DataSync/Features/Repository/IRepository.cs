namespace Console.DataSync.Features.Repository
{
    using Console.DataSync.Features.Models;

    public interface IRepository<T> where T : class, ISyncEntity
    {
        DeviceInfo Device { get; }
        IReadOnlyCollection<T> Items { get; }
        int Count { get; }

        bool Contains(Guid id);
        T Find(Guid id);
        bool TryFind(Guid id, out T item);
        void Add(T item);
        void Remove(Guid id);
        void Replace(T item);
        void Clear();
        SyncState GetSyncState(Guid remoteDeviceId);
    }
}
