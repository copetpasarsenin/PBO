using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Controllers;
using Tubes_Alia_Richard_714240035_714240047.Helpers;
using Tubes_Alia_Richard_714240035_714240047.Models;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public class ManajemenPesananForm : Form
    {
        private PesananController pesananController = new PesananController();
        private DataGridView dgvPesanan;
        private ComboBox cmbFilter;
        private Panel pnlDetail;
        private DataGridView dgvDetail;
        private Label lblDetailInfo;
        private ComboBox cmbStatus;
        private Button btnUpdateStatus;

        public ManajemenPesananForm()
        {
            InitializeComponent();
            LoadPesanan();
        }

        private void InitializeComponent()
        {
            this.Text = "Manajemen Pesanan";
            this.Width = 1100;
            this.Height = 700;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Header Panel
            Panel pnlHeader = UIHelper.CreateHeader("MANAJEMEN PESANAN", this.Width);
            this.Controls.Add(pnlHeader);

            // Filter Panel
            Panel pnlFilter = new Panel
            {
                Left = 15,
                Top = 60,
                Width = 400,
                Height = 50,
                BackColor = Color.Transparent
            };

            Label lblFilter = new Label
            {
                Text = "Filter Status:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 0,
                Top = 15,
                AutoSize = true
            };

            cmbFilter = new ComboBox
            {
                Left = 100,
                Top = 10,
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cmbFilter.Items.AddRange(new string[] { "Semua", "Pending", "Diproses", "Dikirim", "Selesai", "Dibatalkan" });
            cmbFilter.SelectedIndex = 0;
            cmbFilter.SelectedIndexChanged += (s, e) => FilterPesanan();

            Button btnRefresh = new Button
            {
                Text = "🔄 Refresh",
                Left = 270,
                Top = 8,
                Width = 100,
                Height = 35
            };
            UIHelper.StyleButton(btnRefresh, UIHelper.PrimaryColor);
            btnRefresh.Click += (s, e) => LoadPesanan();

            pnlFilter.Controls.Add(lblFilter);
            pnlFilter.Controls.Add(cmbFilter);
            pnlFilter.Controls.Add(btnRefresh);
            this.Controls.Add(pnlFilter);

            // DataGridView Pesanan
            dgvPesanan = new DataGridView
            {
                Left = 15,
                Top = 120,
                Width = 650,
                Height = 500
            };
            UIHelper.StyleDataGridView(dgvPesanan);
            dgvPesanan.CellClick += DgvPesanan_CellClick;
            dgvPesanan.CellFormatting += DgvPesanan_CellFormatting;
            this.Controls.Add(dgvPesanan);

            // Detail Panel
            pnlDetail = new Panel
            {
                Left = 680,
                Top = 120,
                Width = 390,
                Height = 500,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblDetailTitle = new Label
            {
                Text = "📋 DETAIL PESANAN",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = UIHelper.DarkColor,
                Left = 15,
                Top = 15,
                AutoSize = true
            };

            lblDetailInfo = new Label
            {
                Text = "Pilih pesanan untuk melihat detail",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Left = 15,
                Top = 50,
                Width = 360,
                Height = 120
            };

            dgvDetail = new DataGridView
            {
                Left = 15,
                Top = 180,
                Width = 355,
                Height = 180
            };
            UIHelper.StyleDataGridView(dgvDetail);

            Label lblStatusLabel = new Label
            {
                Text = "Update Status:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 15,
                Top = 380,
                AutoSize = true
            };

            cmbStatus = new ComboBox
            {
                Left = 15,
                Top = 405,
                Width = 170,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cmbStatus.Items.AddRange(new string[] { "Diproses", "Dikirim", "Selesai", "Dibatalkan" });

            btnUpdateStatus = new Button
            {
                Text = "✓ Update",
                Left = 195,
                Top = 403,
                Width = 100,
                Height = 35,
                Enabled = false
            };
            UIHelper.StyleButton(btnUpdateStatus, UIHelper.SuccessColor);
            btnUpdateStatus.Click += BtnUpdateStatus_Click;

            Button btnCetak = new Button
            {
                Text = "🖨 Cetak",
                Left = 300,
                Top = 403,
                Width = 70,
                Height = 35,
                Enabled = false
            };
            UIHelper.StyleButton(btnCetak, UIHelper.InfoColor);

            pnlDetail.Controls.Add(lblDetailTitle);
            pnlDetail.Controls.Add(lblDetailInfo);
            pnlDetail.Controls.Add(dgvDetail);
            pnlDetail.Controls.Add(lblStatusLabel);
            pnlDetail.Controls.Add(cmbStatus);
            pnlDetail.Controls.Add(btnUpdateStatus);
            pnlDetail.Controls.Add(btnCetak);
            this.Controls.Add(pnlDetail);

            // Stats summary at bottom
            Panel pnlStats = new Panel
            {
                Left = 15,
                Top = 630,
                Width = 650,
                Height = 30,
                BackColor = Color.Transparent
            };

            Label lblStats = new Label
            {
                Text = "Statistik pesanan akan ditampilkan di sini",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize = true
            };
            pnlStats.Controls.Add(lblStats);
            this.Controls.Add(pnlStats);

            UpdateStats(pnlStats);
        }

        private void LoadPesanan()
        {
            DataTable dt = pesananController.GetAllPesanan();
            dgvPesanan.DataSource = dt;

            if (dgvPesanan.Columns.Contains("Total"))
            {
                dgvPesanan.Columns["Total"].DefaultCellStyle.Format = "N0";
            }
        }

        private void FilterPesanan()
        {
            if (cmbFilter.SelectedIndex == 0)
            {
                LoadPesanan();
            }
            else
            {
                DataTable dt = pesananController.FilterByStatus(cmbFilter.SelectedItem.ToString());
                dgvPesanan.DataSource = dt;
            }
        }

        private void DgvPesanan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int pesananId = Convert.ToInt32(dgvPesanan.Rows[e.RowIndex].Cells["ID"].Value);
                ShowPesananDetail(pesananId);
            }
        }

        private void DgvPesanan_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvPesanan.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();
                e.CellStyle.BackColor = UIHelper.GetStatusColor(status);
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }
        }

        private void ShowPesananDetail(int pesananId)
        {
            Pesanan pesanan = pesananController.GetPesananById(pesananId);
            if (pesanan != null)
            {
                lblDetailInfo.Text = $"ID Pesanan: #{pesanan.PesananId}\n" +
                                    $"Pelanggan: {pesanan.NamaUser}\n" +
                                    $"Tanggal: {pesanan.TanggalPesan:dd/MM/yyyy HH:mm}\n" +
                                    $"Total: Rp {pesanan.TotalHarga:N0}\n" +
                                    $"Status: {pesanan.Status}\n" +
                                    $"Alamat: {pesanan.AlamatPengiriman}";

                // Load detail items
                List<DetailPesanan> details = pesananController.GetDetailPesanan(pesananId);
                DataTable dt = new DataTable();
                dt.Columns.Add("Produk");
                dt.Columns.Add("Harga", typeof(decimal));
                dt.Columns.Add("Qty", typeof(int));
                dt.Columns.Add("Subtotal", typeof(decimal));

                foreach (var item in details)
                {
                    dt.Rows.Add(item.NamaProduk, item.Harga, item.Jumlah, item.Subtotal);
                }

                dgvDetail.DataSource = dt;
                dgvDetail.Tag = pesananId;

                // Set current status in combo
                cmbStatus.SelectedItem = pesanan.Status;
                btnUpdateStatus.Enabled = true;
            }
        }

        private void BtnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dgvDetail.Tag != null && cmbStatus.SelectedItem != null)
            {
                int pesananId = (int)dgvDetail.Tag;
                string newStatus = cmbStatus.SelectedItem.ToString();

                if (pesananController.UpdateStatus(pesananId, newStatus))
                {
                    MessageBox.Show($"Status berhasil diupdate menjadi '{newStatus}'", "Sukses", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPesanan();
                    ShowPesananDetail(pesananId);
                }
            }
        }

        private void UpdateStats(Panel pnlStats)
        {
            Dictionary<string, object> stats = pesananController.GetOrderStatistics();
            
            if (stats.Count > 0)
            {
                Label lblStats = pnlStats.Controls[0] as Label;
                if (lblStats != null)
                {
                    lblStats.Text = $"📊 Total: {stats["TotalPesanan"]} pesanan | " +
                                   $"⏳ Pending: {stats["Pending"]} | " +
                                   $"🔄 Diproses: {stats["Diproses"]} | " +
                                   $"💰 Pendapatan: Rp {Convert.ToDecimal(stats["PendapatanTotal"]):N0}";
                    lblStats.ForeColor = UIHelper.DarkColor;
                }
            }
        }
    }
}
