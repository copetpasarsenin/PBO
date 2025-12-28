using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using P9_714240047.model;

namespace P9_714240047.controller
{
    class Transaksi
    {
        Koneksi koneksi = new Koneksi();

        // TAMPILKAN DATA (READ)
        public DataTable TampilTransaksi()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT t.id_transaksi, t.id_barang, b.nama_barang, b.harga, t.qty, t.total " +
                               "FROM t_transaksi t JOIN t_barang b ON t.id_barang = b.id_barang";
                koneksi.OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, koneksi.kon);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            finally { koneksi.CloseConnection(); }
            return dt;
        }

        // AMBIL DATA BARANG UNTUK COMBOBOX
        public DataTable LoadBarang()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT id_barang, nama_barang FROM t_barang";
                koneksi.OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, koneksi.kon);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            finally { koneksi.CloseConnection(); }
            return dt;
        }

        // AMBIL DETAIL BARANG (NAMA & HARGA)
        public void GetBarangDetail(string id, out string nama, out string harga)
        {
            nama = ""; harga = "0";
            try
            {
                string query = "SELECT nama_barang, harga FROM t_barang WHERE id_barang = @id";
                koneksi.OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, koneksi.kon);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    nama = reader["nama_barang"].ToString();
                    harga = reader["harga"].ToString();
                }
                reader.Close();
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            finally { koneksi.CloseConnection(); }
        }

        // CEK DUPLIKASI BARANG (VALIDASI)
        public bool CekBarangAda(string idBarang)
        {
            bool status = false;
            try
            {
                string query = "SELECT COUNT(*) FROM t_transaksi WHERE id_barang = @id";
                koneksi.OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, koneksi.kon);
                cmd.Parameters.AddWithValue("@id", idBarang);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0) status = true;
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            finally { koneksi.CloseConnection(); }
            return status;
        }

        // TAMBAH DATA (INSERT)
        public bool Insert(M_transaksi tr)
        {
            bool status = false;
            try
            {
                string query = "INSERT INTO t_transaksi (id_barang, qty, total) VALUES (@id_barang, @qty, @total)";
                koneksi.OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, koneksi.kon);
                cmd.Parameters.AddWithValue("@id_barang", tr.Id_barang);
                cmd.Parameters.AddWithValue("@qty", tr.Qty);
                cmd.Parameters.AddWithValue("@total", tr.Total);
                cmd.ExecuteNonQuery();
                status = true;
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            finally { koneksi.CloseConnection(); }
            return status;
        }

        // UBAH DATA (UPDATE) - INI YANG BARU
        public bool Update(M_transaksi tr, string id_transaksi)
        {
            bool status = false;
            try
            {
                string query = "UPDATE t_transaksi SET id_barang=@id_barang, qty=@qty, total=@total WHERE id_transaksi=@id";
                koneksi.OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, koneksi.kon);
                cmd.Parameters.AddWithValue("@id_barang", tr.Id_barang);
                cmd.Parameters.AddWithValue("@qty", tr.Qty);
                cmd.Parameters.AddWithValue("@total", tr.Total);
                cmd.Parameters.AddWithValue("@id", id_transaksi);
                cmd.ExecuteNonQuery();
                status = true;
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            finally { koneksi.CloseConnection(); }
            return status;
        }

        // HAPUS DATA (DELETE) - INI YANG BARU
        public bool Delete(string id_transaksi)
        {
            bool status = false;
            try
            {
                string query = "DELETE FROM t_transaksi WHERE id_transaksi = @id";
                koneksi.OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, koneksi.kon);
                cmd.Parameters.AddWithValue("@id", id_transaksi);
                cmd.ExecuteNonQuery();
                status = true;
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            finally { koneksi.CloseConnection(); }
            return status;
        }
    }
}