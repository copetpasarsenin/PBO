-- Migration script untuk menambahkan kolom payment di tabel pesanan
-- Jalankan di phpMyAdmin atau MySQL client

USE `tubes_manajemen`;

-- Tambah kolom payment_method, midtrans_transaction_id, dan payment_status
ALTER TABLE `pesanan` 
ADD COLUMN `payment_method` VARCHAR(20) DEFAULT 'COD' AFTER `catatan`,
ADD COLUMN `midtrans_transaction_id` VARCHAR(100) NULL AFTER `payment_method`,
ADD COLUMN `payment_status` VARCHAR(20) DEFAULT 'Pending' AFTER `midtrans_transaction_id`;

-- Update existing records
UPDATE `pesanan` SET `payment_method` = 'COD', `payment_status` = 'Pending' WHERE `payment_method` IS NULL;
