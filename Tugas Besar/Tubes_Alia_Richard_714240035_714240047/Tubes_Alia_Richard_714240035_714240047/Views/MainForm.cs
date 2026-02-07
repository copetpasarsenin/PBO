using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Controllers;
using Tubes_Alia_Richard_714240035_714240047.Helpers;
using Tubes_Alia_Richard_714240035_714240047.Models;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public partial class MainForm : Form
    {
        private User currentUser;
        private Panel pnlDashboard;
        private Panel pnlContent;
        private ProdukController produkController = new ProdukController();
        private PesananController pesananController = new PesananController();

        public MainForm(User user)
        {
            this.currentUser = user;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = $"Sistem Manajemen Produk - {currentUser.FullName} ({currentUser.Role})";
            this.Width = 1200;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Create MenuStrip
            MenuStrip menuStrip = new MenuStrip();
            menuStrip.BackColor = Color.FromArgb(41, 128, 185);
            menuStrip.ForeColor = Color.White;
            menuStrip.Font = new Font("Arial", 11, FontStyle.Bold);

            // Menu Data
            ToolStripMenuItem menuData = new ToolStripMenuItem("DATA");
            menuData.ForeColor = Color.White;

            ToolStripMenuItem menuDashboard = new ToolStripMenuItem("Dashboard");
            menuDashboard.Click += (s, e) => ShowDashboard();

            ToolStripMenuItem menuProduk = new ToolStripMenuItem("Manajemen Produk");
            menuProduk.Click += (s, e) => OpenChildForm(new ProdukForm());

            ToolStripMenuItem menuKategori = new ToolStripMenuItem("Manajemen Kategori");
            menuKategori.Click += (s, e) => OpenChildForm(new KategoriForm());

            ToolStripMenuItem menuPesanan = new ToolStripMenuItem("Manajemen Pesanan");
            menuPesanan.Click += (s, e) => OpenChildForm(new ManajemenPesananForm());

            menuData.DropDownItems.Add(menuDashboard);
            menuData.DropDownItems.Add(new ToolStripSeparator());
            menuData.DropDownItems.Add(menuProduk);
            menuData.DropDownItems.Add(menuKategori);
            menuData.DropDownItems.Add(new ToolStripSeparator());
            menuData.DropDownItems.Add(menuPesanan);

            // Menu Report
            ToolStripMenuItem menuReport = new ToolStripMenuItem("REPORT");
            menuReport.ForeColor = Color.White;

            ToolStripMenuItem menuExcel = new ToolStripMenuItem("Export ke Excel");
            menuExcel.Click += (s, e) => OpenChildForm(new ExportForm());

            menuReport.DropDownItems.Add(menuExcel);

            // Menu Help
            ToolStripMenuItem menuHelp = new ToolStripMenuItem("HELP");
            menuHelp.ForeColor = Color.White;

            ToolStripMenuItem menuAbout = new ToolStripMenuItem("Tentang Aplikasi");
            menuAbout.Click += (s, e) => 
            {
                MessageBox.Show($"Sistem Manajemen Produk v1.0\n\n" +
                    $"Dibuat oleh: Alia & Richard\n" +
                    $"NIM: 714240035 & 714240047\n\n" +
                    $"User: {currentUser.FullName}\n" +
                    $"Role: {currentUser.Role}", 
                    "Tentang", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            ToolStripMenuItem menuLogout = new ToolStripMenuItem("Logout");
            menuLogout.Click += (s, e) =>
            {
                if (MessageBox.Show("Yakin ingin logout?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Close();
                }
            };

            menuHelp.DropDownItems.Add(menuAbout);
            menuHelp.DropDownItems.Add(new ToolStripSeparator());
            menuHelp.DropDownItems.Add(menuLogout);

            // Menu Settings
            ToolStripMenuItem menuSettings = new ToolStripMenuItem("SETTINGS");
            menuSettings.ForeColor = Color.White;

            ToolStripMenuItem menuChangePassword = new ToolStripMenuItem("Ganti Password");
            menuChangePassword.Click += (s, e) => OpenChildForm(new ChangePasswordForm(currentUser.UserId));

            ToolStripMenuItem menuProfile = new ToolStripMenuItem("Profile User");
            menuProfile.Click += (s, e) => MessageBox.Show($"Username: {currentUser.Username}\nNama: {currentUser.FullName}\nEmail: {currentUser.Email}\nRole: {currentUser.Role}");

            menuSettings.DropDownItems.Add(menuChangePassword);
            menuSettings.DropDownItems.Add(menuProfile);

            menuStrip.Items.Add(menuData);
            menuStrip.Items.Add(menuReport);
            menuStrip.Items.Add(menuHelp);
            menuStrip.Items.Add(menuSettings);

            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;

            // Create embedded dashboard panel
            CreateDashboardPanel();

            // Show dashboard by default
            this.Load += (s, e) => ShowDashboard();
        }

        private void CreateDashboardPanel()
        {
            pnlDashboard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(41, 128, 185), // Same as header
                Visible = true
            };

            // Content Panel dengan scroll - putih
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250),
                AutoScroll = true,
                Padding = new Padding(15)
            };

            // Header Label di dalam content
            Label lblHeader = new Label
            {
                Text = "DASHBOARD ADMIN",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = UIHelper.DarkColor,
                Left = 20,
                Top = 10,
                AutoSize = true
            };
            pnlContent.Controls.Add(lblHeader);

            LoadDashboardContent();

            pnlDashboard.Controls.Add(pnlContent);
            this.Controls.Add(pnlDashboard);
        }

        private void LoadDashboardContent()
        {
            // Clear except header label
            var header = pnlContent.Controls.Count > 0 ? pnlContent.Controls[0] : null;
            pnlContent.Controls.Clear();
            if (header != null) pnlContent.Controls.Add(header);

            // Get Statistics
            Dictionary<string, object> produkStats = produkController.GetStatistics();
            Dictionary<string, object> orderStats = pesananController.GetOrderStatistics();

            int startY = 50;
            int cardWidth = 240;
            int cardHeight = 100;
            int gap = 10;

            // Row 1: Produk Stats (4 cards)
            Panel card1 = CreateDashboardCard("📦 Total Produk", produkStats["TotalProduk"].ToString(), 
                UIHelper.SecondaryColor, 20, startY, cardWidth, cardHeight);
            Panel card2 = CreateDashboardCard("📁 Total Kategori", produkStats["TotalKategori"].ToString(), 
                UIHelper.SuccessColor, 20 + cardWidth + gap, startY, cardWidth, cardHeight);
            Panel card3 = CreateDashboardCard("📊 Total Stok", produkStats["TotalStok"].ToString(), 
                UIHelper.InfoColor, 20 + (cardWidth + gap) * 2, startY, cardWidth, cardHeight);
            Panel card4 = CreateDashboardCard("💰 Nilai Inventory", 
                UIHelper.FormatCurrency(Convert.ToDecimal(produkStats["TotalNilai"])), 
                UIHelper.OrangeColor, 20 + (cardWidth + gap) * 3, startY, cardWidth, cardHeight);

            // Row 2: Stock Alerts (2 cards) + Order Stats (2 cards)
            int row2Y = startY + cardHeight + gap;
            int outOfStock = Convert.ToInt32(produkStats["OutOfStock"]);
            int lowStock = Convert.ToInt32(produkStats["LowStock"]);
            
            Panel card5 = CreateDashboardCard("❌ Stok Habis", outOfStock.ToString(), 
                outOfStock > 0 ? Color.FromArgb(183, 28, 28) : UIHelper.SuccessColor, 
                20, row2Y, cardWidth, cardHeight);
            Panel card6 = CreateDashboardCard("⚠️ Stok Rendah", lowStock.ToString(), 
                lowStock > 0 ? UIHelper.WarningColor : UIHelper.SuccessColor, 
                20 + cardWidth + gap, row2Y, cardWidth, cardHeight);
            
            int totalPesanan = orderStats.ContainsKey("TotalPesanan") ? Convert.ToInt32(orderStats["TotalPesanan"]) : 0;
            int pending = orderStats.ContainsKey("Pending") ? Convert.ToInt32(orderStats["Pending"]) : 0;
            int diproses = orderStats.ContainsKey("Diproses") ? Convert.ToInt32(orderStats["Diproses"]) : 0;
            decimal pendapatan = orderStats.ContainsKey("PendapatanTotal") ? Convert.ToDecimal(orderStats["PendapatanTotal"]) : 0;
            
            Panel card7 = CreateDashboardCard("📋 Total Pesanan", totalPesanan.ToString(), 
                UIHelper.PrimaryColor, 20 + (cardWidth + gap) * 2, row2Y, cardWidth, cardHeight);
            Panel card8 = CreateDashboardCard("⏳ Perlu Diproses", (pending + diproses).ToString(), 
                pending + diproses > 0 ? UIHelper.WarningColor : UIHelper.SuccessColor, 
                20 + (cardWidth + gap) * 3, row2Y, cardWidth, cardHeight);

            // Row 3: Revenue
            int row3Y = row2Y + cardHeight + gap;
            Panel card9 = CreateDashboardCard("💵 Total Pendapatan", 
                UIHelper.FormatCurrency(pendapatan), 
                UIHelper.SuccessColor, 20, row3Y, cardWidth * 2 + gap, cardHeight);

            // Add cards
            pnlContent.Controls.Add(card1);
            pnlContent.Controls.Add(card2);
            pnlContent.Controls.Add(card3);
            pnlContent.Controls.Add(card4);
            pnlContent.Controls.Add(card5);
            pnlContent.Controls.Add(card6);
            pnlContent.Controls.Add(card7);
            pnlContent.Controls.Add(card8);
            pnlContent.Controls.Add(card9);

            // Section: Pesanan Terbaru
            int tableY = row3Y + cardHeight + gap + 10;
            Label lblRecentOrders = new Label
            {
                Text = "📋 PESANAN TERBARU",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = UIHelper.DarkColor,
                Left = 20,
                Top = tableY,
                AutoSize = true
            };
            pnlContent.Controls.Add(lblRecentOrders);

            DataGridView dgvRecentOrders = new DataGridView
            {
                Left = 20,
                Top = tableY + 30,
                Width = 420,
                Height = 180,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            UIHelper.StyleDataGridView(dgvRecentOrders);
            dgvRecentOrders.DataSource = pesananController.GetRecentOrders(5);
            pnlContent.Controls.Add(dgvRecentOrders);

            // Section: Produk Terlaris
            Label lblTopProducts = new Label
            {
                Text = "🏆 PRODUK TERLARIS",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = UIHelper.DarkColor,
                Left = 460,
                Top = tableY,
                AutoSize = true
            };
            pnlContent.Controls.Add(lblTopProducts);

            DataGridView dgvTopProducts = new DataGridView
            {
                Left = 460,
                Top = tableY + 30,
                Width = 420,
                Height = 180,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            UIHelper.StyleDataGridView(dgvTopProducts);
            dgvTopProducts.DataSource = produkController.GetTopProducts(5);
            pnlContent.Controls.Add(dgvTopProducts);

            // Button Refresh
            int btnY = tableY + 220;
            Button btnRefresh = new Button
            {
                Text = "🔄 REFRESH DATA",
                Left = 20,
                Top = btnY,
                Width = 150,
                Height = 40
            };
            UIHelper.StyleButton(btnRefresh, UIHelper.PrimaryColor);
            btnRefresh.Click += (s, e) => LoadDashboardContent();
            pnlContent.Controls.Add(btnRefresh);

            // Info Label
            Label lblInfo = new Label
            {
                Text = $"Terakhir diperbarui: {DateTime.Now:dd/MM/yyyy HH:mm:ss}",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray,
                Left = 185,
                Top = btnY + 12,
                AutoSize = true
            };
            pnlContent.Controls.Add(lblInfo);
        }

        private Panel CreateDashboardCard(string title, string value, Color bgColor, int x, int y, int w, int h)
        {
            Panel card = new Panel
            {
                Left = x,
                Top = y,
                Width = w,
                Height = h,
                BackColor = bgColor
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                Left = 15,
                Top = 15,
                AutoSize = true
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Left = 15,
                Top = 45,
                AutoSize = true
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            return card;
        }

        private void ShowDashboard()
        {
            // Close all MDI children
            foreach (Form child in this.MdiChildren)
            {
                child.Close();
            }
            
            // Show dashboard panel
            pnlDashboard.Visible = true;
            pnlDashboard.BringToFront();
            
            // Refresh data
            LoadDashboardContent();
        }

        private void OpenChildForm(Form childForm)
        {
            // Hide dashboard when opening child form
            pnlDashboard.Visible = false;
            
            childForm.MdiParent = this;
            childForm.WindowState = FormWindowState.Maximized;
            childForm.Show();
        }
    }
}