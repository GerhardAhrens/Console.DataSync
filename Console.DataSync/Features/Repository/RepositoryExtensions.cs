namespace Console.DataSync.Features.Repository
{
    using Console.DataSync.Features.Models;

    public static class RepositoryExtensions
    {
        public static IEnumerable<T> ActiveItems<T>(this IRepository<T> repository) where T : class, ISyncEntity
        {
            return repository.Items.Where(x => !x.Sync.Deleted);
        }

        public static IEnumerable<T> DeletedItems<T>(this IRepository<T> repository) where T : class, ISyncEntity
        {
            return repository.Items.Where(x => x.Sync.Deleted);
        }
    }
}
