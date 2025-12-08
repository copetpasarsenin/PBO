using P9_714240047;
using System;
using System.Windows.Forms;
// using MySql.Data.MySqlClient; // (Opsional jika dibutuhkan di form, tapi di sini kita pakai class Koneksi)

namespace P9_714240047
{
    public partial class Form1 : Form
    {
        // 1. Panggil class Koneksi
        Koneksi koneksi = new Koneksi();

        public Form1()
        {
            InitializeComponent();
        }

        // 2. Method Tampil (JAWABAN CHALLENGE HALAMAN 8)
        public void Tampil()
        {
            // Query untuk mengambil semua data dari tabel t_mahasiswa
            string query = "SELECT * FROM t_mahasiswa";

            // Panggil method ShowData dari class Koneksi dan masukkan ke DataGridView
            DataMahasiswa.DataSource = koneksi.ShowData(query);

            // 3. Mengubah Header Tabel (Sesuai Halaman 9, Langkah 19)
            // Pastikan DataGridView sudah ada datanya sebelum kode ini jalan
            if (DataMahasiswa.Columns.Count > 0)
            {
                DataMahasiswa.Columns[0].HeaderText = "NPM";
                DataMahasiswa.Columns[1].HeaderText = "Nama";
                DataMahasiswa.Columns[2].HeaderText = "Angkatan";
                DataMahasiswa.Columns[3].HeaderText = "Alamat";
                DataMahasiswa.Columns[4].HeaderText = "Email";
                DataMahasiswa.Columns[5].HeaderText = "No HP";

                // Agar kolom memenuhi lebar tabel (Langkah 20)
                DataMahasiswa.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        // 4. Panggil Tampil saat Form dimuat (Langkah 17)
        private void Form1_Load(object sender, EventArgs e)
        {
            Tampil();
        }

        // --- Tombol Refresh (Opsional, agar form lebih interaktif) ---
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Reset form
            textboxNpm.Clear();
            textboxNama.Clear();
            textboxAlamat.Clear();
            textboxEmail.Clear();
            textboxNohp.Clear();
            comboBoxAngkatan.SelectedIndex = -1;

            // Refresh DataGridView
            Tampil();
        }
    }
}