namespace Console.DataSync.Features.Service
{
    using Console.DataSync.Features.Models;
    using Console.DataSync.Features.Repository;

    public class ChangeTracker<T> : IChangeTracker<T> where T : class, ISyncEntity
    {
        private readonly IRepository<T> repository;

        public ChangeTracker(IRepository<T> repository)
        {
            this.repository = repository;
        }

        private long NextChangeNumber()
        {
            repository.Device.CurrentChangeNumber++;

            return repository.Device.CurrentChangeNumber;
        }

        private void Touch(T item)
        {
            item.Sync.ChangeNumber = NextChangeNumber();
            item.Sync.DeviceId = repository.Device.DeviceId;
            item.Sync.LastModifiedUtc = DateTime.UtcNow;
        }

        public T Insert(T item)
        {
            Touch(item);

            repository.Add(item);

            return item;
        }

        public bool Update(Guid id, Action<T> updateAction)
        {
            var item = repository.Find(id);

            if (item == null)
                return false;

            updateAction(item);

            Touch(item);

            return true;
        }

        public bool Delete(Guid id)
        {
            var item = repository.Find(id);

            if (item == null)
                return false;

            item.Sync.Deleted = true;

            Touch(item);

            return true;
        }
    }
}
