using System.Data;
using ShowUp2Move_DAL;

namespace ShowUp2Move.BLL
{
    public class clsPoll
    {
        public int PollID { get; set; }
        public string Question { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }

        public static List<clsPoll> GetByGroup(int groupID)
        {
            DataTable dt = clsPollDAL.GetPollsByGroup(groupID);
            List<clsPoll> list = new List<clsPoll>();

            foreach (DataRow row in dt.Rows)
                list.Add(new clsPoll
                {
                    PollID = (int)row["PollID"],
                    Question = row["Question"].ToString()!,
                    ExpiresAt = row["ExpiresAt"] == DBNull.Value ? null : (DateTime?)row["ExpiresAt"]
                });

            return list;
        }

        public static bool Create(int groupID, int createdByID, string question,
                                  DateTime? expiresAt, ref int newPollID)
            => clsPollDAL.CreatePoll(groupID, createdByID, question, expiresAt, ref newPollID);

        public static bool AddOption(int pollID, string optionText, ref int newOptionID)
            => clsPollDAL.AddPollOption(pollID, optionText, ref newOptionID);

        public static int Vote(int pollID, int optionID, int userID)
            => clsPollDAL.VoteOnPoll(pollID, optionID, userID);

        public static DataTable GetResults(int pollID)
            => clsPollDAL.GetPollResults(pollID);
    }
}