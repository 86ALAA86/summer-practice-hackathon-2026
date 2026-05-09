using System.Data;
using ShowUp2Move_DAL;

namespace ShowUp2Move.BLL
{
    public class clsVenue
    {
        public int VenueID { get; set; }
        public string VenueName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public decimal? PriceEst { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string SuggestedBy { get; set; } = string.Empty;

        public static List<clsVenue> GetByGroup(int groupID)
        {
            DataTable dt = clsVenueDAL.GetVenueSuggestions(groupID);
            List<clsVenue> list = new List<clsVenue>();

            foreach (DataRow row in dt.Rows)
                list.Add(new clsVenue
                {
                    VenueID = (int)row["VenueID"],
                    VenueName = row["VenueName"].ToString()!,
                    Address = row["Address"] == DBNull.Value ? "" : row["Address"].ToString()!,
                    Latitude = row["Latitude"] == DBNull.Value ? null : (double?)row["Latitude"],
                    Longitude = row["Longitude"] == DBNull.Value ? null : (double?)row["Longitude"],
                    PriceEst = row["PriceEst"] == DBNull.Value ? null : (decimal?)row["PriceEst"],
                    Notes = row["Notes"] == DBNull.Value ? "" : row["Notes"].ToString()!,
                    SuggestedBy = row["SuggestedBy"].ToString()!
                });

            return list;
        }

        public static bool Add(int groupID, int suggestedBy, string venueName,
                               string? address, double? latitude, double? longitude,
                               decimal? priceEst, string? notes, ref int newVenueID)
            => clsVenueDAL.AddVenueSuggestion(groupID, suggestedBy, venueName,
                                               address, latitude, longitude,
                                               priceEst, notes, ref newVenueID);
    }
}