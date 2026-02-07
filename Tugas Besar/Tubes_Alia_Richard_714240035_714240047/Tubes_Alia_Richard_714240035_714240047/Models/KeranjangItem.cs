using System;

namespace Tubes_Alia_Richard_714240035_714240047.Models
{
    public class KeranjangItem
    {
        public int ProdukId { get; set; }
        public string NamaProduk { get; set; }
        public string NamaKategori { get; set; }
        public decimal Harga { get; set; }
        public int Jumlah { get; set; }
        public int StokTersedia { get; set; }
        public string Gambar { get; set; }

        public decimal Subtotal
        {
            get { return Harga * Jumlah; }
        }
    }
}
