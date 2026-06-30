namespace Console.DataSync.Features.Service
{
    using Console.DataSync.Features.Models;
    using Console.DataSync.Features.Repository;

    public class SyncEngine<T> where T : class, ISyncEntity
    {
        public int Synchronize(IRepository<T> repository, SyncPackage<T> package)
        {
            int count = 0;

            foreach (var remote in package.Items)
            {
                var local = repository.Find(remote.Id);

                //
                // Neuer Datensatz
                //

                if (local == null)
                {
                    repository.Add(remote);

                    count++;

                    continue;
                }

                //
                // Neuere Version?
                //

                if (SyncComparer.IsRemoteNewer(local.Sync, remote.Sync))
                {
                    repository.Remove(remote.Id);

                    repository.Add(remote);

                    count++;
                }
            }

            return count;
        }
    }
}
