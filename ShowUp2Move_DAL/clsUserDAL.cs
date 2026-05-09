using Microsoft.Data.SqlClient;
using ShowUp2Move_DAL;
using System.Data;

namespace ShowUp2Move.DAL
{
    public static class clsUserDAL
    {
        public static bool AddUser(string username, string password, string fullName, ref int newUserID)
        {
            newUserID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("usp_AddUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = username;
                command.Parameters.Add("@Password", SqlDbType.NVarChar, 100).Value = password;
                command.Parameters.Add("@FullName", SqlDbType.NVarChar, 150).Value = fullName;

                SqlParameter outputID = new SqlParameter("@NewUserID", SqlDbType.Int);
                outputID.Direction = ParameterDirection.Output;
                command.Parameters.Add(outputID);

                SqlParameter returnValue = new SqlParameter();
                returnValue.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(returnValue);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    newUserID = (int)command.Parameters["@NewUserID"].Value;
                    return (int)returnValue.Value == 1;
                }
                catch { return false; }
            }
        }

        public static DataTable GetUserByCredentials(string username, string password)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("usp_GetUserByCredentials", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@Username", SqlDbType.NVarChar, 100).Value = username;
                command.Parameters.Add("@Password", SqlDbType.NVarChar, 100).Value = password;

                try
                {
                    connection.Open();
                    dt.Load(command.ExecuteReader());
                }
                catch { }
            }

            return dt;
        }

        public static DataTable GetUserByID(int userID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("usp_GetUserByID", connection))
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

        public static bool UpdateUserAvailability(int userID, bool isAvailable)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("usp_UpdateUserAvailability", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                command.Parameters.Add("@IsAvailableToday", SqlDbType.Bit).Value = isAvailable;

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

        public static bool UpdateUserProfile(int userID, string fullName, string description, string skillLevel)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("usp_UpdateUserProfile", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                command.Parameters.Add("@FullName", SqlDbType.NVarChar, 150).Value = fullName;
                command.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = description;
                command.Parameters.Add("@SkillLevel", SqlDbType.NVarChar, 50).Value = skillLevel;

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