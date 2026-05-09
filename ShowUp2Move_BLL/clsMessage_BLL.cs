using System.Data;
using ShowUp2Move_DAL;

namespace ShowUp2Move.BLL
{
    public class clsMessage
    {
        public int MessageID { get; set; }
        public int GroupID { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }

        public static List<clsMessage> GetByGroup(int groupID, int lastN = 50)
        {
            DataTable dt = clsMessageDAL.GetMessagesByGroup(groupID, lastN);
            List<clsMessage> list = new List<clsMessage>();

            foreach (DataRow row in dt.Rows)
                list.Add(new clsMessage
                {
                    MessageID = (int)row["MessageID"],
                    GroupID = groupID,
                    SenderName = row["SenderName"].ToString()!,
                    Content = row["Content"].ToString()!,
                    SentAt = (DateTime)row["SentAt"]
                });

            return list;
        }

        public static bool Send(int groupID, int senderID, string content, ref int newMessageID)
            => clsMessageDAL.SendMessage(groupID, senderID, content, ref newMessageID);

        public static int MarkRead(int groupID, int userID)
            => clsMessageDAL.MarkMessagesRead(groupID, userID);
    }
}