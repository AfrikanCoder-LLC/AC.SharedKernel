
namespace Kernel.Common.Configuration.ServiceBus
{
    public class CircuitBreakerConfig
    {
        public int TrackingPeriodInSeconds { get; set; }
        public int TripThreshold { get; set; }
        public int ActiveThreshold { get; set; }
        public int ResetIntervalInSeconds { get; set; }
    }
}
