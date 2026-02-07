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
    public class RiwayatPesananForm : Form
    {
        private PesananController pesananController = new PesananController();
        private int userId;
        private DataGridView dgvPesanan;
        private Panel pnlDetail;
        private DataGridView dgvDetail;
        private Label lblDetailInfo;

        public RiwayatPesananForm(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            LoadPesanan();
        }

        private void InitializeComponent()
        {
            this.Text = "Riwayat Pesanan";
            this.Width = 1050;
            this.Height = 650;
            this.BackColor = Color.FromArgb(245, 247, 250);

            // Header
            Panel pnlHeader = UIHelper.CreateHeader("📋 RIWAYAT PESANAN SAYA", this.Width);
            this.Controls.Add(pnlHeader);

            // DataGridView Pesanan
            dgvPesanan = new DataGridView
            {
                Left = 15,
                Top = 65,
                Width = 600,
                Height = 520
            };
            UIHelper.StyleDataGridView(dgvPesanan);
            dgvPesanan.CellClick += DgvPesanan_CellClick;
            dgvPesanan.CellFormatting += DgvPesanan_CellFormatting;
            this.Controls.Add(dgvPesanan);

            // Detail Panel
            pnlDetail = new Panel
            {
                Left = 630,
                Top = 65,
                Width = 390,
                Height = 520,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblDetailTitle = new Label
            {
                Text = "📦 DETAIL PESANAN",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = UIHelper.DarkColor,
                Left = 15,
                Top = 15,
                AutoSize = true
            };

            lblDetailInfo = new Label
            {
                Text = "Klik pesanan untuk melihat detail",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                Left = 15,
                Top = 50,
                Width = 360,
                Height = 150
            };

            dgvDetail = new DataGridView
            {
                Left = 15,
                Top = 210,
                Width = 355,
                Height = 200
            };
            UIHelper.StyleDataGridView(dgvDetail);

            // Cancel order button (only for pending orders)
            Button btnBatalkan = new Button
            {
                Text = "❌ Batalkan Pesanan",
                Left = 15,
                Top = 425,
                Width = 170,
                Height = 40,
                Visible = false,
                Tag = 0
            };
            UIHelper.StyleButton(btnBatalkan, UIHelper.DangerColor);
            btnBatalkan.Click += BtnBatalkan_Click;

            // Reorder button
            Button btnPesanLagi = new Button
            {
                Text = "🔄 Pesan Lagi",
                Left = 195,
                Top = 425,
                Width = 140,
                Height = 40,
                Visible = false
            };
            UIHelper.StyleButton(btnPesanLagi, UIHelper.SuccessColor);

            pnlDetail.Controls.Add(lblDetailTitle);
            pnlDetail.Controls.Add(lblDetailInfo);
            pnlDetail.Controls.Add(dgvDetail);
            pnlDetail.Controls.Add(btnBatalkan);
            pnlDetail.Controls.Add(btnPesanLagi);
            this.Controls.Add(pnlDetail);

            // Refresh Button
            Button btnRefresh = new Button
            {
                Text = "🔄 Refresh",
                Left = 15,
                Top = 595,
                Width = 100,
                Height = 35
            };
            UIHelper.StyleButton(btnRefresh, UIHelper.PrimaryColor);
            btnRefresh.Click += (s, e) => LoadPesanan();
            this.Controls.Add(btnRefresh);
        }

        private void LoadPesanan()
        {
            DataTable dt = pesananController.GetPesananByUserId(userId);
            dgvPesanan.DataSource = dt;

            if (dgvPesanan.Columns.Contains("Total"))
            {
                dgvPesanan.Columns["Total"].DefaultCellStyle.Format = "N0";
            }

            // Reset detail panel
            lblDetailInfo.Text = "Klik pesanan untuk melihat detail";
            dgvDetail.DataSource = null;
            
            foreach (Control ctrl in pnlDetail.Controls)
            {
                if (ctrl is Button btn && (btn.Text.Contains("Batalkan") || btn.Text.Contains("Pesan Lagi")))
                {
                    btn.Visible = false;
                }
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
                string statusIcon = GetStatusIcon(pesanan.Status);
                
                lblDetailInfo.Text = $"ID Pesanan: #{pesanan.PesananId}\n\n" +
                                    $"📅 Tanggal: {pesanan.TanggalPesan:dd/MM/yyyy HH:mm}\n" +
                                    $"💰 Total: {UIHelper.FormatCurrency(pesanan.TotalHarga)}\n" +
                                    $"{statusIcon} Status: {pesanan.Status}\n\n" +
                                    $"📍 Alamat:\n{pesanan.AlamatPengiriman}";

                if (!string.IsNullOrEmpty(pesanan.Catatan))
                {
                    lblDetailInfo.Text += $"\n\n📝 Catatan: {pesanan.Catatan}";
                }

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

                // Show/Hide buttons based on status
                foreach (Control ctrl in pnlDetail.Controls)
                {
                    if (ctrl is Button btn)
                    {
                        if (btn.Text.Contains("Batalkan"))
                        {
                            btn.Visible = pesanan.Status == "Pending";
                            btn.Tag = pesananId;
                        }
                        else if (btn.Text.Contains("Pesan Lagi"))
                        {
                            btn.Visible = pesanan.Status == "Selesai" || pesanan.Status == "Dibatalkan";
                        }
                    }
                }
            }
        }

        private string GetStatusIcon(string status)
        {
            switch (status.ToLower())
            {
                case "pending": return "⏳";
                case "diproses": return "🔄";
                case "dikirim": return "🚚";
                case "selesai": return "✅";
                case "dibatalkan": return "❌";
                default: return "📋";
            }
        }

        private void BtnBatalkan_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn?.Tag == null) return;

            int pesananId = Convert.ToInt32(btn.Tag);

            if (MessageBox.Show("Yakin ingin membatalkan pesanan ini?\nStok produk akan dikembalikan.",
                "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (pesananController.CancelPesanan(pesananId))
                {
                    MessageBox.Show("Pesanan berhasil dibatalkan!", "Sukses",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPesanan();
                }
            }
        }
    }
}
