namespace Console.DataSync.Features.Service
{
    using Console.DataSync.Features.Models;

    using System.Text.Json;

    public class JsonImporter<T> where T : class, ISyncEntity
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public SyncPackage<T> Load(string fileName)
        {
            var json = File.ReadAllText(fileName);

            return JsonSerializer.Deserialize<SyncPackage<T>>(json, Options)
                   ?? throw new InvalidOperationException("Fehler beim Deserialisieren.");
        }
    }
}
