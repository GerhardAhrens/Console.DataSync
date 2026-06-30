namespace Console.DataSync.Features.Models
{
    public class DeviceInfo
    {
        public Guid DeviceId { get; init; } = Guid.NewGuid();

        public string DeviceName { get; set; } = "";

        /// <summary>
        /// Höchste lokale Änderungsnummer.
        /// </summary>
        public long CurrentChangeNumber { get; set; }
    }
}
