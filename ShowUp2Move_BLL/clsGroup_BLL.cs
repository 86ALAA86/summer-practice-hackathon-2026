using ShowUp2Move.DAL;
using ShowUp2Move_DAL;
using System.Data;

namespace ShowUp2Move.BLL
{
    public class clsGroup
    {
        public int GroupID { get; set; }
        public string SportName { get; set; } = string.Empty;
        public string CaptainName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool HasConfirmed { get; set; }

        public static List<clsGroup> GetGroupsByUser(int userID)
        {
            DataTable dt = clsGroupDAL.GetGroupsByUser(userID);
            List<clsGroup> list = new List<clsGroup>();

            foreach (DataRow row in dt.Rows)
                list.Add(new clsGroup
                {
                    GroupID = (int)row["GroupID"],
                    SportName = row["SportName"].ToString()!,
                    CaptainName = row["CaptainName"].ToString()!,
                    Status = row["Status"].ToString()!,
                    CreatedAt = (DateTime)row["CreatedAt"],
                    HasConfirmed = (bool)row["HasConfirmed"]
                });

            return list;
        }

        public static List<clsGroupMember> GetGroupMembers(int groupID)
        {
            DataTable dt = clsGroupDAL.GetGroupMembers(groupID);
            List<clsGroupMember> list = new List<clsGroupMember>();

            foreach (DataRow row in dt.Rows)
                list.Add(new clsGroupMember
                {
                    UserID = (int)row["UserID"],
                    FullName = row["FullName"].ToString()!,
                    SkillLevel = row["SkillLevel"] == DBNull.Value ? "" : row["SkillLevel"].ToString()!,
                    HasConfirmed = (bool)row["HasConfirmed"],
                    JoinedAt = (DateTime)row["JoinedAt"]
                });

            return list;
        }

        public static bool RunMatching(int sportID, int minSize, int maxSize)
        {
            DataTable dt = clsGroupDAL.GetAvailableUsersBySport(sportID);
            return CreateGroupFromTable(dt, sportID, minSize, maxSize);
        }

        
        public static bool RunMatchingNearby(int sportID, int minSize, int maxSize,
                                             double latitude, double longitude, double radiusKm = 10.0)
        {
            DataTable dt = clsGroupDAL.GetAvailableUsersBySportNearby(sportID, latitude, longitude, radiusKm);
            return CreateGroupFromTable(dt, sportID, minSize, maxSize);
        }

        private static bool CreateGroupFromTable(DataTable dt, int sportID, int minSize, int maxSize)
        {
            if (dt.Rows.Count < minSize)
                return false;

            int captainUserID = (int)dt.Rows[0]["UserID"];
            int newGroupID = -1;

            bool created = clsGroupDAL.CreateGroup(sportID, captainUserID, ref newGroupID);
            if (!created) return false;

            int count = Math.Min(dt.Rows.Count, maxSize);
            for (int i = 0; i < count; i++)
            {
                int memberUserID = (int)dt.Rows[i]["UserID"];
                clsGroupDAL.AddGroupMember(newGroupID, memberUserID);
            }

            return true;
        }

        public static bool ConfirmParticipation(int groupID, int userID)
            => clsGroupDAL.ConfirmParticipation(groupID, userID);
    }

    public class clsGroupMember
    {
        public int UserID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string SkillLevel { get; set; } = string.Empty;
        public bool HasConfirmed { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}