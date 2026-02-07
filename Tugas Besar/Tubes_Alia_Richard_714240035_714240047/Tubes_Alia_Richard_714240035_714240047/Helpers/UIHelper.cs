using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Tubes_Alia_Richard_714240035_714240047.Helpers
{
    public static class UIHelper
    {
        // Color Scheme
        public static Color PrimaryColor = Color.FromArgb(41, 128, 185);
        public static Color SecondaryColor = Color.FromArgb(52, 152, 219);
        public static Color SuccessColor = Color.FromArgb(46, 204, 113);
        public static Color DangerColor = Color.FromArgb(231, 76, 60);
        public static Color WarningColor = Color.FromArgb(241, 196, 15);
        public static Color InfoColor = Color.FromArgb(155, 89, 182);
        public static Color DarkColor = Color.FromArgb(52, 73, 94);
        public static Color LightColor = Color.FromArgb(236, 240, 241);
        public static Color OrangeColor = Color.FromArgb(230, 126, 34);

        // Style Button dengan efek modern
        public static void StyleButton(Button btn, Color bgColor)
        {
            btn.BackColor = bgColor;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

            // Hover effect
            Color hoverColor = ControlPaint.Light(bgColor, 0.2f);
            Color pressColor = ControlPaint.Dark(bgColor, 0.2f);

            btn.MouseEnter += (s, e) => btn.BackColor = hoverColor;
            btn.MouseLeave += (s, e) => btn.BackColor = bgColor;
            btn.MouseDown += (s, e) => btn.BackColor = pressColor;
            btn.MouseUp += (s, e) => btn.BackColor = hoverColor;
        }

        // Style DataGridView dengan tampilan modern
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Color.White;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(220, 220, 220);
            
            // Header Style
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            
            // Row Style
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgv.DefaultCellStyle.Padding = new Padding(5);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.RowTemplate.Height = 35;
            
            // Alternating Row Colors
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 248, 250);
            
            // Selection
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Create Card Panel dengan shadow effect
        public static Panel CreateCard(int left, int top, int width, int height, Color bgColor)
        {
            Panel card = new Panel
            {
                Left = left,
                Top = top,
                Width = width,
                Height = height,
                BackColor = bgColor
            };

            // Paint event untuk shadow
            card.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(30, 0, 0, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            return card;
        }

        // Create Stat Card untuk Dashboard
        public static Panel CreateStatCard(string title, string value, Color bgColor, int left, int top, int width = 280, int height = 140)
        {
            Panel card = new Panel
            {
                Left = left,
                Top = top,
                Width = width,
                Height = height,
                BackColor = bgColor,
                Cursor = Cursors.Hand
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(230, 255, 255, 255),
                AutoSize = true,
                Left = 15,
                Top = 15
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Left = 15,
                Top = 45
            };

            // Hover effect
            card.MouseEnter += (s, e) => card.BackColor = ControlPaint.Light(bgColor, 0.1f);
            card.MouseLeave += (s, e) => card.BackColor = bgColor;

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            return card;
        }

        // Create Product Card untuk Katalog
        public static Panel CreateProductCard(int produkId, string nama, string kategori, decimal harga, int stok, 
            int left, int top, Action<int> onAddToCart)
        {
            Panel card = new Panel
            {
                Left = left,
                Top = top,
                Width = 220,
                Height = 280,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // Card shadow effect
            card.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(50, 0, 0, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            // Product Image placeholder
            Panel imgPanel = new Panel
            {
                Left = 10,
                Top = 10,
                Width = 200,
                Height = 100,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            Label lblImg = new Label
            {
                Text = "📦",
                Font = new Font("Segoe UI", 32),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            imgPanel.Controls.Add(lblImg);

            // Product Name
            Label lblNama = new Label
            {
                Text = nama,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = DarkColor,
                Left = 10,
                Top = 120,
                Width = 200,
                Height = 40
            };

            // Category
            Label lblKategori = new Label
            {
                Text = kategori,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Left = 10,
                Top = 160,
                Width = 200
            };

            // Price
            Label lblHarga = new Label
            {
                Text = "Rp " + harga.ToString("N0"),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = SuccessColor,
                Left = 10,
                Top = 185,
                Width = 200
            };

            // Stock
            Label lblStok = new Label
            {
                Text = stok > 0 ? $"Stok: {stok}" : "Habis",
                Font = new Font("Segoe UI", 9),
                ForeColor = stok > 0 ? Color.Gray : DangerColor,
                Left = 10,
                Top = 210,
                Width = 200
            };

            // Add to Cart Button
            Button btnAdd = new Button
            {
                Text = stok > 0 ? "🛒 Tambah" : "Stok Habis",
                Left = 10,
                Top = 235,
                Width = 200,
                Height = 35,
                Enabled = stok > 0
            };
            StyleButton(btnAdd, stok > 0 ? PrimaryColor : Color.Gray);
            btnAdd.Click += (s, e) => onAddToCart?.Invoke(produkId);

            card.Controls.Add(imgPanel);
            card.Controls.Add(lblNama);
            card.Controls.Add(lblKategori);
            card.Controls.Add(lblHarga);
            card.Controls.Add(lblStok);
            card.Controls.Add(btnAdd);

            // Hover effect
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(250, 253, 255);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

            return card;
        }

        // Get Status Color
        public static Color GetStatusColor(string status)
        {
            switch (status.ToLower())
            {
                case "pending":
                    return WarningColor;
                case "diproses":
                    return SecondaryColor;
                case "dikirim":
                    return OrangeColor;
                case "selesai":
                    return SuccessColor;
                case "dibatalkan":
                    return DangerColor;
                default:
                    return Color.Gray;
            }
        }

        // Format Currency
        public static string FormatCurrency(decimal amount)
        {
            return "Rp " + amount.ToString("N0");
        }

        // Style TextBox dengan modern look
        public static void StyleTextBox(TextBox txt)
        {
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = new Font("Segoe UI", 10);
            txt.Padding = new Padding(5);
        }

        // Style ComboBox
        public static void StyleComboBox(ComboBox cmb)
        {
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.Font = new Font("Segoe UI", 10);
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // Create Header Panel
        public static Panel CreateHeader(string title, int formWidth)
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = PrimaryColor
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Left = 15,
                Top = 10
            };

            header.Controls.Add(lblTitle);
            return header;
        }
    }
}
