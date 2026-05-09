using Microsoft.Data.SqlClient;
using ShowUp2Move_DAL;
using System.Data;

namespace ShowUp2Move.DAL
{
    public static class clsGroupDAL
    {
        public static DataTable GetAvailableUsersBySport(int sportID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("usp_GetAvailableUsersBySport", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@SportID", SqlDbType.Int).Value = sportID;

                try
                {
                    connection.Open();
                    dt.Load(command.ExecuteReader());
                }
                catch { }
            }

            return dt;
        }

        public static bool CreateGroup(int sportID, int captainUserID, ref int newGroupID)
        {
            newGroupID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("usp_CreateGroup", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@SportID", SqlDbType.Int).Value = sportID;
                command.Parameters.Add("@CaptainUserID", SqlDbType.Int).Value = captainUserID;

                SqlParameter outputID = new SqlParameter("@NewGroupID", SqlDbType.Int);
                outputID.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputID);

                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    newGroupID = (int)command.Parameters["@NewGroupID"].Value;
                    return (int)returnValue.Value == 1;
                }
                catch { return false; }
            }
        }

        public static bool AddGroupMember(int groupID, int userID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("usp_AddGroupMember", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@GroupID", SqlDbType.Int).Value = groupID;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;

                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return (int)returnValue.Value == 1;
                }
                catch { return false; }
            }
        }

        public static DataTable GetGroupsByUser(int userID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("usp_GetGroupsByUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;

                try
                {
                    connection.Open();
                    dt.Load(command.ExecuteReader());
                }
                catch { }
            }

            return dt;
        }

        public static DataTable GetGroupMembers(int groupID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("usp_GetGroupMembers", connection))
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

        public static bool ConfirmParticipation(int groupID, int userID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("usp_ConfirmParticipation", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@GroupID", SqlDbType.Int).Value = groupID;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;

                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return (int)returnValue.Value == 1;
                }
                catch { return false; }
            }
        }
    }
}