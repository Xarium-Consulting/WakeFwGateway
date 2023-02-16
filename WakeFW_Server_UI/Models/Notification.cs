using WakeFW_Server_UI.Data.Enumeration;

namespace WakeFW_Server_UI.Models
{
    internal class Notification
    {
        public string? Message { get; set; }
        public NotificationType Type { get; set; }

    }
}
