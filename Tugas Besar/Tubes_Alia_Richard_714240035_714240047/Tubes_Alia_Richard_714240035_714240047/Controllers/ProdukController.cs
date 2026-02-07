using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Tubes_Alia_Richard_714240035_714240047.Database;
using Tubes_Alia_Richard_714240035_714240047.Models;

namespace Tubes_Alia_Richard_714240035_714240047.Controllers
{
    public class ProdukController
    {
        private DatabaseConnection dbConnection = new DatabaseConnection();

        // Get All Produk dengan JOIN ke Kategori
        public List<Produk> GetAllProduk()
        {
            List<Produk> produkList = new List<Produk>();
            try
            {
                string query = @"SELECT p.produk_id, p.nama_produk, p.kategori_id, k.nama_kategori, 
                               p.harga, p.stok, p.deskripsi, p.gambar, p.created_at
                               FROM produk p
                               JOIN kategori k ON p.kategori_id = k.kategori_id
                               ORDER BY p.produk_id DESC";
                
                DataTable dt = dbConnection.ExecuteQuery(query);
                
                foreach (DataRow row in dt.Rows)
                {
                    produkList.Add(new Produk
                    {
                        ProdukId = Convert.ToInt32(row["produk_id"]),
                        NamaProduk = row["nama_produk"].ToString(),
                        KategoriId = Convert.ToInt32(row["kategori_id"]),
                        NamaKategori = row["nama_kategori"].ToString(),
                        Harga = Convert.ToDecimal(row["harga"]),
                        Stok = Convert.ToInt32(row["stok"]),
                        Deskripsi = row["deskripsi"].ToString(),
                        Gambar = row["gambar"] != DBNull.Value ? row["gambar"].ToString() : null,
                        CreatedAt = Convert.ToDateTime(row["created_at"])
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Get Produk: " + ex.Message);
            }
            return produkList;
        }

        // Get DataTable untuk GridView
        public DataTable GetProdukDataTable()
        {
            try
            {
                string query = @"SELECT p.produk_id AS 'ID', p.nama_produk AS 'Nama Produk', 
                               k.nama_kategori AS 'Kategori', p.harga AS 'Harga', 
                               p.stok AS 'Stok', p.deskripsi AS 'Deskripsi'
                               FROM produk p
                               JOIN kategori k ON p.kategori_id = k.kategori_id
                               ORDER BY p.produk_id DESC";
                
                return dbConnection.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return new DataTable();
            }
        }

        // Search Produk by Nama - SAFE dari SQL Injection
        public DataTable SearchProduk(string searchTerm)
        {
            try
            {
                string query = @"SELECT p.produk_id AS 'ID', p.nama_produk AS 'Nama Produk', 
                               k.nama_kategori AS 'Kategori', p.harga AS 'Harga', 
                               p.stok AS 'Stok', p.deskripsi AS 'Deskripsi'
                               FROM produk p
                               JOIN kategori k ON p.kategori_id = k.kategori_id
                               WHERE p.nama_produk LIKE @search OR k.nama_kategori LIKE @search
                               ORDER BY p.produk_id DESC";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@search", "%" + searchTerm + "%")
                };

                return dbConnection.ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return new DataTable();
            }
        }

        // Filter by Kategori - SAFE dari SQL Injection
        public DataTable FilterByKategori(int kategoriId)
        {
            try
            {
                string query = @"SELECT p.produk_id AS 'ID', p.nama_produk AS 'Nama Produk', 
                               k.nama_kategori AS 'Kategori', p.harga AS 'Harga', 
                               p.stok AS 'Stok', p.deskripsi AS 'Deskripsi'
                               FROM produk p
                               JOIN kategori k ON p.kategori_id = k.kategori_id
                               WHERE p.kategori_id = @kategoriid
                               ORDER BY p.produk_id DESC";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@kategoriid", kategoriId)
                };

                return dbConnection.ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return new DataTable();
            }
        }

        // Get Statistics untuk Dashboard
        public Dictionary<string, object> GetStatistics()
        {
            Dictionary<string, object> stats = new Dictionary<string, object>();
            try
            {
                // Total Produk
                object totalProduk = dbConnection.ExecuteScalar("SELECT COUNT(*) FROM produk");
                stats["TotalProduk"] = Convert.ToInt32(totalProduk);

                // Total Kategori
                object totalKategori = dbConnection.ExecuteScalar("SELECT COUNT(*) FROM kategori");
                stats["TotalKategori"] = Convert.ToInt32(totalKategori);

                // Total Stok
                object totalStok = dbConnection.ExecuteScalar("SELECT SUM(stok) FROM produk");
                stats["TotalStok"] = totalStok == DBNull.Value ? 0 : Convert.ToInt32(totalStok);

                // Total Nilai Inventory (Harga * Stok)
                object totalNilai = dbConnection.ExecuteScalar("SELECT SUM(harga * stok) FROM produk");
                stats["TotalNilai"] = totalNilai == DBNull.Value ? 0 : Convert.ToDecimal(totalNilai);

                // Produk Stok Habis (= 0)
                object outOfStock = dbConnection.ExecuteScalar("SELECT COUNT(*) FROM produk WHERE stok = 0");
                stats["OutOfStock"] = Convert.ToInt32(outOfStock);

                // Produk Stok Rendah (1-9)
                object lowStock = dbConnection.ExecuteScalar("SELECT COUNT(*) FROM produk WHERE stok > 0 AND stok < 10");
                stats["LowStock"] = Convert.ToInt32(lowStock);

                // Harga Rata-rata
                object avgHarga = dbConnection.ExecuteScalar("SELECT AVG(harga) FROM produk");
                stats["AvgHarga"] = avgHarga == DBNull.Value ? 0 : Convert.ToDecimal(avgHarga);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Get Statistics: " + ex.Message);
            }
            return stats;
        }

        // Get Top Selling Products
        public DataTable GetTopProducts(int limit = 5)
        {
            try
            {
                string query = @"SELECT p.nama_produk AS 'Produk', 
                               k.nama_kategori AS 'Kategori',
                               COALESCE(SUM(dp.jumlah), 0) AS 'Terjual',
                               p.harga AS 'Harga'
                               FROM produk p
                               LEFT JOIN detail_pesanan dp ON p.produk_id = dp.produk_id
                               LEFT JOIN kategori k ON p.kategori_id = k.kategori_id
                               GROUP BY p.produk_id, p.nama_produk, k.nama_kategori, p.harga
                               ORDER BY Terjual DESC
                               LIMIT " + limit;
                
                return dbConnection.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Get Top Products: " + ex.Message);
                return new DataTable();
            }
        }

        // Create Produk - SAFE dari SQL Injection
        public bool CreateProduk(Produk produk)
        {
            try
            {
                string query = @"INSERT INTO produk (nama_produk, kategori_id, harga, stok, deskripsi, gambar) 
                                 VALUES (@nama, @kategoriid, @harga, @stok, @deskripsi, @gambar)";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@nama", produk.NamaProduk),
                    new MySqlParameter("@kategoriid", produk.KategoriId),
                    new MySqlParameter("@harga", produk.Harga),
                    new MySqlParameter("@stok", produk.Stok),
                    new MySqlParameter("@deskripsi", produk.Deskripsi),
                    new MySqlParameter("@gambar", string.IsNullOrEmpty(produk.Gambar) ? (object)DBNull.Value : produk.Gambar)
                };

                return dbConnection.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Create: " + ex.Message);
                return false;
            }
        }

        // Update Produk - SAFE dari SQL Injection
        public bool UpdateProduk(Produk produk)
        {
            try
            {
                string query = @"UPDATE produk SET nama_produk = @nama, kategori_id = @kategoriid, 
                                 harga = @harga, stok = @stok, deskripsi = @deskripsi, gambar = @gambar
                                 WHERE produk_id = @produkid";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@nama", produk.NamaProduk),
                    new MySqlParameter("@kategoriid", produk.KategoriId),
                    new MySqlParameter("@harga", produk.Harga),
                    new MySqlParameter("@stok", produk.Stok),
                    new MySqlParameter("@deskripsi", produk.Deskripsi),
                    new MySqlParameter("@gambar", string.IsNullOrEmpty(produk.Gambar) ? (object)DBNull.Value : produk.Gambar),
                    new MySqlParameter("@produkid", produk.ProdukId)
                };

                return dbConnection.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Update: " + ex.Message);
                return false;
            }
        }

        // Delete Produk - SAFE dari SQL Injection
        public bool DeleteProduk(int produkId)
        {
            try
            {
                string query = "DELETE FROM produk WHERE produk_id = @produkid";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@produkid", produkId)
                };

                return dbConnection.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Delete: " + ex.Message);
                return false;
            }
        }

        // Get Produk By ID
        public Produk GetProdukById(int produkId)
        {
            try
            {
                string query = @"SELECT p.produk_id, p.nama_produk, p.kategori_id, k.nama_kategori, 
                                 p.harga, p.stok, p.deskripsi, p.gambar
                                 FROM produk p
                                 JOIN kategori k ON p.kategori_id = k.kategori_id
                                 WHERE p.produk_id = @produkid";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@produkid", produkId)
                };
                
                DataTable dt = dbConnection.ExecuteQuery(query, parameters);
                
                if (dt.Rows.Count > 0)
                {
                    return new Produk
                    {
                        ProdukId = Convert.ToInt32(dt.Rows[0]["produk_id"]),
                        NamaProduk = dt.Rows[0]["nama_produk"].ToString(),
                        KategoriId = Convert.ToInt32(dt.Rows[0]["kategori_id"]),
                        NamaKategori = dt.Rows[0]["nama_kategori"].ToString(),
                        Harga = Convert.ToDecimal(dt.Rows[0]["harga"]),
                        Stok = Convert.ToInt32(dt.Rows[0]["stok"]),
                        Deskripsi = dt.Rows[0]["deskripsi"].ToString(),
                        Gambar = dt.Rows[0]["gambar"] != DBNull.Value ? dt.Rows[0]["gambar"].ToString() : null
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return null;
            }
        }
    }
}