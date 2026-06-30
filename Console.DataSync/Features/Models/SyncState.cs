namespace Console.DataSync.Features.Models
{
    public class SyncState
    {
        /// <summary>
        /// Partnergerät.
        /// </summary>
        public Guid RemoteDeviceId { get; set; }

        /// <summary>
        /// Letzte exportierte Änderung.
        /// </summary>
        public long LastExportedChange { get; set; }

        /// <summary>
        /// Letzte importierte Änderung.
        /// </summary>
        public long LastImportedChange { get; set; }

        /// <summary>
        /// Zeitpunkt der letzten Synchronisation.
        /// </summary>
        public DateTime LastSyncUtc { get; set; }
    }
}
