using System.IO;
using System.Threading.Tasks;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Orange.ApiTokenValidation.Notification
{
    class JsonMessageAsyncSerializer<T> : IAsyncSerializer<T>
    {
        private static readonly JsonSerializer JsonSerializer;

        static JsonMessageAsyncSerializer()
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new StringEnumConverter());

            JsonSerializer = JsonSerializer.Create(serializerSettings);
        }

        public async Task<byte[]> SerializeAsync(T data, SerializationContext context)
        {
            await using var stream = new MemoryStream();
            await using var streamWriter = new StreamWriter(stream);

            JsonSerializer.Serialize(streamWriter, data);

            return stream.ToArray();
        }
    }
}
