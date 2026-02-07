using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Models;
using Tubes_Alia_Richard_714240035_714240047.Helpers;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public class UserMainForm : Form
    {
        private User currentUser;
        public static List<KeranjangItem> Keranjang = new List<KeranjangItem>();

        public UserMainForm(User user)
        {
            this.currentUser = user;
            Keranjang = new List<KeranjangItem>();
            InitializeComponent();
            
            // Auto-open katalog on startup
            this.Load += (s, e) => OpenChildForm(new KatalogForm(currentUser.UserId));
        }

        private void InitializeComponent()
        {
            this.Text = $"E-Commerce - Selamat Datang, {currentUser.FullName}!";
            this.Width = 1300;
            this.Height = 750;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.IsMdiContainer = true;
            this.BackColor = Color.FromArgb(236, 240, 241);

            // Menu Strip Modern
            MenuStrip menuStrip = new MenuStrip
            {
                BackColor = UIHelper.PrimaryColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Padding = new Padding(10, 5, 0, 5)
            };

            // Menu Beranda
            ToolStripMenuItem menuBeranda = new ToolStripMenuItem("🏠 BERANDA");
            menuBeranda.ForeColor = Color.White;
            menuBeranda.Click += (s, e) =>
            {
                MessageBox.Show($"🛍️ Selamat datang di E-Commerce!\n\n" +
                    $"👤 User: {currentUser.FullName}\n" +
                    $"📧 Email: {currentUser.Email}\n" +
                    $"🛒 Keranjang: {Keranjang.Count} item",
                    "Beranda", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            // Menu Katalog
            ToolStripMenuItem menuKatalog = new ToolStripMenuItem("📦 KATALOG");
            menuKatalog.ForeColor = Color.White;
            menuKatalog.Click += (s, e) => OpenChildForm(new KatalogForm(currentUser.UserId));

            // Menu Keranjang
            ToolStripMenuItem menuKeranjang = new ToolStripMenuItem("🛒 KERANJANG");
            menuKeranjang.ForeColor = Color.White;
            menuKeranjang.Click += (s, e) => OpenChildForm(new KeranjangForm(currentUser.UserId));

            // Menu Pesanan Saya
            ToolStripMenuItem menuPesanan = new ToolStripMenuItem("📋 PESANAN SAYA");
            menuPesanan.ForeColor = Color.White;
            menuPesanan.Click += (s, e) => OpenChildForm(new RiwayatPesananForm(currentUser.UserId));

            // Menu Akun
            ToolStripMenuItem menuAkun = new ToolStripMenuItem("👤 AKUN");
            menuAkun.ForeColor = Color.White;

            ToolStripMenuItem menuProfil = new ToolStripMenuItem("Profil Saya");
            menuProfil.Click += (s, e) =>
            {
                MessageBox.Show($"👤 Profil User\n\n" +
                    $"Username: {currentUser.Username}\n" +
                    $"Nama: {currentUser.FullName}\n" +
                    $"Email: {currentUser.Email}\n" +
                    $"Role: {currentUser.Role}",
                    "Profil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            ToolStripMenuItem menuGantiPassword = new ToolStripMenuItem("Ganti Password");
            menuGantiPassword.Click += (s, e) => OpenChildForm(new ChangePasswordForm(currentUser.UserId));

            ToolStripMenuItem menuLogout = new ToolStripMenuItem("Logout");
            menuLogout.Click += (s, e) =>
            {
                if (MessageBox.Show("Yakin ingin logout?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Close();
                }
            };

            menuAkun.DropDownItems.Add(menuProfil);
            menuAkun.DropDownItems.Add(menuGantiPassword);
            menuAkun.DropDownItems.Add(new ToolStripSeparator());
            menuAkun.DropDownItems.Add(menuLogout);

            menuStrip.Items.Add(menuBeranda);
            menuStrip.Items.Add(menuKatalog);
            menuStrip.Items.Add(menuKeranjang);
            menuStrip.Items.Add(menuPesanan);
            menuStrip.Items.Add(menuAkun);

            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;
        }

        private void OpenChildForm(Form childForm)
        {
            // Check if form of same type is already open
            foreach (Form child in this.MdiChildren)
            {
                if (child.GetType() == childForm.GetType())
                {
                    child.Activate();
                    return;
                }
            }

            childForm.MdiParent = this;
            childForm.WindowState = FormWindowState.Maximized;
            childForm.Show();
        }
    }
}


