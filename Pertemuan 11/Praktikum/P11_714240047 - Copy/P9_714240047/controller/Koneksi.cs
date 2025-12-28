using MySql.Data.MySqlClient; // Library ini WAJIB di paling atas
using System;
using System.Data;
using System.Windows.Forms;

namespace P9_714240047.controller
{
    class Koneksi
    {
        // Sesuaikan string koneksi ini dengan password database kamu jika ada
        string connectionString = "Server=localhost;Database=pemrog2ulbi;Uid=root;Pwd=;";
        public MySqlConnection kon;

        public Koneksi()
        {
            kon = new MySqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            if (kon.State == ConnectionState.Closed)
            {
                kon.Open();
            }
        }

        public void CloseConnection()
        {
            if (kon.State == ConnectionState.Open)
            {
                kon.Close();
            }
        }

        public void ExecuteQuery(string query)
        {
            MySqlCommand command = new MySqlCommand(query, kon);
            OpenConnection();
            command.ExecuteNonQuery();
            CloseConnection();
        }

        public object ShowData(string query)
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, connectionString);
            DataSet data = new DataSet();
            adapter.Fill(data);
            object ret = data.Tables[0];
            return ret;
        }

        // Method Tambahan untuk Praktikum 11 (Search & Parameter)
        public object ShowDataParam(string query, params MySqlParameter[] parameters)
        {
            OpenConnection();
            MySqlCommand cmd = new MySqlCommand(query, kon);
            cmd.Parameters.AddRange(parameters);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            CloseConnection();
            return table;
        }

        // Method Tambahan untuk Praktikum 11 (Reader untuk ComboBox)
        public MySqlDataReader reader(string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, kon);
            // Note: Koneksi harus dibuka manual sebelum memanggil method ini di Form
            MySqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }
    }
}