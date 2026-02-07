using System;

namespace Tubes_Alia_Richard_714240035_714240047.Models
{
    public class DetailPesanan
    {
        public int DetailId { get; set; }
        public int PesananId { get; set; }
        public int ProdukId { get; set; }
        public string NamaProduk { get; set; }
        public decimal Harga { get; set; }
        public int Jumlah { get; set; }
        public decimal Subtotal { get; set; }
    }
}
