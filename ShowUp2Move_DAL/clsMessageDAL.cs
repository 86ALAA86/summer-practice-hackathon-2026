using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowUp2Move_DAL
{
    public static class clsMessageDAL
    {
        
        public static bool SendMessage(int groupID, int senderID, string content, ref int newMessageID)
        {
            newMessageID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("USP_SendMessage", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@GroupID", SqlDbType.Int).Value = groupID;
                command.Parameters.Add("@SenderID", SqlDbType.Int).Value = senderID;
                command.Parameters.Add("@Content", SqlDbType.NVarChar, 1000).Value = content;

                SqlParameter outputID = new SqlParameter("@NewMessageID", SqlDbType.Int);
                outputID.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputID);

                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    newMessageID = (int)command.Parameters["@NewMessageID"].Value;
                    return (int)returnValue.Value == 1;
                }
                catch { return false; }
            }
        }

       
        public static DataTable GetMessagesByGroup(int groupID, int lastN = 50)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("USP_GetMessagesByGroup", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@GroupID", SqlDbType.Int).Value = groupID;
                command.Parameters.Add("@LastN", SqlDbType.Int).Value = lastN;

                try
                {
                    connection.Open();
                    dt.Load(command.ExecuteReader());
                }
                catch { }
            }

            return dt;
        }

       
        public static int MarkMessagesRead(int groupID, int userID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("USP_MarkMessagesRead", connection))
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
                    return (int)returnValue.Value;
                }
                catch { return 0; }
            }
        }
    }
}
