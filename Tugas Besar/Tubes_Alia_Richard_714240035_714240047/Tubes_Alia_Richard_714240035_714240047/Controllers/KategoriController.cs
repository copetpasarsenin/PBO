using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Database;
using Tubes_Alia_Richard_714240035_714240047.Models;

namespace Tubes_Alia_Richard_714240035_714240047.Controllers
{
    public class KategoriController
    {
        private DatabaseConnection dbConnection = new DatabaseConnection();

        // Get All Kategori
        public List<Kategori> GetAllKategori()
        {
            List<Kategori> kategoriList = new List<Kategori>();
            try
            {
                string query = "SELECT * FROM kategori ORDER BY kategori_id DESC";
                DataTable dt = dbConnection.ExecuteQuery(query);
                
                foreach (DataRow row in dt.Rows)
                {
                    kategoriList.Add(new Kategori
                    {
                        KategoriId = Convert.ToInt32(row["kategori_id"]),
                        NamaKategori = row["nama_kategori"].ToString(),
                        Deskripsi = row["deskripsi"].ToString(),
                        CreatedAt = Convert.ToDateTime(row["created_at"])
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return kategoriList;
        }

        // Get DataTable untuk ComboBox
        public DataTable GetKategoriDataTable()
        {
            try
            {
                return dbConnection.ExecuteQuery("SELECT kategori_id AS ID, nama_kategori AS Nama FROM kategori");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return new DataTable();
            }
        }

        // Create Kategori
        public bool CreateKategori(Kategori kategori)
        {
            try
            {
                string query = $"INSERT INTO kategori (nama_kategori, deskripsi) VALUES ('{kategori.NamaKategori}', '{kategori.Deskripsi}')";
                return dbConnection.ExecuteNonQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        // Update Kategori
        public bool UpdateKategori(Kategori kategori)
        {
            try
            {
                string query = $"UPDATE kategori SET nama_kategori = '{kategori.NamaKategori}', deskripsi = '{kategori.Deskripsi}' WHERE kategori_id = {kategori.KategoriId}";
                return dbConnection.ExecuteNonQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        // Delete Kategori
        public bool DeleteKategori(int kategoriId)
        {
            try
            {
                string query = $"DELETE FROM kategori WHERE kategori_id = {kategoriId}";
                return dbConnection.ExecuteNonQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }
    }
}