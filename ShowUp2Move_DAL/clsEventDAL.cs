using Microsoft.Data.SqlClient;
using ShowUp2Move_DAL;
using System.Data;

namespace ShowUp2Move.DAL
{
    public static class clsEventDAL
    {
        public static bool CreateEvent(int createdByUserID, int sportID, string location,
                                       DateTime eventDate, string description, bool isManual,
                                       ref int newEventID)
        {
            newEventID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("usp_CreateEvent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = createdByUserID;
                command.Parameters.Add("@SportID", SqlDbType.Int).Value = sportID;
                command.Parameters.Add("@Location", SqlDbType.NVarChar, 255).Value = location;
                command.Parameters.Add("@EventDate", SqlDbType.DateTime).Value = eventDate;
                command.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = description;
                command.Parameters.Add("@IsManual", SqlDbType.Bit).Value = isManual;

                SqlParameter outputID = new SqlParameter("@NewEventID", SqlDbType.Int);
                outputID.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputID);

                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    newEventID = (int)command.Parameters["@NewEventID"].Value;
                    return (int)returnValue.Value == 1;
                }
                catch { return false; }
            }
        }

        public static DataTable GetAllEvents()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("usp_GetAllEvents", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                try
                {
                    connection.Open();
                    dt.Load(command.ExecuteReader());
                }
                catch { }
            }

            return dt;
        }

        public static DataTable GetEventByID(int eventID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("usp_GetEventByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@EventID", SqlDbType.Int).Value = eventID;

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