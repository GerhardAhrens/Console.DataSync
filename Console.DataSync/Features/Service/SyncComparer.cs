namespace Console.DataSync.Features.Service
{
    using Console.DataSync.Features.Models;

    public static class SyncComparer
    {
        public static bool IsRemoteNewer(SyncMetadata local, SyncMetadata remote)
        {
            if (remote.LastModifiedUtc > local.LastModifiedUtc)
            {
                return true;
            }

            return false;
        }
    }
}
