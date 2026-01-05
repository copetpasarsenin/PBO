using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using P9_714240047.controller;
using P9_714240047.model;
using P9_714240047.lib;

namespace P9_714240047
{
    public partial class FormNilai : Form
    {
        Koneksi koneksi = new Koneksi();
        M_nilai m_nilai = new M_nilai();
        string id_nilai;

        public FormNilai()
        {
            InitializeComponent();
        }

        public void Tampil()
        {
            string sql = "SELECT id_nilai, matkul, kategori, t_nilai.npm, nama, nilai " +
                         "FROM t_nilai JOIN t_mahasiswa ON t_mahasiswa.npm = t_nilai.npm";

            DataNilai.DataSource = koneksi.ShowData(sql);

            DataNilai.Columns[0].HeaderText = "ID";
            DataNilai.Columns[1].HeaderText = "Matkul";
            DataNilai.Columns[2].HeaderText = "Kategori";
            DataNilai.Columns[3].HeaderText = "NPM";
            DataNilai.Columns[4].HeaderText = "Nama";
            DataNilai.Columns[5].HeaderText = "Nilai";
        }

        public void GetDataMhs()
        {
            checkBoxNpm.Items.Clear();
            koneksi.OpenConnection();
            MySqlDataReader reader = koneksi.reader("SELECT npm FROM t_mahasiswa");
            while (reader.Read())
            {
                checkBoxNpm.Items.Add(reader["npm"].ToString());
            }
            reader.Close();
            koneksi.CloseConnection();
        }

        public void GetNamaMhs()
        {
            if (string.IsNullOrWhiteSpace(checkBoxNpm.Text)) return;

            string sql = "SELECT nama FROM t_mahasiswa WHERE npm = @npm";
            DataTable dt = (DataTable)koneksi.ShowDataParam(sql, new MySqlParameter("@npm", checkBoxNpm.Text));

            if (dt.Rows.Count > 0)
            {
                textBoxNama.Text = dt.Rows[0]["nama"].ToString();
            }
        }

        public void ResetForm()
        {
            checkBoxMatkul.SelectedIndex = -1;
            checkBoxKategori.SelectedIndex = -1;
            checkBoxNpm.SelectedIndex = -1;
            textBoxNilai.Text = "";
            textBoxNama.Text = "";
            textBoxCariData.Text = "";
            id_nilai = "";
        }

        private void FormNilai_Load(object sender, EventArgs e)
        {
            Tampil();
            GetDataMhs();
        }

        private void checkBoxNpm_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetNamaMhs();
        }

        private void textBoxCariData_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBoxCariData.Text.Trim();

            if (keyword == "")
            {
                Tampil();
                return;
            }

            string sql = "SELECT id_nilai, matkul, kategori, t_nilai.npm, nama, nilai " +
                         "FROM t_nilai JOIN t_mahasiswa ON t_mahasiswa.npm = t_nilai.npm " +
                         "WHERE CAST(t_nilai.npm AS CHAR) LIKE @param OR nama LIKE @param OR matkul LIKE @param";

            DataNilai.DataSource = koneksi.ShowDataParam(sql, new MySqlParameter("@param", "%" + keyword + "%"));
        }

        private void DataNilai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            id_nilai = DataNilai.Rows[e.RowIndex].Cells[0].Value.ToString();
            checkBoxMatkul.Text = DataNilai.Rows[e.RowIndex].Cells[1].Value.ToString();
            checkBoxKategori.Text = DataNilai.Rows[e.RowIndex].Cells[2].Value.ToString();
            checkBoxNpm.Text = DataNilai.Rows[e.RowIndex].Cells[3].Value.ToString();
            textBoxNilai.Text = DataNilai.Rows[e.RowIndex].Cells[5].Value.ToString();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ResetForm();
            Tampil();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (checkBoxMatkul.SelectedIndex == -1 || checkBoxKategori.SelectedIndex == -1 || checkBoxNpm.SelectedIndex == -1 || textBoxNilai.Text == "")
            {
                MessageBox.Show("Data tidak boleh kosong", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Nilai nilai = new Nilai();
                m_nilai.Matkul = checkBoxMatkul.Text;
                m_nilai.Kategori = checkBoxKategori.Text;
                m_nilai.Npm = checkBoxNpm.Text;
                m_nilai.Nilai = textBoxNilai.Text;

                nilai.Insert(m_nilai);
                ResetForm();
                Tampil();
            }
        }

        private void btnUbah_Click(object sender, EventArgs e)
        {
            if (checkBoxMatkul.SelectedIndex == -1 || checkBoxKategori.SelectedIndex == -1 || checkBoxNpm.SelectedIndex == -1 || textBoxNilai.Text == "")
            {
                MessageBox.Show("Data tidak boleh kosong", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Nilai nilai = new Nilai();
                m_nilai.Matkul = checkBoxMatkul.Text;
                m_nilai.Kategori = checkBoxKategori.Text;
                m_nilai.Npm = checkBoxNpm.Text;
                m_nilai.Nilai = textBoxNilai.Text;

                nilai.Update(m_nilai, id_nilai);
                ResetForm();
                Tampil();
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            DialogResult pesan = MessageBox.Show("Apakah yakin akan menghapus data ini?", "Perhatian", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (pesan == DialogResult.Yes)
            {
                Nilai nilai = new Nilai();
                nilai.Delete(id_nilai);
                ResetForm();
                Tampil();
            }
        }

        // --- BAGIAN EXPORT EXCEL (SESUAI PRAKTIKUM 13) ---
        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel Documents (*.xlsx)|*.xlsx";
            save.FileName = "Report Nilai.xlsx";
            save.OverwritePrompt = false;

            if (save.ShowDialog() == DialogResult.OK)
            {
                string filePath = save.FileName;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                Excel excel_lib = new Excel();
                excel_lib.ExportToExcel(DataNilai, filePath);

                MessageBox.Show(
                    "Data berhasil diekspor ke file Excel.",
                    "Informasi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}