using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Common.Configuration.ServiceBus
{
    public class ReceiverEndPointConfiguration
    {
        public string QueueName { get; set; } = string.Empty;
        public int RateLimitVolume { get; set; }
        public int RateLimitIntervalInSeconds { get; set; }
        public int ConcurrentConsumerMessageLimit { get; set; }
        public CircuitBreakerConfig CircuitBreakerConfig { get; set; }
        public ushort PrefetchCount { get; set; }
    }
}
