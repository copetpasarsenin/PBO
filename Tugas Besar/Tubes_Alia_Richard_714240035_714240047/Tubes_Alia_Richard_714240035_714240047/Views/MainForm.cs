using System;
using System.Drawing;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Models;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public partial class MainForm : Form
    {
        private User currentUser;

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

            ToolStripMenuItem menuProduk = new ToolStripMenuItem("Manajemen Produk");
            menuProduk.Click += (s, e) => OpenChildForm(new ProdukForm());

            ToolStripMenuItem menuKategori = new ToolStripMenuItem("Manajemen Kategori");
            menuKategori.Click += (s, e) => OpenChildForm(new KategoriForm());

            menuData.DropDownItems.Add(menuProduk);
            menuData.DropDownItems.Add(menuKategori);

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

            // Menu Settings (baru)
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

            // Update Menu Data - Tambah Dashboard
            ToolStripMenuItem menuDashboard = new ToolStripMenuItem("Dashboard");
            menuDashboard.Click += (s, e) => OpenChildForm(new DashboardForm());

            menuData.DropDownItems.Insert(0, menuDashboard); // Tambah di posisi pertama
        }

        private void OpenChildForm(Form childForm)
        {
            childForm.MdiParent = this;
            childForm.Show();
        }
    }
}