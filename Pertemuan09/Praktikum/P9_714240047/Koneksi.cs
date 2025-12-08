using System.Data;
using MySql.Data.MySqlClient; // Jangan lupa using ini

namespace P9_714240047
{
    internal class Koneksi
    {
        // Connection string sesuai modul
        string connectionstring = "Server=localhost; Database=pemrog2ulbi; Uid=root; Pwd=;";
        MySqlConnection kon;

        public void OpenConnection()
        {
            kon = new MySqlConnection(connectionstring);
            kon.Open();
        }

        public void CloseConnection()
        {
            kon.Close();
        }

        public object ShowData(string query)
        {
            OpenConnection();
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, kon);
            DataTable table = new DataTable();
            adapter.Fill(table);
            CloseConnection();
            return table;
        }
    }
}