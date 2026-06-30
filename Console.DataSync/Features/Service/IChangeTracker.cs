namespace Console.DataSync.Features.Service
{
    using Console.DataSync.Features.Models;

    public interface IChangeTracker<T> where T : class, ISyncEntity
    {
        T Insert(T item);

        bool Update(Guid id, Action<T> updateAction);

        bool Delete(Guid id);
    }
}
