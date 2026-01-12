using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using P9_714240047.model;

namespace P9_714240047.controller
{
    class Nilai
    {
        Koneksi koneksi = new Koneksi();

        // Method Insert
        public bool Insert(M_nilai nilai)
        {
            bool status = false;
            try
            {
                koneksi.OpenConnection();
                // Perhatikan: kita pakai koneksi.kon di sini
                MySqlCommand cmd = new MySqlCommand("INSERT INTO t_nilai (matkul, kategori, npm, nilai) VALUES (@matkul, @kategori, @npm, @nilai)", koneksi.kon);
                cmd.Parameters.AddWithValue("@matkul", nilai.Matkul);
                cmd.Parameters.AddWithValue("@kategori", nilai.Kategori);
                cmd.Parameters.AddWithValue("@npm", nilai.Npm);
                cmd.Parameters.AddWithValue("@nilai", nilai.Nilai);

                // PERBAIKAN: Gunakan ExecuteNonQuery milik cmd, BUKAN koneksi.ExecuteQuery
                cmd.ExecuteNonQuery();

                status = true;
                MessageBox.Show("Data berhasil ditambahkan", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                koneksi.CloseConnection();
            }
            catch (Exception e)
            {
                koneksi.CloseConnection();
                MessageBox.Show(e.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return status;
        }

        // Method Update
        public bool Update(M_nilai nilai, string id)
        {
            bool status = false;
            try
            {
                koneksi.OpenConnection();
                MySqlCommand cmd = new MySqlCommand("UPDATE t_nilai SET matkul=@matkul, kategori=@kategori, npm=@npm, nilai=@nilai WHERE id_nilai=@id", koneksi.kon);
                cmd.Parameters.AddWithValue("@matkul", nilai.Matkul);
                cmd.Parameters.AddWithValue("@kategori", nilai.Kategori);
                cmd.Parameters.AddWithValue("@npm", nilai.Npm);
                cmd.Parameters.AddWithValue("@nilai", nilai.Nilai);
                cmd.Parameters.AddWithValue("@id", id);

                // PERBAIKAN: Gunakan ExecuteNonQuery milik cmd
                cmd.ExecuteNonQuery();

                status = true;
                MessageBox.Show("Data berhasil diubah", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                koneksi.CloseConnection();
            }
            catch (Exception e)
            {
                koneksi.CloseConnection();
                MessageBox.Show(e.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return status;
        }

        // Method Delete
        public bool Delete(string id)
        {
            bool status = false;
            try
            {
                koneksi.OpenConnection();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM t_nilai WHERE id_nilai=@id", koneksi.kon);
                cmd.Parameters.AddWithValue("@id", id);

                // PERBAIKAN: Gunakan ExecuteNonQuery milik cmd
                cmd.ExecuteNonQuery();

                status = true;
                MessageBox.Show("Data berhasil dihapus", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                koneksi.CloseConnection();
            }
            catch (Exception e)
            {
                koneksi.CloseConnection();
                MessageBox.Show(e.Message, "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return status;
        }
    }
}