using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowUp2Move_DAL
{
    public static class clsPollDAL
    {
        
        public static bool CreatePoll(int groupID, int createdByID, string question,
                                      DateTime? expiresAt, ref int newPollID)
        {
            newPollID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("USP_CreatePoll", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@GroupID", SqlDbType.Int).Value = groupID;
                command.Parameters.Add("@CreatedByID", SqlDbType.Int).Value = createdByID;
                command.Parameters.Add("@Question", SqlDbType.NVarChar, 300).Value = question;

                SqlParameter expiresParam = command.Parameters.Add("@ExpiresAt", SqlDbType.DateTime);
                expiresParam.Value = expiresAt.HasValue ? (object)expiresAt.Value : DBNull.Value;

                SqlParameter outputID = new SqlParameter("@NewPollID", SqlDbType.Int);
                outputID.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputID);

                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    newPollID = (int)command.Parameters["@NewPollID"].Value;
                    return (int)returnValue.Value == 1;
                }
                catch { return false; }
            }
        }

        
        public static bool AddPollOption(int pollID, string optionText, ref int newOptionID)
        {
            newOptionID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("USP_AddPollOption", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@PollID", SqlDbType.Int).Value = pollID;
                command.Parameters.Add("@OptionText", SqlDbType.NVarChar, 200).Value = optionText;

                SqlParameter outputID = new SqlParameter("@NewOptionID", SqlDbType.Int);
                outputID.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputID);

                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    newOptionID = (int)command.Parameters["@NewOptionID"].Value;
                    return (int)returnValue.Value == 1;
                }
                catch { return false; }
            }
        }

        
        public static int VoteOnPoll(int pollID, int optionID, int userID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("USP_VoteOnPoll", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@PollID", SqlDbType.Int).Value = pollID;
                command.Parameters.Add("@OptionID", SqlDbType.Int).Value = optionID;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;

                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return (int)returnValue.Value;
                }
                catch { return 0; }
            }
        }

        
        public static DataTable GetPollResults(int pollID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("USP_GetPollResults", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@PollID", SqlDbType.Int).Value = pollID;

                try
                {
                    connection.Open();
                    dt.Load(command.ExecuteReader());
                }
                catch { }
            }

            return dt;
        }

        
        public static DataTable GetPollsByGroup(int groupID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("USP_GetPollsByGroup", connection))
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
