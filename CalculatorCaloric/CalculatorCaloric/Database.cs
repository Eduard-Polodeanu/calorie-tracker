using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CalculatorCaloric
{
    public static class Database
    {
        private static string connectionString = "Server=DESKTOP-U4TTGIU\\SQLEXPRESS;Database=CalorieTrackerDB;Trusted_Connection=true";


        public static bool CheckCredentials(string username, string pass)
        {
            string passHash = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT LoginDetails.PassHash " +
                        "FROM Users JOIN LoginDetails ON Users.userID = LoginDetails.userID " +
                        "WHERE Users.username = @username";

                    using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@username", username);
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                passHash = reader.GetString(0);
                            }
                        }
                    }
                       
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Calorie tracker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (passHash == pass) return true;    
            return false;
        }

        public static void AddNewUserToDB(string username, string pass, string email)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    int userId;
                    connection.Open();

                    string query = "INSERT INTO Users (Username, Email) VALUES (@username, @email); SELECT SCOPE_IDENTITY();";
                    using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@username", username);
                        sqlCommand.Parameters.AddWithValue("@email", email);

                        // Execute the INSERT statement and retrieve the newly generated ID
                        userId = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    }

                    query = "INSERT INTO LoginDetails (UserId, PassHash) VALUES (@userId, @passHash)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@passHash", pass);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Calorie tracker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool isValueInTable(string tableName, string columnName, string value)
        {
            int count = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM " + tableName + " WHERE " + columnName + " = @searchedValue";
                    using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                    {
                        sqlCommand.Parameters.AddWithValue("@searchedValue", value);

                        count = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Calorie tracker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return count > 0;
        }


    }
}
