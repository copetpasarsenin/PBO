using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Tubes_Alia_Richard_714240035_714240047.Database;
using Tubes_Alia_Richard_714240035_714240047.Models;

namespace Tubes_Alia_Richard_714240035_714240047.Controllers
{
    public class UserController
    {
        private DatabaseConnection dbConnection = new DatabaseConnection();

        // Login User - SAFE dari SQL Injection
        public User LoginUser(string username, string password)
        {
            try
            {
                string query = "SELECT * FROM users WHERE username = @username AND password = @password AND is_active = 1";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@username", username),
                    new MySqlParameter("@password", password)
                };

                DataTable dt = dbConnection.ExecuteQuery(query, parameters);
                
                if (dt.Rows.Count > 0)
                {
                    return new User
                    {
                        UserId = Convert.ToInt32(dt.Rows[0]["user_id"]),
                        Username = dt.Rows[0]["username"].ToString(),
                        Email = dt.Rows[0]["email"].ToString(),
                        FullName = dt.Rows[0]["full_name"].ToString(),
                        Role = dt.Rows[0]["role"].ToString(),
                        IsActive = Convert.ToBoolean(dt.Rows[0]["is_active"])
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Login: " + ex.Message);
                return null;
            }
        }

        // Register User - SAFE dari SQL Injection
        public bool RegisterUser(User user)
        {
            try
            {
                string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @username";
                MySqlParameter[] checkParam = new MySqlParameter[]
                {
                    new MySqlParameter("@username", user.Username)
                };
                
                object result = dbConnection.ExecuteScalar(checkQuery, checkParam);
                
                if (Convert.ToInt32(result) > 0)
                {
                    MessageBox.Show("Username sudah terdaftar!");
                    return false;
                }

                string query = @"INSERT INTO users (username, email, password, full_name, role) 
                                 VALUES (@username, @email, @password, @fullname, @role)";
                
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@username", user.Username),
                    new MySqlParameter("@email", user.Email),
                    new MySqlParameter("@password", user.Password),
                    new MySqlParameter("@fullname", user.FullName),
                    new MySqlParameter("@role", user.Role)
                };

                return dbConnection.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Register: " + ex.Message);
                return false;
            }
        }

        // Get User By Username
        public User GetUserByUsername(string username)
        {
            try
            {
                string query = "SELECT * FROM users WHERE username = @username";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@username", username)
                };

                DataTable dt = dbConnection.ExecuteQuery(query, parameters);
                
                if (dt.Rows.Count > 0)
                {
                    return new User
                    {
                        UserId = Convert.ToInt32(dt.Rows[0]["user_id"]),
                        Username = dt.Rows[0]["username"].ToString(),
                        Email = dt.Rows[0]["email"].ToString(),
                        FullName = dt.Rows[0]["full_name"].ToString(),
                        Role = dt.Rows[0]["role"].ToString()
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
        }

        // Change Password
        public bool ChangePassword(int userId, string oldPassword, string newPassword)
        {
            try
            {
                // Verify old password
                string verifyQuery = "SELECT COUNT(*) FROM users WHERE user_id = @userid AND password = @oldpass";
                MySqlParameter[] verifyParam = new MySqlParameter[]
                {
                    new MySqlParameter("@userid", userId),
                    new MySqlParameter("@oldpass", oldPassword)
                };

                object result = dbConnection.ExecuteScalar(verifyQuery, verifyParam);
                
                if (Convert.ToInt32(result) == 0)
                {
                    MessageBox.Show("Password lama tidak sesuai!");
                    return false;
                }

                // Update password
                string updateQuery = "UPDATE users SET password = @newpass WHERE user_id = @userid";
                MySqlParameter[] updateParam = new MySqlParameter[]
                {
                    new MySqlParameter("@newpass", newPassword),
                    new MySqlParameter("@userid", userId)
                };

                return dbConnection.ExecuteNonQuery(updateQuery, updateParam);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }
    }
}