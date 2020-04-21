using System;
using System.Threading;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orange.ApiTokenValidation.Application.Interfaces;
using Orange.ApiTokenValidation.Application.NotifyMessages;

namespace Orange.ApiTokenValidation.Notification
{
    internal class TokenNotifier: ITokenNotifier, IDisposable
    {
        private readonly IOptions<TokenNotifierConfig> _tokenNotifierConfig;
        private readonly ILogger _logger;
        private IProducer<Null, TokenNotifyMessage> _publisher;

        public TokenNotifier(IOptions<TokenNotifierConfig> tokenNotifierConfig,
                             ILogger<TokenNotifier> logger)
        {
            _tokenNotifierConfig = tokenNotifierConfig;
            _logger = logger;
            var config = new ProducerConfig
            {
                BootstrapServers = string.Join(";", tokenNotifierConfig.Value.Servers),
                Acks = Acks.Leader,
                CompressionType = CompressionType.Gzip,
            };

            _publisher = new ProducerBuilder<Null, TokenNotifyMessage>(config)
                            .SetValueSerializer(new JsonMessageSerializer<TokenNotifyMessage>())
                            .Build();
        }

        public void Notify(TokenNotifyMessage message)
        {
            _publisher.Produce(_tokenNotifierConfig.Value.Topic, new Message<Null, TokenNotifyMessage>
                                                                 {
                                                                     Value = message, 
                                                                     Timestamp = new Timestamp(DateTimeOffset.UtcNow)
                                                                 }, 
            report =>
            {
                if (report.Error.IsError)
                {
                    _logger.LogError("Sent notify message error @error", report.Error);
                }
            });
        }

        public void Flush(CancellationToken token = default)
        {
            _publisher.Flush(token);
        }
        
        public void Dispose()
        {
            Flush();

            _publisher?.Dispose();
            _publisher = null;
        }
    }
}
