using System;

namespace P9_714240047.model // Sesuaikan namespace
{
    public class M_nilai
    {
        string matkul, kategori, npm, nilai;

        public M_nilai() { }

        public M_nilai(string matkul, string kategori, string npm, string nilai)
        {
            this.Matkul = matkul;
            this.Kategori = kategori;
            this.Npm = npm;
            this.Nilai = nilai;
        }

        public string Matkul { get => matkul; set => matkul = value; }
        public string Kategori { get => kategori; set => kategori = value; }
        public string Npm { get => npm; set => npm = value; }
        public string Nilai { get => nilai; set => nilai = value; }
    }
}