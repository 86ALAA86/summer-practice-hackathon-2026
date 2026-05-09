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
    public class clsSport_BLL
    {
        public int SportID { get; set; }
        public string SportName { get; set; } = string.Empty;
        public int MinGroupSize { get; set; }
        public int MaxGroupSize { get; set; }

        // ── Get All ─────────────────────────────────────────────
        public static List<clsSport_BLL> GetAll()
        {
            DataTable dt = clsSportDAL.GetAllSports();
            List<clsSport_BLL> list = new List<clsSport_BLL>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new clsSport_BLL
                {
                    SportID = (int)row["SportID"],
                    SportName = row["SportName"].ToString()!,
                    MinGroupSize = (int)row["MinGroupSize"],
                    MaxGroupSize = (int)row["MaxGroupSize"]
                });
            }

            return list;
        }

        // ── User Sports ─────────────────────────────────────────
        public static List<clsSport_BLL> GetUserSports(int userID)
        {
            DataTable dt = clsSportDAL.GetUserSports(userID);
            List<clsSport_BLL> list = new List<clsSport_BLL>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new clsSport_BLL
                {
                    SportID = (int)row["SportID"],
                    SportName = row["SportName"].ToString()!,
                    MinGroupSize = (int)row["MinGroupSize"],
                    MaxGroupSize = (int)row["MaxGroupSize"]
                });
            }

            return list;
        }

        public static bool AddUserSport(int userID, int sportID)
        {
            return clsSportDAL.AddUserSport(userID, sportID);
        }

        public static bool DeleteUserSport(int userID, int sportID)
        {
            return clsSportDAL.DeleteUserSport(userID, sportID);
        }
    }
}
