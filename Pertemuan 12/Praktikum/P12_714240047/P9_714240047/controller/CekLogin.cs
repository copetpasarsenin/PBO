using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace P9_714240047.controller
{
    class CekLogin
    {
        // Panggil class Koneksi kamu
        Koneksi koneksi = new Koneksi();

        public bool cek_login(string username, string password)
        {
            MySqlDataReader dr = null;
            try
            {
                koneksi.OpenConnection();
                // Query simpel cek user & pass
                string query = "SELECT 1 FROM t_user WHERE username=@username AND password=@password LIMIT 1";

                MySqlParameter[] param = new MySqlParameter[]
                {
                    new MySqlParameter("@username", username),
                    new MySqlParameter("@password", password)
                };

                dr = koneksi.reader(query, param);
                return dr.Read(); // True kalau ketemu
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (dr != null && !dr.IsClosed) dr.Close();
                koneksi.CloseConnection();
            }
        }
    }
}