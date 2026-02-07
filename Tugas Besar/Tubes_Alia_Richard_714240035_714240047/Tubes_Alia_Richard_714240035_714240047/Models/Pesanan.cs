using System;

namespace Tubes_Alia_Richard_714240035_714240047.Models
{
    public class Pesanan
    {
        public int PesananId { get; set; }
        public int UserId { get; set; }
        public string NamaUser { get; set; }
        public DateTime TanggalPesan { get; set; }
        public decimal TotalHarga { get; set; }
        public string Status { get; set; }
        public string AlamatPengiriman { get; set; }
        public string Catatan { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Payment fields
        public string PaymentMethod { get; set; } = "COD";  // COD or Midtrans
        public string MidtransTransactionId { get; set; }
        public string PaymentStatus { get; set; } = "Pending";  // Pending, Paid, Failed
    }
}
