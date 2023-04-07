
namespace Kernel.Common.Configuration.ServiceBus
{
    public class BusOptions
    {
        public string Host { get; set; } = string.Empty;    
        public string VirtualHost { get; set; } = string.Empty; 
        public ushort Port { get; set; }
        public string UserName { get; set; } = string.Empty;    
        public string Password { get; set; } = string.Empty;    
    }
}
