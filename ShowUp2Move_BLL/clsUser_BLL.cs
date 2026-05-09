using ShowUp2Move.DAL;
using ShowUp2Move_DAL;
using System.Data;

namespace ShowUp2Move.BLL
{
    public class clsUser
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SkillLevel { get; set; } = string.Empty;
        public bool IsAvailableToday { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProfilePhotoUrl { get; set; } = string.Empty;

        public static clsUser? Find(int userID)
        {
            DataTable dt = clsUserDAL.GetUserByID(userID);
            if (dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            return new clsUser
            {
                UserID = (int)row["UserID"],
                Username = row["Username"].ToString()!,
                FullName = row["FullName"].ToString()!,
                Description = row["Description"] == DBNull.Value ? "" : row["Description"].ToString()!,
                SkillLevel = row["SkillLevel"] == DBNull.Value ? "" : row["SkillLevel"].ToString()!,
                IsAvailableToday = (bool)row["IsAvailableToday"],
                CreatedAt = (DateTime)row["CreatedAt"],
                ProfilePhotoUrl = row["ProfilePhotoUrl"] == DBNull.Value ? "" : row["ProfilePhotoUrl"].ToString()!
            };
        }

        public static clsUser? FindByCredentials(string username, string password)
        {
            DataTable dt = clsUserDAL.GetUserByCredentials(username, password);
            if (dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            return new clsUser
            {
                UserID = (int)row["UserID"],
                Username = row["Username"].ToString()!,
                FullName = row["FullName"].ToString()!,
                Description = row["Description"] == DBNull.Value ? "" : row["Description"].ToString()!,
                SkillLevel = row["SkillLevel"] == DBNull.Value ? "" : row["SkillLevel"].ToString()!,
                IsAvailableToday = (bool)row["IsAvailableToday"]
            };
        }

        public static bool Add(string username, string password, string fullName, ref int newUserID)
            => clsUserDAL.AddUser(username, password, fullName, ref newUserID);

        public static bool UpdateProfile(int userID, string fullName, string description, string skillLevel)
            => clsUserDAL.UpdateUserProfile(userID, fullName, description, skillLevel);

        public static bool UpdateAvailability(int userID, bool isAvailable)
            => clsUserDAL.UpdateUserAvailability(userID, isAvailable);

        public static bool UpdateProfilePhoto(int userID, string photoUrl)
            => clsUserDAL.UpdateProfilePhoto(userID, photoUrl);

        public static bool UpdateLocation(int userID, double latitude, double longitude)
            => clsUserDAL.UpdateUserLocation(userID, latitude, longitude);
    }
}