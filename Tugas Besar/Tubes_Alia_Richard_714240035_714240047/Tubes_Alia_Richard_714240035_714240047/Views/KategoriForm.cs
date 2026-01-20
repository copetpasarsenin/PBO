using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Controllers;
using Tubes_Alia_Richard_714240035_714240047.Models;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public partial class KategoriForm : Form
    {
        private KategoriController kategoriController = new KategoriController();
        private int selectedKategoriId = 0;

        public KategoriForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Manajemen Kategori";
            this.Width = 900;
            this.Height = 600;
            this.BackColor = Color.White;

            // Panel Header
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = Color.FromArgb(41, 128, 185),
                Padding = new Padding(10)
            };

            Label lblHeader = new Label
            {
                Text = "MANAJEMEN KATEGORI",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Left = 10,
                Top = 5
            };

            pnlHeader.Controls.Add(lblHeader);

            // Panel Input
            Panel pnlInput = new Panel
            {
                Dock = DockStyle.Top,
                Height = 180,
                BackColor = Color.FromArgb(245, 245, 245),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15)
            };

            Label lblNama = new Label { Text = "Nama Kategori:", Left = 15, Top = 15, Font = new Font("Arial", 10, FontStyle.Bold) };
            TextBox txtNama = new TextBox { Left = 150, Top = 15, Width = 400, Height = 30, Font = new Font("Arial", 10) };

            Label lblDeskripsi = new Label { Text = "Deskripsi:", Left = 15, Top = 60, Font = new Font("Arial", 10, FontStyle.Bold) };
            TextBox txtDeskripsi = new TextBox { Left = 150, Top = 60, Width = 650, Height = 70, Multiline = true, Font = new Font("Arial", 10) };

            Button btnAdd = new Button 
            { 
                Text = "TAMBAH", 
                Left = 15, 
                Top = 140, 
                Width = 100, 
                Height = 35, 
                BackColor = Color.FromArgb(46, 204, 113), 
                ForeColor = Color.White, 
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.FlatAppearance.BorderSize = 0;

            Button btnUpdate = new Button 
            { 
                Text = "UPDATE", 
                Left = 125, 
                Top = 140, 
                Width = 100, 
                Height = 35, 
                BackColor = Color.FromArgb(52, 152, 219), 
                ForeColor = Color.White, 
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnUpdate.FlatAppearance.BorderSize = 0;

            Button btnDelete = new Button 
            { 
                Text = "HAPUS", 
                Left = 235, 
                Top = 140, 
                Width = 100, 
                Height = 35, 
                BackColor = Color.FromArgb(231, 76, 60), 
                ForeColor = Color.White, 
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnDelete.FlatAppearance.BorderSize = 0;

            Button btnClear = new Button 
            { 
                Text = "BERSIHKAN", 
                Left = 345, 
                Top = 140, 
                Width = 105, 
                Height = 35, 
                BackColor = Color.FromArgb(149, 165, 166), 
                ForeColor = Color.White, 
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnClear.FlatAppearance.BorderSize = 0;

            pnlInput.Controls.Add(lblNama);
            pnlInput.Controls.Add(txtNama);
            pnlInput.Controls.Add(lblDeskripsi);
            pnlInput.Controls.Add(txtDeskripsi);
            pnlInput.Controls.Add(btnAdd);
            pnlInput.Controls.Add(btnUpdate);
            pnlInput.Controls.Add(btnDelete);
            pnlInput.Controls.Add(btnClear);

            // DataGridView
            DataGridView dgvKategori = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BorderStyle = BorderStyle.FixedSingle
            };

            LoadKategoriData(dgvKategori);

            this.Controls.Add(dgvKategori);
            this.Controls.Add(pnlInput);
            this.Controls.Add(pnlHeader);

            // Event Handlers
            btnAdd.Click += (s, e) =>
            {
                if (ValidateInput(txtNama))
                {
                    var kategori = new Kategori
                    {
                        NamaKategori = txtNama.Text,
                        Deskripsi = txtDeskripsi.Text
                    };

                    if (kategoriController.CreateKategori(kategori))
                    {
                        MessageBox.Show("Data berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadKategoriData(dgvKategori);
                        ClearInputs();
                    }
                }
            };

            dgvKategori.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    selectedKategoriId = Convert.ToInt32(dgvKategori.Rows[e.RowIndex].Cells[0].Value);
                    txtNama.Text = dgvKategori.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txtDeskripsi.Text = dgvKategori.Rows[e.RowIndex].Cells[2].Value?.ToString() ?? "";
                }
            };

            btnUpdate.Click += (s, e) =>
            {
                if (selectedKategoriId > 0 && ValidateInput(txtNama))
                {
                    var kategori = new Kategori
                    {
                        KategoriId = selectedKategoriId,
                        NamaKategori = txtNama.Text,
                        Deskripsi = txtDeskripsi.Text
                    };

                    if (kategoriController.UpdateKategori(kategori))
                    {
                        MessageBox.Show("Data berhasil diupdate!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadKategoriData(dgvKategori);
                        ClearInputs();
                    }
                }
                else
                {
                    MessageBox.Show("Pilih data terlebih dahulu!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            btnDelete.Click += (s, e) =>
            {
                if (selectedKategoriId > 0)
                {
                    if (MessageBox.Show("Yakin ingin menghapus?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (kategoriController.DeleteKategori(selectedKategoriId))
                        {
                            MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadKategoriData(dgvKategori);
                            ClearInputs();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Pilih data terlebih dahulu!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            btnClear.Click += (s, e) => ClearInputs();
        }

        private void LoadKategoriData(DataGridView dgv)
        {
            var data = kategoriController.GetAllKategori();
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Nama");
            dt.Columns.Add("Deskripsi");

            foreach (var item in data)
            {
                dt.Rows.Add(item.KategoriId, item.NamaKategori, item.Deskripsi);
            }

            dgv.DataSource = dt;
        }

        private bool ValidateInput(TextBox txtNama)
        {
            if (string.IsNullOrEmpty(txtNama.Text))
            {
                MessageBox.Show("Nama kategori tidak boleh kosong!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void ClearInputs()
        {
            selectedKategoriId = 0;
            foreach (Control control in this.Controls)
            {
                if (control is Panel panel)
                {
                    foreach (Control innerControl in panel.Controls)
                    {
                        if (innerControl is TextBox textBox)
                            textBox.Clear();
                    }
                }
            }
        }
    }
}