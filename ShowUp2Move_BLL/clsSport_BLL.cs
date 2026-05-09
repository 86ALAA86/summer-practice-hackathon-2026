using ShowUp2Move.DAL;
using ShowUp2Move_DAL;
using System.Data;

namespace ShowUp2Move.BLL
{
    public class clsSport
    {
        public int SportID { get; set; }
        public string SportName { get; set; } = string.Empty;
        public int MinGroupSize { get; set; }
        public int MaxGroupSize { get; set; }

        public static List<clsSport> GetAll()
        {
            DataTable dt = clsSportDAL.GetAllSports();
            List<clsSport> list = new List<clsSport>();

            foreach (DataRow row in dt.Rows)
                list.Add(new clsSport
                {
                    SportID = (int)row["SportID"],
                    SportName = row["SportName"].ToString()!,
                    MinGroupSize = (int)row["MinGroupSize"],
                    MaxGroupSize = (int)row["MaxGroupSize"]
                });

            return list;
        }

        public static List<clsSport> GetUserSports(int userID)
        {
            DataTable dt = clsSportDAL.GetUserSports(userID);
            List<clsSport> list = new List<clsSport>();

            foreach (DataRow row in dt.Rows)
                list.Add(new clsSport
                {
                    SportID = (int)row["SportID"],
                    SportName = row["SportName"].ToString()!,
                    MinGroupSize = (int)row["MinGroupSize"],
                    MaxGroupSize = (int)row["MaxGroupSize"]
                });

            return list;
        }

        public static bool AddUserSport(int userID, int sportID)
            => clsSportDAL.AddUserSport(userID, sportID);

        public static bool DeleteUserSport(int userID, int sportID)
            => clsSportDAL.DeleteUserSport(userID, sportID);
    }
}