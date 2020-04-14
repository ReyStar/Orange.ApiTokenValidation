using System.IO;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Orange.ApiTokenValidation.Notification
{
    class JsonMessageSerializer<T>:  ISerializer<T>
    {
        private static readonly JsonSerializer JsonSerializer;

        static JsonMessageSerializer()
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new StringEnumConverter());

            JsonSerializer = JsonSerializer.Create(serializerSettings);
        }

        public byte[] Serialize(T data, SerializationContext context)
        {
            using var stream = new MemoryStream();
            using var streamWriter = new StreamWriter(stream);

            JsonSerializer.Serialize(streamWriter, data);
                
            return stream.ToArray();
        }
    }
}
