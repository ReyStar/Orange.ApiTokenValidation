using System;
using System.Collections.Generic;

namespace Orange.ApiTokenValidation.Notification
{
    public class TokenNotifierConfig
    {
        public string Topic { get; set; }

        public IEnumerable<string> Servers { get; set; }

        public TimeSpan FlushPeriod { get; set; }
    }
}
