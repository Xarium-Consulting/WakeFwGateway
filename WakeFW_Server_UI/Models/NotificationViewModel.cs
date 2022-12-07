using System.Collections;

namespace WakeFW_Server_UI.Models
{
    public class NotificationViewModel : IEnumerable
    {
        public List<string> Notifications = new();
        public IEnumerator GetEnumerator()
        {
            foreach (object notification in Notifications)
            {
                if (notification == null)
                {
                    break;
                }

                yield return notification;
            }
        }
    }
}
