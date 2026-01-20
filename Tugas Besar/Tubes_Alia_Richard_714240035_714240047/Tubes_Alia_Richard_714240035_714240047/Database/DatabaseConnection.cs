using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Tubes_Alia_Richard_714240035_714240047.Database
{
    public class DatabaseConnection
    {
        private string connectionString = "Server=localhost;Database=tubes_manajemen;User Id=root;Password=;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Database: " + ex.Message);
            }
            return dt;
        }

        public DataTable ExecuteQuery(string query, MySqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Database: " + ex.Message);
            }
            return dt;
        }

        public bool ExecuteNonQuery(string query)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Database: " + ex.Message);
                return false;
            }
        }

        public bool ExecuteNonQuery(string query, MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Database: " + ex.Message);
                return false;
            }
        }

        public object ExecuteScalar(string query)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Database: " + ex.Message);
                return null;
            }
        }

        public object ExecuteScalar(string query, MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Database: " + ex.Message);
                return null;
            }
        }
    }
}