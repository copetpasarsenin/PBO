using MySql.Data.MySqlClient;
using P9_714240047.controller;
using P9_714240047.model; // Panggil folder model
using System;
using System.Windows.Forms;

namespace P9_714240047.controller
{
    internal class Mahasiswa
    {
        // Panggil class Koneksi
        Koneksi koneksi = new Koneksi();

        // 1. METHOD INSERT [cite: 269]
        public bool Insert(M_mahasiswa mahasiswa)
        {
            bool status = false;
            try
            {
                koneksi.OpenConnection();
                var cmd = new MySqlCommand("INSERT INTO t_mahasiswa (npm, nama, angkatan, alamat, email, nohp) VALUES (@npm, @nama, @angkatan, @alamat, @email, @nohp)", koneksi.kon);

                cmd.Parameters.AddWithValue("@npm", mahasiswa.Npm);
                cmd.Parameters.AddWithValue("@nama", mahasiswa.Nama);
                cmd.Parameters.AddWithValue("@angkatan", mahasiswa.Angkatan);
                cmd.Parameters.AddWithValue("@alamat", mahasiswa.Alamat);
                cmd.Parameters.AddWithValue("@email", mahasiswa.Email);
                cmd.Parameters.AddWithValue("@nohp", mahasiswa.Nohp);

                koneksi.ExecuteQuery(cmd);
                status = true;
                MessageBox.Show("Data berhasil ditambahkan", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Gagal Insert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                koneksi.CloseConnection();
            }
            return status;
        }

        // 2. METHOD UPDATE 
        // Perhatikan ada parameter tambahan 'npm_mhs' untuk klausa WHERE
        public bool Update(M_mahasiswa mahasiswa, string npm_mhs)
        {
            bool status = false;
            try
            {
                koneksi.OpenConnection();
                var cmd = new MySqlCommand("UPDATE t_mahasiswa SET nama=@nama, angkatan=@angkatan, alamat=@alamat, email=@email, nohp=@nohp WHERE npm=@npm", koneksi.kon);

                cmd.Parameters.AddWithValue("@npm", npm_mhs); // Menggunakan npm_mhs untuk WHERE
                cmd.Parameters.AddWithValue("@nama", mahasiswa.Nama);
                cmd.Parameters.AddWithValue("@angkatan", mahasiswa.Angkatan);
                cmd.Parameters.AddWithValue("@alamat", mahasiswa.Alamat);
                cmd.Parameters.AddWithValue("@email", mahasiswa.Email);
                cmd.Parameters.AddWithValue("@nohp", mahasiswa.Nohp);

                koneksi.ExecuteQuery(cmd);
                status = true;
                MessageBox.Show("Data berhasil diubah", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Gagal Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                koneksi.CloseConnection();
            }
            return status;
        }

        // 3. METHOD DELETE 
        public bool Delete(string npm_mhs)
        {
            bool status = false;
            try
            {
                koneksi.OpenConnection();
                var cmd = new MySqlCommand("DELETE FROM t_mahasiswa WHERE npm=@npm", koneksi.kon);
                cmd.Parameters.AddWithValue("@npm", npm_mhs);

                koneksi.ExecuteQuery(cmd);
                status = true;
                MessageBox.Show("Data berhasil dihapus", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Gagal Hapus", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                koneksi.CloseConnection();
            }
            return status;
        }
    }
}