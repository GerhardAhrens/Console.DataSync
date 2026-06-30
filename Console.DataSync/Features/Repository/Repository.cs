namespace Console.DataSync.Features.Repository
{
    using Console.DataSync.Features.Models;

    using System.Collections;

    public class Repository<T> : IRepository<T>, IEnumerable<T> where T : class, ISyncEntity
    {
        private readonly Dictionary<Guid, T> _items = [];
        private readonly Dictionary<Guid, SyncState> _syncStates = new();


        public DeviceInfo Device { get; }

        public int Count => this._items.Count;

        public Repository(string deviceName)
        {
            Device = new DeviceInfo
            {
                DeviceName = deviceName
            };
        }

        public IReadOnlyCollection<T> Items => _items.Values;

        public bool Contains(Guid id)
        {
            return _items.ContainsKey(id);
        }

        public T Find(Guid id)
        {
            _items.TryGetValue(id, out var item);

            return item;
        }

        public bool TryFind(Guid id, out T item)
        {
            return _items.TryGetValue(id, out item);
        }

        public void Add(T item)
        {
            _items[item.Id] = item;
        }

        public void Remove(Guid id)
        {
            _items.Remove(id);
        }

        public void Replace(T item)
        {
            if (!_items.ContainsKey(item.Id))
            {
                throw new InvalidOperationException($"Datensatz {item.Id} existiert nicht.");
            }

            _items[item.Id] = item;
        }
        public void Clear()
        {
            _items.Clear();
        }

        public SyncState GetSyncState(Guid remoteDeviceId)
        {
            if (!_syncStates.TryGetValue(remoteDeviceId, out var state))
            {
                state = new SyncState
                {
                    RemoteDeviceId = remoteDeviceId
                };

                _syncStates.Add(remoteDeviceId, state);
            }

            return state;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
