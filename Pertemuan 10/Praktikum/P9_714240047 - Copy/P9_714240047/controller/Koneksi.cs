using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace P9_714240047.controller
{
    internal class Koneksi
    {
        string connectionstring = "Server=localhost; Database=pemrog2ulbi; Uid=root; Pwd=;";
        public MySqlConnection kon;

        public void OpenConnection()
        {
            try
            {
                kon = new MySqlConnection(connectionstring);
                kon.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Koneksi: " + ex.Message);
            }
        }

        public void CloseConnection()
        {
            if (kon != null && kon.State == ConnectionState.Open)
            {
                kon.Close();
            }
        }

        public DataTable ShowData(string query)
        {
            DataTable table = new DataTable();
            try
            {
                OpenConnection();
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, kon);
                adapter.Fill(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ShowData: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return table;
        }

        public void ExecuteQuery(MySqlCommand command)
        {
            command.Connection = kon;
            command.ExecuteNonQuery();
        }
    }
}