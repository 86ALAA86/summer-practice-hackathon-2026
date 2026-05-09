using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowUp2Move_DAL
{
    public static class clsVenueDAL
    {
        
        public static bool AddVenueSuggestion(int groupID, int suggestedBy, string venueName,
                                              string? address, double? latitude, double? longitude,
                                              decimal? priceEst, string? notes, ref int newVenueID)
        {
            newVenueID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("USP_AddVenueSuggestion", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@GroupID", SqlDbType.Int).Value = groupID;
                command.Parameters.Add("@SuggestedBy", SqlDbType.Int).Value = suggestedBy;
                command.Parameters.Add("@VenueName", SqlDbType.NVarChar, 200).Value = venueName;

                SqlParameter addressParam = command.Parameters.Add("@Address", SqlDbType.NVarChar, 300);
                addressParam.Value = address is not null ? address : DBNull.Value;

                SqlParameter latParam = command.Parameters.Add("@Latitude", SqlDbType.Float);
                latParam.Value = latitude.HasValue ? (object)latitude.Value : DBNull.Value;

                SqlParameter lonParam = command.Parameters.Add("@Longitude", SqlDbType.Float);
                lonParam.Value = longitude.HasValue ? (object)longitude.Value : DBNull.Value;

                SqlParameter priceParam = command.Parameters.Add("@PriceEst", SqlDbType.Decimal);
                priceParam.Precision = 10;
                priceParam.Scale = 2;
                priceParam.Value = priceEst.HasValue ? (object)priceEst.Value : DBNull.Value;

                SqlParameter notesParam = command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500);
                notesParam.Value = notes is not null ? notes : DBNull.Value;

                SqlParameter outputID = new SqlParameter("@NewVenueID", SqlDbType.Int);
                outputID.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputID);

                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    newVenueID = (int)command.Parameters["@NewVenueID"].Value;
                    return (int)returnValue.Value == 1;
                }
                catch { return false; }
            }
        }

       
        public static DataTable GetVenueSuggestions(int groupID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("USP_GetVenueSuggestions", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@GroupID", SqlDbType.Int).Value = groupID;

                try
                {
                    connection.Open();
                    dt.Load(command.ExecuteReader());
                }
                catch { }
            }

            return dt;
        }
    }
}
