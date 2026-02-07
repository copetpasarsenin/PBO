-- =============================================
-- Database Migration - E-Commerce Extension
-- Untuk: tubes_manajemen
-- Dibuat: 2026-02-06
-- =============================================

-- Tabel Pesanan (Orders)
CREATE TABLE IF NOT EXISTS pesanan (
    pesanan_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    tanggal_pesan DATETIME DEFAULT CURRENT_TIMESTAMP,
    total_harga DECIMAL(15,2) NOT NULL,
    status ENUM('Pending','Diproses','Dikirim','Selesai','Dibatalkan') DEFAULT 'Pending',
    alamat_pengiriman TEXT NOT NULL,
    catatan TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tabel Detail Pesanan (Order Items)
CREATE TABLE IF NOT EXISTS detail_pesanan (
    detail_id INT AUTO_INCREMENT PRIMARY KEY,
    pesanan_id INT NOT NULL,
    produk_id INT NOT NULL,
    nama_produk VARCHAR(255) NOT NULL,
    harga DECIMAL(15,2) NOT NULL,
    jumlah INT NOT NULL,
    subtotal DECIMAL(15,2) NOT NULL,
    FOREIGN KEY (pesanan_id) REFERENCES pesanan(pesanan_id) ON DELETE CASCADE,
    FOREIGN KEY (produk_id) REFERENCES produk(produk_id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tambah kolom gambar ke tabel produk (opsional, untuk fitur gambar produk)
ALTER TABLE produk ADD COLUMN IF NOT EXISTS gambar VARCHAR(500) DEFAULT NULL;

-- Index untuk performa
CREATE INDEX idx_pesanan_user ON pesanan(user_id);
CREATE INDEX idx_pesanan_status ON pesanan(status);
CREATE INDEX idx_pesanan_tanggal ON pesanan(tanggal_pesan);
CREATE INDEX idx_detail_pesanan ON detail_pesanan(pesanan_id);

-- =============================================
-- Sample Data (Optional - untuk testing)
-- =============================================
-- INSERT INTO pesanan (user_id, total_harga, status, alamat_pengiriman, catatan)
-- VALUES (1, 150000.00, 'Pending', 'Jl. Contoh No. 123, Jakarta', 'Tolong packing rapi');
