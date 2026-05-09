using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowUp2Move_DAL
{
    public static class clsNotificationDAL
    {
        
        public static bool AddNotification(int userID, string title, string body,
                                           string type, int? relatedID, ref int newNotifID)
        {
            newNotifID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("USP_AddNotification", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                command.Parameters.Add("@Title", SqlDbType.NVarChar, 150).Value = title;
                command.Parameters.Add("@Body", SqlDbType.NVarChar, 500).Value = body;
                command.Parameters.Add("@Type", SqlDbType.NVarChar, 50).Value = type;

                SqlParameter relatedParam = command.Parameters.Add("@RelatedID", SqlDbType.Int);
                relatedParam.Value = relatedID.HasValue ? (object)relatedID.Value : DBNull.Value;

                SqlParameter outputID = new SqlParameter("@NewNotifID", SqlDbType.Int);
                outputID.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputID);

                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    newNotifID = (int)command.Parameters["@NewNotifID"].Value;
                    return (int)returnValue.Value == 1;
                }
                catch { return false; }
            }
        }

       
        public static DataTable GetUserNotifications(int userID, bool unreadOnly = false)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("USP_GetUserNotifications", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                command.Parameters.Add("@UnreadOnly", SqlDbType.Bit).Value = unreadOnly;

                try
                {
                    connection.Open();
                    dt.Load(command.ExecuteReader());
                }
                catch { }
            }

            return dt;
        }

       
        public static bool MarkNotificationRead(int notificationID, int userID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("USP_MarkNotificationRead", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@NotificationID", SqlDbType.Int).Value = notificationID;
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

        
        public static int MarkAllNotificationsRead(int userID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("USP_MarkAllNotificationsRead", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

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
