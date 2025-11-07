using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4_2_714240047
{
    // ===== 1.a. ABSTRACTION =====
    public abstract class Karyawan
    {
        // ===== 1.d. ENCAPSULATION (Field dan Property) =====
        private string _nik;

        // 'property' publik untuk mengakses dan mengatur _nik.
        // Ini menyembunyikan detail implementasi (enkapsulasi).
        public string NIK
        {
            get { return _nik; }
            set { _nik = value; }
        }

        // Property auto-implemented (cara singkat untuk enkapsulasi)
        public string Nama { get; set; }

        // ===== 1.e. CONSTRUCTOR =====
        public Karyawan(string nik, string nama)
        {
            this._nik = nik;
            this.Nama = nama;
            Console.WriteLine($"Karyawan {nama} (NIK: {nik}) telah terdaftar.");
        }

        // Method abstrak (Abstraction)
        // Semua kelas turunan WAJIB mengimplementasikan (override) method ini.
        public abstract double HitungGaji();
    }

    // ===== 1.b. INHERITANCE (Pewarisan) =====
    public class KaryawanTetap : Karyawan
    {
        public double GajiBulanan { get; set; }

        // Constructor untuk 'KaryawanTetap'
        // Ia memanggil constructor 'base' (Karyawan) terlebih dahulu.
        public KaryawanTetap(string nik, string nama, double gajiBulanan)
            : base(nik, nama) // Memanggil constructor Karyawan
        {
            this.GajiBulanan = gajiBulanan;
        }

        // ===== 1.c. POLYMORPHISM (Override) =====
        // Mengimplementasikan method abstrak dari 'Karyawan'.
        public override double HitungGaji()
        {
            // Gaji karyawan tetap adalah gaji bulanannya.
            return GajiBulanan;
        }
    }

    // ===== 1.b. INHERITANCE (Pewarisan) =====
    public class KaryawanHarian : Karyawan
    {
        public double UpahPerHari { get; set; }
        public int JumlahHariKerja { get; set; }

        // Constructor untuk 'KaryawanHarian'
        public KaryawanHarian(string nik, string nama, double upahPerHari, int jumlahHariKerja)
            : base(nik, nama) // Memanggil constructor Karyawan
        {
            this.UpahPerHari = upahPerHari;
            this.JumlahHariKerja = jumlahHariKerja;
        }

        // ===== 1.c. POLYMORPHISM (Override) =====
        // Mengimplementasikan method abstrak 'HitungGaji'.
        public override double HitungGaji()
        {
            // Gaji karyawan harian = upah * jumlah hari
            return UpahPerHari * JumlahHariKerja;
        }
    }

    // ----- Kelas Utama untuk Menjalankan Program -----
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Membuat Objek ---");

            // Membuat objek dari kelas turunan
            KaryawanTetap karyawan1 = new KaryawanTetap("71424001", "Syahira", 5000000);
            KaryawanHarian karyawan2 = new KaryawanHarian("71424002", "Putri", 150000, 20);
            KaryawanTetap karyawan3 = new KaryawanTetap("71424003", "Parista", 6000000);

            // ===== 1.c. POLYMORPHISM (Banyak Bentuk) =====
            List<Karyawan> daftarKaryawan = new List<Karyawan>();
            daftarKaryawan.Add(karyawan1);
            daftarKaryawan.Add(karyawan2);
            daftarKaryawan.Add(karyawan3);

            Console.WriteLine("\n--- Menghitung Gaji (Polymorphism) ---");

            // Looping melalui list
            foreach (Karyawan k in daftarKaryawan)
            {
                // Program akan secara otomatis memanggil method HitungGaji()
                // yang sesuai dengan tipe objek aslinya (Tetap atau Harian).
                // Inilah inti dari Polymorphism.
                Console.WriteLine($"Gaji untuk {k.Nama}: {k.HitungGaji():C}");
            }

            Console.ReadKey(); // Jeda agar console tidak langsung tertutup
        }
    }
}