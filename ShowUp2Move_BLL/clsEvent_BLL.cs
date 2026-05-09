using ShowUp2Move.DAL;
using ShowUp2Move_DAL;
using System.Data;

namespace ShowUp2Move.BLL
{
    public class clsEvent
    {
        public int EventID { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string SportName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsManual { get; set; }
        public DateTime CreatedAt { get; set; }

        public static List<clsEvent> GetAll()
        {
            DataTable dt = clsEventDAL.GetAllEvents();
            List<clsEvent> list = new List<clsEvent>();

            foreach (DataRow row in dt.Rows)
                list.Add(MapRow(row));

            return list;
        }

        public static clsEvent? Find(int eventID)
        {
            DataTable dt = clsEventDAL.GetEventByID(eventID);
            if (dt.Rows.Count == 0) return null;
            return MapRow(dt.Rows[0]);
        }

        public static bool Add(int createdByUserID, int sportID, string location,
                               DateTime eventDate, string description, bool isManual,
                               ref int newEventID, int? groupID = null)
        {
            return clsEventDAL.CreateEvent(createdByUserID, sportID, location,
                                           eventDate, description, isManual,
                                           ref newEventID, groupID);
        }

        private static clsEvent MapRow(DataRow row)
        {
            return new clsEvent
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
    }
}