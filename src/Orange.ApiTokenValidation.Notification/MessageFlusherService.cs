using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Orange.ApiTokenValidation.Notification
{
    class MessageFlusherService: BackgroundService
    {
        private readonly TimeSpan _period;
        private readonly TokenNotifier _tokenNotifier;
        private readonly ILogger _logger;

        public MessageFlusherService(IOptions<TokenNotifierConfig> notifierConfig, 
                                     TokenNotifier tokenNotifier, 
                                     ILogger<MessageFlusherService> logger)
        {
            _tokenNotifier = tokenNotifier;
            _logger = logger;
            _period = notifierConfig.Value.FlushPeriod;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    _tokenNotifier.Flush(token);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Flush notification error");
                }

                await Task.Delay(_period, token);
            }
        }
    }
}
