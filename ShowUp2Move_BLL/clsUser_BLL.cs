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
    public class clsUser_BLL
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SkillLevel { get; set; } = string.Empty;
        public bool IsAvailableToday { get; set; }
        public DateTime CreatedAt { get; set; }

        // ── Find ────────────────────────────────────────────────
        public static clsUser_BLL? Find(int userID)
        {
            DataTable dt = clsUserDAL.GetUserByID(userID);

            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];

            return new clsUser_BLL
            {
                UserID = (int)row["UserID"],
                Username = row["Username"].ToString()!,
                FullName = row["FullName"].ToString()!,
                Description = row["Description"] == DBNull.Value ? "" : row["Description"].ToString()!,
                SkillLevel = row["SkillLevel"] == DBNull.Value ? "" : row["SkillLevel"].ToString()!,
                IsAvailableToday = (bool)row["IsAvailableToday"],
                CreatedAt = (DateTime)row["CreatedAt"]
            };
        }

        public static clsUser_BLL? FindByCredentials(string username, string password)
        {
            DataTable dt = clsUserDAL.GetUserByCredentials(username, password);

            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];

            return new clsUser_BLL
            {
                UserID = (int)row["UserID"],
                Username = row["Username"].ToString()!,
                FullName = row["FullName"].ToString()!,
                Description = row["Description"] == DBNull.Value ? "" : row["Description"].ToString()!,
                SkillLevel = row["SkillLevel"] == DBNull.Value ? "" : row["SkillLevel"].ToString()!,
                IsAvailableToday = (bool)row["IsAvailableToday"]
            };
        }

        // ── Add ─────────────────────────────────────────────────
        public static bool Add(string username, string password, string fullName, ref int newUserID)
        {
            return clsUserDAL.AddUser(username, password, fullName, ref newUserID);
        }

        // ── Update ──────────────────────────────────────────────
        public static bool UpdateProfile(int userID, string fullName, string description, string skillLevel)
        {
            return clsUserDAL.UpdateUserProfile(userID, fullName, description, skillLevel);
        }

        public static bool UpdateAvailability(int userID, bool isAvailable)
        {
            return clsUserDAL.UpdateUserAvailability(userID, isAvailable);
        }
    }
}
