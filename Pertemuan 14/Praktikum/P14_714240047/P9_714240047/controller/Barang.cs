using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using P9_714240047.model;

namespace P9_714240047.controller
{
    class Barang
    {
        Koneksi koneksi = new Koneksi();

        // 1. TAMPIL DATA (READ)
        public DataTable Tampil()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT * FROM t_barang";
                koneksi.OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, koneksi.kon);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                koneksi.CloseConnection();
            }
            return dt;
        }

        // 2. TAMBAH DATA (INSERT)
        public bool Insert(M_barang barang)
        {
            bool status = false;
            try
            {
                string query = "INSERT INTO t_barang (nama_barang, harga) VALUES (@nama, @harga)";
                koneksi.OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, koneksi.kon);
                cmd.Parameters.AddWithValue("@nama", barang.Nama_barang);
                cmd.Parameters.AddWithValue("@harga", barang.Harga);

                cmd.ExecuteNonQuery();
                status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                koneksi.CloseConnection();
            }
            return status;
        }

        // 3. UBAH DATA (UPDATE)
        public bool Update(M_barang barang, string id)
        {
            bool status = false;
            try
            {
                string query = "UPDATE t_barang SET nama_barang=@nama, harga=@harga WHERE id_barang=@id";
                koneksi.OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, koneksi.kon);
                cmd.Parameters.AddWithValue("@nama", barang.Nama_barang);
                cmd.Parameters.AddWithValue("@harga", barang.Harga);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
                status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                koneksi.CloseConnection();
            }
            return status;
        }

        // 4. HAPUS DATA (DELETE)
        public bool Delete(string id)
        {
            bool status = false;
            try
            {
                string query = "DELETE FROM t_barang WHERE id_barang=@id";
                koneksi.OpenConnection();
                MySqlCommand cmd = new MySqlCommand(query, koneksi.kon);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
                status = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                koneksi.CloseConnection();
            }
            return status;
        }
    }
}