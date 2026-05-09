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
    public class clsEvent_BLL
    {
        public int EventID { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string SportName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsManual { get; set; }
        public DateTime CreatedAt { get; set; }

        // ── Get All ─────────────────────────────────────────────
        public static List<clsEvent_BLL> GetAll()
        {
            DataTable dt = clsEventDAL.GetAllEvents();
            List<clsEvent_BLL> list = new List<clsEvent_BLL>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new clsEvent_BLL
                {
                    EventID = (int)row["EventID"],
                    CreatedBy = row["CreatedBy"].ToString()!,
                    SportName = row["SportName"].ToString()!,
                    Location = row["Location"] == DBNull.Value ? "" : row["Location"].ToString()!,
                    EventDate = (DateTime)row["EventDate"],
                    Description = row["Description"] == DBNull.Value ? "" : row["Description"].ToString()!,
                    IsManual = (bool)row["IsManual"],
                    CreatedAt = (DateTime)row["CreatedAt"]
                });
            }

            return list;
        }

        // ── Find ─────────────────────────────────────────────────
        public static clsEvent_BLL? Find(int eventID)
        {
            DataTable dt = clsEventDAL.GetEventByID(eventID);

            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];

            return new clsEvent_BLL
            {
                EventID = (int)row["EventID"],
                CreatedBy = row["CreatedBy"].ToString()!,
                SportName = row["SportName"].ToString()!,
                Location = row["Location"] == DBNull.Value ? "" : row["Location"].ToString()!,
                EventDate = (DateTime)row["EventDate"],
                Description = row["Description"] == DBNull.Value ? "" : row["Description"].ToString()!,
                IsManual = (bool)row["IsManual"],
                CreatedAt = (DateTime)row["CreatedAt"]
            };
        }

        // ── Add ──────────────────────────────────────────────────
        public static bool Add(int createdByUserID, int sportID, string location,
                               DateTime eventDate, string description, bool isManual,
                               ref int newEventID)
        {
            return clsEventDAL.CreateEvent(createdByUserID, sportID, location,
                                           eventDate, description, isManual, ref newEventID);
        }
    }
}
