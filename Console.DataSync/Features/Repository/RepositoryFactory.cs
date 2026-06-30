namespace Console.DataSync.Features.Repository
{
    using Console.DataSync.Features.Models;

    public static class RepositoryFactory
    {
        public static Repository<T> Create<T>(string deviceName) where T : class, ISyncEntity
        {
            return new Repository<T>(deviceName);
        }
    }
}
