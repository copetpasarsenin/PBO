using MySql.Data.MySqlClient;
using P9_714240047.controller;
using P9_714240047.model; // <--- Jika Langkah 1 benar, baris ini tidak akan error lagi
using System;
using System.Windows.Forms;

namespace P9_714240047
{
    public partial class Form1 : Form
    {
        Koneksi koneksi = new Koneksi();
        M_mahasiswa m_mhs = new M_mahasiswa();
        Mahasiswa mhs = new Mahasiswa();

        public Form1()
        {
            InitializeComponent();
        }

        public void ResetForm()
        {
            textboxNpm.Text = "";
            textboxNama.Text = "";
            comboBoxAngkatan.SelectedIndex = -1;
            textboxAlamat.Text = "";
            textboxEmail.Text = "";
            textboxNohp.Text = "";
        }

        public void Tampil()
        {
            DataMahasiswa.DataSource = koneksi.ShowData("SELECT * FROM t_mahasiswa");

            if (DataMahasiswa.Columns.Count > 0)
            {
                DataMahasiswa.Columns[0].HeaderText = "NPM";
                DataMahasiswa.Columns[1].HeaderText = "Nama";
                DataMahasiswa.Columns[2].HeaderText = "Angkatan";
                DataMahasiswa.Columns[3].HeaderText = "Alamat";
                DataMahasiswa.Columns[4].HeaderText = "Email";
                DataMahasiswa.Columns[5].HeaderText = "No HP";
                DataMahasiswa.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Tampil();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (textboxNpm.Text == "" || textboxNama.Text == "" || comboBoxAngkatan.SelectedIndex == -1)
            {
                MessageBox.Show("Data tidak boleh kosong", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                m_mhs.Npm = textboxNpm.Text;
                m_mhs.Nama = textboxNama.Text;
                m_mhs.Angkatan = comboBoxAngkatan.Text;
                m_mhs.Alamat = textboxAlamat.Text;
                m_mhs.Email = textboxEmail.Text;
                m_mhs.Nohp = textboxNohp.Text;

                mhs.Insert(m_mhs);
                ResetForm();
                Tampil();
            }
        }

        private void btnUbah_Click(object sender, EventArgs e)
        {
            if (textboxNpm.Text == "" || textboxNama.Text == "")
            {
                MessageBox.Show("Data tidak boleh kosong", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                m_mhs.Npm = textboxNpm.Text;
                m_mhs.Nama = textboxNama.Text;
                m_mhs.Angkatan = comboBoxAngkatan.Text;
                m_mhs.Alamat = textboxAlamat.Text;
                m_mhs.Email = textboxEmail.Text;
                m_mhs.Nohp = textboxNohp.Text;

                mhs.Update(m_mhs, m_mhs.Npm);
                ResetForm();
                Tampil();
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            DialogResult pesan = MessageBox.Show("Apakah yakin akan menghapus data ini?", "Perhatian", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (pesan == DialogResult.Yes)
            {
                mhs.Delete(textboxNpm.Text);
                ResetForm();
                Tampil();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ResetForm();
            Tampil();
        }

        // --- INI PERBAIKANNYA ---
        // Method ini ditambahkan kosong agar Designer tidak error
        private void DataMahasiswa_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kosongkan saja, kita pakai yang CellClick di bawah
        }

        // Logika yang benar ada di sini (sesuai modul)
        private void DataMahasiswa_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            
                textboxNpm.Text = DataMahasiswa.Rows[e.RowIndex].Cells[0].Value.ToString();
                textboxNama.Text = DataMahasiswa.Rows[e.RowIndex].Cells[1].Value.ToString();
                comboBoxAngkatan.Text = DataMahasiswa.Rows[e.RowIndex].Cells[2].Value.ToString();
                textboxAlamat.Text = DataMahasiswa.Rows[e.RowIndex].Cells[3].Value.ToString();
                textboxEmail.Text = DataMahasiswa.Rows[e.RowIndex].Cells[4].Value.ToString();
                textboxNohp.Text = DataMahasiswa.Rows[e.RowIndex].Cells[5].Value.ToString();
            
        }

        private void textboxCariData_TextChanged(object sender, EventArgs e)
        {
            // Cek apakah textboxCariData sesuai dengan nama textbox kamu (bisa jadi textBoxCariData huruf besar/kecil beda)
            string keyword = textboxCariData.Text.Trim();

            // Query pencarian data mahasiswa
            // Mencari berdasarkan NPM atau Nama
            string sql = "SELECT * FROM t_mahasiswa WHERE npm LIKE @param OR nama LIKE @param";

            // Panggil method ShowDataParam dari koneksi
            // Pastikan DataMahasiswa adalah nama DataGridView kamu
            DataMahasiswa.DataSource = koneksi.ShowDataParam(sql, new MySqlParameter("@param", "%" + keyword + "%"));
        }
    }
}