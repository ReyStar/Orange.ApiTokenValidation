using System;
using Confluent.Kafka;
using Orange.ApiTokenValidation.Domain.Interfaces;

namespace Orange.ApiTokenValidation.Notification
{
    internal class TokenNotifier: ITokenNotifier, IDisposable
    {
        private IProducer<Null, string> _publisher;
        private const string TokenTopic = "token_topic";

        public TokenNotifier(TokenNotifierConfig tokenNotifierConfig)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = string.Join(";", tokenNotifierConfig.Servers)
            };

            _publisher = new ProducerBuilder<Null, string>(config)
                //.SetValueSerializer(new ProtoBufSerializer())
                .Build();
        }

        public void Notify()
        {
            _publisher.Produce(TokenTopic, new Message<Null, string>()
            {
                Value = "Notify"
            });
        }

        public void Dispose()
        {
            Flush();
            _publisher?.Dispose();
            _publisher = null;
        }

        private void Flush()
        {
            //TODO sync on publisher disposing

            _publisher.Flush();
        }
    }

    //internal class ProtoBufSerializer : ISerializer<string>, IDeserializer<string>
    //{
    //    public byte[] Serialize(string data, SerializationContext context)
    //    {
            
    //    }

    //    public string Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    //    {
            
    //    }
    //}
}
