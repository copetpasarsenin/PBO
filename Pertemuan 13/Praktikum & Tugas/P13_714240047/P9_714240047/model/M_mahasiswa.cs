using System;

namespace P9_714240047.model // <--- PASTIKAN ADA .model DI SINI
{
    public class M_mahasiswa // <--- Ubah jadi PUBLIC agar mudah terbaca
    {
        private string npm;
        private string nama;
        private string angkatan;
        private string alamat;
        private string email;
        private string nohp;

        public M_mahasiswa() { }

        public M_mahasiswa(string npm, string nama, string angkatan, string alamat, string email, string nohp)
        {
            this.Npm = npm;
            this.Nama = nama;
            this.Angkatan = angkatan;
            this.Alamat = alamat;
            this.Email = email;
            this.Nohp = nohp;
        }

        public string Npm { get => npm; set => npm = value; }
        public string Nama { get => nama; set => nama = value; }
        public string Angkatan { get => angkatan; set => angkatan = value; }
        public string Alamat { get => alamat; set => alamat = value; }
        public string Email { get => email; set => email = value; }
        public string Nohp { get => nohp; set => nohp = value; }
    }
}