using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Controllers;
using Tubes_Alia_Richard_714240035_714240047.Models;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public partial class ProdukForm : Form
    {
        private ProdukController produkController = new ProdukController();
        private KategoriController kategoriController = new KategoriController();
        private int selectedProdukId = 0;
        private string userRole;

        public ProdukForm(string role = "User")
        {
            userRole = role;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Manajemen Produk";
            this.Width = 1100;
            this.Height = 700;
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
                Text = "MANAJEMEN PRODUK",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Left = 10,
                Top = 5
            };

            pnlHeader.Controls.Add(lblHeader);

            // Panel Search (BARU)
            Panel pnlSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(236, 240, 241),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15)
            };

            Label lblSearch = new Label 
            { 
                Text = "Cari Produk:", 
                Left = 15, 
                Top = 12, 
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold) 
            };

            TextBox txtSearch = new TextBox 
            { 
                Left = 120, 
                Top = 10, 
                Width = 300, 
                Height = 28, 
                Font = new Font("Arial", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            Button btnSearch = new Button 
            { 
                Text = "CARI", 
                Left = 430, 
                Top = 10, 
                Width = 70, 
                Height = 28, 
                BackColor = Color.FromArgb(52, 152, 219), 
                ForeColor = Color.White, 
                Font = new Font("Arial", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.FlatAppearance.BorderSize = 0;

            Button btnResetSearch = new Button 
            { 
                Text = "RESET", 
                Left = 510, 
                Top = 10, 
                Width = 70, 
                Height = 28, 
                BackColor = Color.FromArgb(149, 165, 166), 
                ForeColor = Color.White, 
                Font = new Font("Arial", 9, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnResetSearch.FlatAppearance.BorderSize = 0;

            pnlSearch.Controls.Add(lblSearch);
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Controls.Add(btnSearch);
            pnlSearch.Controls.Add(btnResetSearch);

            // Panel Input
            Panel pnlInput = new Panel
            {
                Dock = DockStyle.Top,
                Height = 200,
                BackColor = Color.FromArgb(245, 245, 245),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15)
            };

            // Row 1
            Label lblNama = new Label { Text = "Nama Produk:", Left = 15, Top = 15, Font = new Font("Arial", 10, FontStyle.Bold) };
            TextBox txtNama = new TextBox { Left = 150, Top = 15, Width = 300, Height = 30, Font = new Font("Arial", 10) };

            Label lblKategori = new Label { Text = "Kategori:", Left = 520, Top = 15, Font = new Font("Arial", 10, FontStyle.Bold) };
            ComboBox cmbKategori = new ComboBox { Left = 650, Top = 15, Width = 200, Height = 30, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Arial", 10) };
            LoadKategori(cmbKategori);

            // Row 2
            Label lblHarga = new Label { Text = "Harga:", Left = 15, Top = 55, Font = new Font("Arial", 10, FontStyle.Bold) };
            TextBox txtHarga = new TextBox { Left = 150, Top = 55, Width = 300, Height = 30, Font = new Font("Arial", 10) };

            Label lblStok = new Label { Text = "Stok:", Left = 520, Top = 55, Font = new Font("Arial", 10, FontStyle.Bold) };
            TextBox txtStok = new TextBox { Left = 650, Top = 55, Width = 200, Height = 30, Font = new Font("Arial", 10) };

            // Row 3
            Label lblDeskripsi = new Label { Text = "Deskripsi:", Left = 15, Top = 95, Font = new Font("Arial", 10, FontStyle.Bold) };
            TextBox txtDeskripsi = new TextBox { Left = 150, Top = 95, Width = 700, Height = 70, Multiline = true, Font = new Font("Arial", 10) };

            // Buttons
            Button btnAdd = new Button 
            { 
                Text = "TAMBAH", 
                Left = 15, 
                Top = 165, 
                Width = 100, 
                Height = 28, 
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
                Top = 165, 
                Width = 100, 
                Height = 28, 
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
                Top = 165, 
                Width = 100, 
                Height = 28, 
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
                Top = 165, 
                Width = 105, 
                Height = 28, 
                BackColor = Color.FromArgb(149, 165, 166), 
                ForeColor = Color.White, 
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnClear.FlatAppearance.BorderSize = 0;

            Button btnRefresh = new Button 
            { 
                Text = "REFRESH", 
                Left = 460, 
                Top = 165, 
                Width = 100, 
                Height = 28, 
                BackColor = Color.FromArgb(155, 89, 182), 
                ForeColor = Color.White, 
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.FlatAppearance.BorderSize = 0;

            pnlInput.Controls.Add(lblNama);
            pnlInput.Controls.Add(txtNama);
            pnlInput.Controls.Add(lblKategori);
            pnlInput.Controls.Add(cmbKategori);
            pnlInput.Controls.Add(lblHarga);
            pnlInput.Controls.Add(txtHarga);
            pnlInput.Controls.Add(lblStok);
            pnlInput.Controls.Add(txtStok);
            pnlInput.Controls.Add(lblDeskripsi);
            pnlInput.Controls.Add(txtDeskripsi);
            pnlInput.Controls.Add(btnAdd);
            pnlInput.Controls.Add(btnUpdate);
            pnlInput.Controls.Add(btnDelete);
            pnlInput.Controls.Add(btnClear);
            pnlInput.Controls.Add(btnRefresh);

            // DataGridView
            DataGridView dgvProduk = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = true,
                BorderStyle = BorderStyle.FixedSingle
            };

            LoadProdukData(dgvProduk);

            this.Controls.Add(dgvProduk);
            this.Controls.Add(pnlInput);
            this.Controls.Add(pnlSearch);
            this.Controls.Add(pnlHeader);

            // Disable buttons untuk User
            if (userRole != "Admin")
            {
                btnAdd.Enabled = true;
                btnAdd.BackColor = Color.FromArgb(189, 195, 199);
                
                btnUpdate.Enabled = true;
                btnUpdate.BackColor = Color.FromArgb(189, 195, 199);
                
                btnDelete.Enabled = true;
                btnDelete.BackColor = Color.FromArgb(189, 195, 199);
                
                btnClear.Visible = true;
            }

            if (userRole != "User")
            {
                btnAdd.Enabled = false;
                btnAdd.BackColor = Color.FromArgb(189, 195, 199);

                btnUpdate.Enabled = false;
                btnUpdate.BackColor = Color.FromArgb(189, 195, 199);

                btnDelete.Enabled = false;
                btnDelete.BackColor = Color.FromArgb(189, 195, 199);

                btnClear.Visible = false;
            }

            // Event Handlers
            btnAdd.Click += (s, e) =>
            {
                if (ValidateInput(txtNama, txtHarga, txtStok, cmbKategori))
                {
                    var produk = new Produk
                    {
                        NamaProduk = txtNama.Text,
                        KategoriId = Convert.ToInt32(cmbKategori.SelectedValue),
                        Harga = Convert.ToDecimal(txtHarga.Text),
                        Stok = Convert.ToInt32(txtStok.Text),
                        Deskripsi = txtDeskripsi.Text
                    };

                    if (produkController.CreateProduk(produk))
                    {
                        MessageBox.Show("Data berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProdukData(dgvProduk);
                        ClearInputs();
                    }
                }
            };

            dgvProduk.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    selectedProdukId = Convert.ToInt32(dgvProduk.Rows[e.RowIndex].Cells[0].Value);
                    var produk = produkController.GetProdukById(selectedProdukId);
                    
                    if (produk != null)
                    {
                        txtNama.Text = produk.NamaProduk;
                        cmbKategori.SelectedValue = produk.KategoriId;
                        txtHarga.Text = produk.Harga.ToString();
                        txtStok.Text = produk.Stok.ToString();
                        txtDeskripsi.Text = produk.Deskripsi;
                    }
                }
            };

            btnUpdate.Click += (s, e) =>
            {
                if (selectedProdukId > 0 && ValidateInput(txtNama, txtHarga, txtStok, cmbKategori))
                {
                    var produk = new Produk
                    {
                        ProdukId = selectedProdukId,
                        NamaProduk = txtNama.Text,
                        KategoriId = Convert.ToInt32(cmbKategori.SelectedValue),
                        Harga = Convert.ToDecimal(txtHarga.Text),
                        Stok = Convert.ToInt32(txtStok.Text),
                        Deskripsi = txtDeskripsi.Text
                    };

                    if (produkController.UpdateProduk(produk))
                    {
                        MessageBox.Show("Data berhasil diupdate!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProdukData(dgvProduk);
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
                if (selectedProdukId > 0)
                {
                    if (MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (produkController.DeleteProduk(selectedProdukId))
                        {
                            MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadProdukData(dgvProduk);
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

            btnRefresh.Click += (s, e) => LoadProdukData(dgvProduk);

            btnSearch.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtSearch.Text))
                {
                    MessageBox.Show("Masukkan kata kunci pencarian!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                dgvProduk.DataSource = produkController.SearchProduk(txtSearch.Text);
            };

            btnResetSearch.Click += (s, e) =>
            {
                txtSearch.Clear();
                LoadProdukData(dgvProduk);
            };
        }

        private void LoadProdukData(DataGridView dgv)
        {
            dgv.DataSource = produkController.GetProdukDataTable();
        }

        private void LoadKategori(ComboBox cmb)
        {
            DataTable dt = kategoriController.GetKategoriDataTable();
            cmb.DataSource = dt;
            cmb.DisplayMember = "Nama";
            cmb.ValueMember = "ID";
        }

        private bool ValidateInput(TextBox txtNama, TextBox txtHarga, TextBox txtStok, ComboBox cmbKategori)
        {
            if (string.IsNullOrEmpty(txtNama.Text))
            {
                MessageBox.Show("Nama produk tidak boleh kosong!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtHarga.Text, out _))
            {
                MessageBox.Show("Harga harus berupa angka!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtStok.Text, out _))
            {
                MessageBox.Show("Stok harus berupa angka!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbKategori.SelectedIndex < 0)
            {
                MessageBox.Show("Kategori harus dipilih!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearInputs()
        {
            selectedProdukId = 0;
            foreach (Control control in this.Controls)
            {
                if (control is Panel panel)
                {
                    foreach (Control innerControl in panel.Controls)
                    {
                        if (innerControl is TextBox textBox)
                            textBox.Clear();
                        else if (innerControl is ComboBox comboBox)
                            comboBox.SelectedIndex = -1;
                    }
                }
            }
        }
    }
}