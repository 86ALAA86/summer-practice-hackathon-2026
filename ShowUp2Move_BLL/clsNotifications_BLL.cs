using System.Data;
using ShowUp2Move_DAL;

namespace ShowUp2Move.BLL
{
    public class clsNotification
    {
        public int NotificationID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        public static List<clsNotification> GetByUser(int userID, bool unreadOnly = false)
        {
            DataTable dt = clsNotificationDAL.GetUserNotifications(userID, unreadOnly);
            List<clsNotification> list = new List<clsNotification>();

            foreach (DataRow row in dt.Rows)
                list.Add(new clsNotification
                {
                    NotificationID = (int)row["NotificationID"],
                    Title = row["Title"].ToString()!,
                    Body = row["Body"].ToString()!,
                    Type = row["Type"].ToString()!,
                    IsRead = (bool)row["IsRead"],
                    CreatedAt = (DateTime)row["CreatedAt"]
                });

            return list;
        }

        public static bool Add(int userID, string title, string body,
                               string type, int? relatedID, ref int newNotifID)
            => clsNotificationDAL.AddNotification(userID, title, body, type, relatedID, ref newNotifID);

        public static bool MarkRead(int notificationID, int userID)
            => clsNotificationDAL.MarkNotificationRead(notificationID, userID);

        public static int MarkAllRead(int userID)
            => clsNotificationDAL.MarkAllNotificationsRead(userID);
    }
}