using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Controllers;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public class DashboardForm : Form
    {
        private ProdukController produkController = new ProdukController();

        public DashboardForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Dashboard";
            this.Width = 1100;
            this.Height = 650;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Panel Header
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.FromArgb(41, 128, 185),
                Padding = new Padding(15)
            };

            Label lblHeader = new Label
            {
                Text = "DASHBOARD STATISTIK",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Left = 10,
                Top = 5
            };

            pnlHeader.Controls.Add(lblHeader);

            // Main Panel
            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(20)
            };

            // Get Statistics
            Dictionary<string, object> stats = produkController.GetStatistics();

            // Stat Card 1: Total Produk
            Panel card1 = CreateStatCard("Total Produk", stats["TotalProduk"].ToString(), 
                Color.FromArgb(52, 152, 219), 20, 20);

            // Stat Card 2: Total Kategori
            Panel card2 = CreateStatCard("Total Kategori", stats["TotalKategori"].ToString(), 
                Color.FromArgb(46, 204, 113), 320, 20);

            // Stat Card 3: Total Stok
            Panel card3 = CreateStatCard("Total Stok", stats["TotalStok"].ToString(), 
                Color.FromArgb(155, 89, 182), 620, 20);

            // Stat Card 4: Total Nilai Inventory
            Panel card4 = CreateStatCard("Total Nilai Inventory", 
                "Rp " + Convert.ToDecimal(stats["TotalNilai"]).ToString("N0"), 
                Color.FromArgb(230, 126, 34), 20, 200);

            // Stat Card 5: Stok Rendah
            Panel card5 = CreateStatCard("Produk Stok Rendah", stats["LowStock"].ToString(), 
                Color.FromArgb(231, 76, 60), 320, 200);

            // Stat Card 6: Harga Rata-rata
            Panel card6 = CreateStatCard("Harga Rata-rata", 
                "Rp " + Convert.ToDecimal(stats["AvgHarga"]).ToString("N0"), 
                Color.FromArgb(52, 73, 94), 620, 200);

            // Button Refresh
            Button btnRefresh = new Button
            {
                Text = "REFRESH DATA",
                Left = 20,
                Top = 380,
                Width = 150,
                Height = 40,
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) =>
            {
                this.Close();
                DashboardForm newDashboard = new DashboardForm();
                newDashboard.MdiParent = this.MdiParent;
                newDashboard.Show();
            };

            // Info Label
            Label lblInfo = new Label
            {
                Text = "Statistik real-time mengenai inventori produk Anda",
                Font = new Font("Arial", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(127, 140, 141),
                Left = 20,
                Top = 430,
                Width = 600,
                Height = 30
            };

            pnlMain.Controls.Add(card1);
            pnlMain.Controls.Add(card2);
            pnlMain.Controls.Add(card3);
            pnlMain.Controls.Add(card4);
            pnlMain.Controls.Add(card5);
            pnlMain.Controls.Add(card6);
            pnlMain.Controls.Add(btnRefresh);
            pnlMain.Controls.Add(lblInfo);

            this.Controls.Add(pnlMain);
            this.Controls.Add(pnlHeader);
        }

        private Panel CreateStatCard(string title, string value, Color bgColor, int left, int top)
        {
            Panel card = new Panel
            {
                Left = left,
                Top = top,
                Width = 280,
                Height = 150,
                BackColor = bgColor,
                BorderStyle = BorderStyle.None,
                Cursor = Cursors.Hand
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Left = 15,
                Top = 20
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Arial", 28, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Left = 15,
                Top = 50
            };

            card.Paint += (s, e) =>
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200), 1), 0, 0, card.Width - 1, card.Height - 1);
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            return card;
        }
    }
}