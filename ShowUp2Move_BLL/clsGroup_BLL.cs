using ShowUp2Move.DAL;
using ShowUp2Move_DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowUp2Move_BLL
{
    
   public class clsGroup_BLL
    {
        public int GroupID { get; set; }
        public string SportName { get; set; } = string.Empty;
        public string CaptainName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool HasConfirmed { get; set; }

        // ── Get Groups for a User ────────────────────────────────
        public static List<clsGroup_BLL> GetGroupsByUser(int userID)
        {
            DataTable dt = clsGroupDAL.GetGroupsByUser(userID);
            List<clsGroup_BLL> list = new List<clsGroup_BLL>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new clsGroup_BLL
                {
                    GroupID = (int)row["GroupID"],
                    SportName = row["SportName"].ToString()!,
                    CaptainName = row["CaptainName"].ToString()!,
                    Status = row["Status"].ToString()!,
                    CreatedAt = (DateTime)row["CreatedAt"],
                    HasConfirmed = (bool)row["HasConfirmed"]
                });
            }

            return list;
        }

        // ── Get Members of a Group ───────────────────────────────
        public static List<clsGroupMember> GetGroupMembers(int groupID)
        {
            DataTable dt = clsGroupDAL.GetGroupMembers(groupID);
            List<clsGroupMember> list = new List<clsGroupMember>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new clsGroupMember
                {
                    UserID = (int)row["UserID"],
                    FullName = row["FullName"].ToString()!,
                    SkillLevel = row["SkillLevel"] == DBNull.Value ? "" : row["SkillLevel"].ToString()!,
                    HasConfirmed = (bool)row["HasConfirmed"],
                    JoinedAt = (DateTime)row["JoinedAt"]
                });
            }

            return list;
        }

        // ── Matching Logic ───────────────────────────────────────
        public static bool RunMatching(int sportID, int minSize, int maxSize)
        {
            DataTable dt = clsGroupDAL.GetAvailableUsersBySport(sportID);

            if (dt.Rows.Count < minSize)
                return false;  // not enough players

            // take up to maxSize players — first row becomes captain (already randomized by NEWID())
            int captainUserID = (int)dt.Rows[0]["UserID"];
            int newGroupID = -1;

            bool created = clsGroupDAL.CreateGroup(sportID, captainUserID, ref newGroupID);

            if (!created)
                return false;

            int count = Math.Min(dt.Rows.Count, maxSize);

            for (int i = 0; i < count; i++)
            {
                int memberUserID = (int)dt.Rows[i]["UserID"];
                clsGroupDAL.AddGroupMember(newGroupID, memberUserID);
            }

            return true;
        }

        // ── Confirm ──────────────────────────────────────────────
        public static bool ConfirmParticipation(int groupID, int userID)
        {
            return clsGroupDAL.ConfirmParticipation(groupID, userID);
        }
    }

    // ── Helper class for group member display ────────────────────
    public class clsGroupMember
    {
        public int UserID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string SkillLevel { get; set; } = string.Empty;
        public bool HasConfirmed { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
