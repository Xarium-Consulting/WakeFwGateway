using WakeFwGateway.Data.Enumeration;

namespace WakeFwGateway.Models
{
    internal class Notification
    {
        public string? Message { get; set; }
        public NotificationType Type { get; set; }

    }
}
