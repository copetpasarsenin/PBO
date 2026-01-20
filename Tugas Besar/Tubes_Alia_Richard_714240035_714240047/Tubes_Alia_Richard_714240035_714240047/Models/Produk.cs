using System;

namespace Tubes_Alia_Richard_714240035_714240047.Models
{
    public class Produk
    {
        public int ProdukId { get; set; }
        public string NamaProduk { get; set; }
        public int KategoriId { get; set; }
        public string NamaKategori { get; set; }
        public decimal Harga { get; set; }
        public int Stok { get; set; }
        public string Deskripsi { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}