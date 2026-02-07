using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Controllers;
using Tubes_Alia_Richard_714240035_714240047.Helpers;
using Tubes_Alia_Richard_714240035_714240047.Models;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public class KatalogForm : Form
    {
        private ProdukController produkController = new ProdukController();
        private KategoriController kategoriController = new KategoriController();
        private int userId;
        private FlowLayoutPanel flowProduk;
        private ComboBox cmbKategori;
        private TextBox txtSearch;
        private Label lblKeranjangCount;
        private Panel pnlPreview;
        private Produk selectedProduk;

        public KatalogForm(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            LoadKategori();
            LoadProduk();
            
            // Refresh cart badge when form becomes active
            this.Activated += (s, e) => UpdateCartBadge();
        }

        private void InitializeComponent()
        {
            this.Text = "Katalog Produk";
            this.Width = 1400;
            this.Height = 700;
            this.BackColor = Color.FromArgb(245, 247, 250);

            // Header
            Panel pnlHeader = UIHelper.CreateHeader("🛍️ KATALOG PRODUK", this.Width);
            this.Controls.Add(pnlHeader);

            // Cart Badge
            Panel cartBadge = new Panel
            {
                Left = this.Width - 150,
                Top = 8,
                Width = 130,
                Height = 35,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };

            Label lblCart = new Label
            {
                Text = "🛒",
                Font = new Font("Segoe UI", 16),
                Left = 5,
                Top = 3,
                AutoSize = true
            };

            lblKeranjangCount = new Label
            {
                Text = $"{(UserMainForm.Keranjang?.Sum(k => k.Jumlah) ?? 0)} item",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = UIHelper.DarkColor,
                Left = 45,
                Top = 8,
                AutoSize = true
            };

            cartBadge.Controls.Add(lblCart);
            cartBadge.Controls.Add(lblKeranjangCount);
            cartBadge.Click += (s, e) =>
            {
                KeranjangForm keranjang = new KeranjangForm(userId);
                keranjang.MdiParent = this.MdiParent;
                keranjang.Show();
            };
            pnlHeader.Controls.Add(cartBadge);

            // Filter Panel
            Panel pnlFilter = new Panel
            {
                Left = 15,
                Top = 60,
                Width = 1030,
                Height = 55,
                BackColor = Color.White
            };

            Label lblKategori = new Label
            {
                Text = "Kategori:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 15,
                Top = 17,
                AutoSize = true
            };

            cmbKategori = new ComboBox
            {
                Left = 90,
                Top = 12,
                Width = 180,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cmbKategori.SelectedIndexChanged += (s, e) => FilterProduk();

            Label lblSearch = new Label
            {
                Text = "Cari:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 290,
                Top = 17,
                AutoSize = true
            };

            txtSearch = new TextBox
            {
                Left = 330,
                Top = 12,
                Width = 200,
                Height = 30,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            Button btnSearch = new Button
            {
                Text = "🔍 Cari",
                Left = 545,
                Top = 10,
                Width = 80,
                Height = 32
            };
            UIHelper.StyleButton(btnSearch, UIHelper.PrimaryColor);
            btnSearch.Click += (s, e) => SearchProduk();

            Button btnReset = new Button
            {
                Text = "Reset",
                Left = 635,
                Top = 10,
                Width = 70,
                Height = 32
            };
            UIHelper.StyleButton(btnReset, UIHelper.DarkColor);
            btnReset.Click += (s, e) =>
            {
                txtSearch.Clear();
                cmbKategori.SelectedIndex = 0;
                LoadProduk();
            };

            pnlFilter.Controls.Add(lblKategori);
            pnlFilter.Controls.Add(cmbKategori);
            pnlFilter.Controls.Add(lblSearch);
            pnlFilter.Controls.Add(txtSearch);
            pnlFilter.Controls.Add(btnSearch);
            pnlFilter.Controls.Add(btnReset);
            this.Controls.Add(pnlFilter);

            // Flow Layout for Products (LEFT SIDE - 5 columns)
            flowProduk = new FlowLayoutPanel
            {
                Left = 15,
                Top = 125,
                Width = 1030,
                Height = 520,
                AutoScroll = true,
                BackColor = Color.FromArgb(245, 247, 250),
                Padding = new Padding(10)
            };
            this.Controls.Add(flowProduk);

            // Preview Panel (RIGHT SIDE)
            pnlPreview = new Panel
            {
                Left = 1060,
                Top = 60,
                Width = 320,
                Height = 585,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Initial state - show hint
            ShowPreviewHint();

            this.Controls.Add(pnlPreview);
        }

        private void ShowPreviewHint()
        {
            pnlPreview.Controls.Clear();

            Label lblHintIcon = new Label
            {
                Text = "👆",
                Font = new Font("Segoe UI", 48),
                Left = 120,
                Top = 180,
                AutoSize = true
            };

            Label lblHint = new Label
            {
                Text = "Klik produk untuk\nmelihat detail",
                Font = new Font("Segoe UI", 14),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Left = 60,
                Top = 260,
                Width = 200,
                Height = 60
            };

            pnlPreview.Controls.Add(lblHintIcon);
            pnlPreview.Controls.Add(lblHint);
        }

        private void ShowProductPreview(Produk produk)
        {
            selectedProduk = produk;
            pnlPreview.Controls.Clear();

            // Preview Title
            Label lblPreviewTitle = new Label
            {
                Text = "📦 DETAIL PRODUK",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = UIHelper.PrimaryColor,
                Left = 15,
                Top = 15,
                AutoSize = true
            };

            // Product Image
            Panel imgPanel = new Panel
            {
                Left = 35,
                Top = 50,
                Width = 250,
                Height = 120,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Check if product has image
            if (!string.IsNullOrEmpty(produk.Gambar) && System.IO.File.Exists(produk.Gambar))
            {
                try
                {
                    PictureBox picProduct = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Image = Image.FromFile(produk.Gambar)
                    };
                    imgPanel.Controls.Add(picProduct);
                }
                catch
                {
                    // If image fails to load, show default icon
                    Label lblImg = new Label
                    {
                        Text = "📦",
                        Font = new Font("Segoe UI", 48),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill
                    };
                    imgPanel.Controls.Add(lblImg);
                }
            }
            else
            {
                Label lblImg = new Label
                {
                    Text = "📦",
                    Font = new Font("Segoe UI", 48),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                imgPanel.Controls.Add(lblImg);
            }

            // Product Name
            Label lblNama = new Label
            {
                Text = produk.NamaProduk,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = UIHelper.DarkColor,
                Left = 15,
                Top = 180,
                Width = 290,
                Height = 40
            };

            // Category
            Label lblKategori = new Label
            {
                Text = $"📂 Kategori: {produk.NamaKategori}",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Left = 15,
                Top = 220,
                AutoSize = true
            };

            // Price
            Label lblHarga = new Label
            {
                Text = UIHelper.FormatCurrency(produk.Harga),
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = UIHelper.SuccessColor,
                Left = 15,
                Top = 245,
                AutoSize = true
            };

            // Stock
            Label lblStok = new Label
            {
                Text = produk.Stok > 0 ? $"✓ Stok tersedia: {produk.Stok} unit" : "✗ Stok habis",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = produk.Stok > 0 ? UIHelper.SuccessColor : UIHelper.DangerColor,
                Left = 15,
                Top = 280,
                AutoSize = true
            };

            // Description
            Label lblDeskLabel = new Label
            {
                Text = "📝 Deskripsi:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Left = 15,
                Top = 305,
                AutoSize = true
            };

            Label lblDeskripsi = new Label
            {
                Text = string.IsNullOrEmpty(produk.Deskripsi) ? "Tidak ada deskripsi" : produk.Deskripsi,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Left = 15,
                Top = 325,
                Width = 290,
                Height = 40
            };

            // Quantity selector
            Label lblQtyLabel = new Label
            {
                Text = "Jumlah:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Left = 15,
                Top = 380,
                AutoSize = true
            };

            NumericUpDown numQty = new NumericUpDown
            {
                Left = 75,
                Top = 375,
                Width = 70,
                Height = 28,
                Minimum = 1,
                Maximum = produk.Stok > 0 ? produk.Stok : 1,
                Value = 1,
                Font = new Font("Segoe UI", 11),
                Enabled = produk.Stok > 0
            };

            // Add to Cart Button
            Button btnAddToCart = new Button
            {
                Text = produk.Stok > 0 ? "🛒 TAMBAH KE KERANJANG" : "STOK HABIS",
                Left = 15,
                Top = 420,
                Width = 290,
                Height = 40,
                Enabled = produk.Stok > 0
            };
            UIHelper.StyleButton(btnAddToCart, produk.Stok > 0 ? UIHelper.SuccessColor : Color.Gray);
            btnAddToCart.Click += (s, e) => AddToCartWithQty(produk.ProdukId, (int)numQty.Value);

            pnlPreview.Controls.Add(lblPreviewTitle);
            pnlPreview.Controls.Add(imgPanel);
            pnlPreview.Controls.Add(lblNama);
            pnlPreview.Controls.Add(lblKategori);
            pnlPreview.Controls.Add(lblHarga);
            pnlPreview.Controls.Add(lblStok);
            pnlPreview.Controls.Add(lblDeskLabel);
            pnlPreview.Controls.Add(lblDeskripsi);
            pnlPreview.Controls.Add(lblQtyLabel);
            pnlPreview.Controls.Add(numQty);
            pnlPreview.Controls.Add(btnAddToCart);
        }

        private void LoadKategori()
        {
            cmbKategori.Items.Clear();
            cmbKategori.Items.Add("Semua Kategori");

            DataTable dt = kategoriController.GetKategoriDataTable();
            foreach (DataRow row in dt.Rows)
            {
                cmbKategori.Items.Add(row["Nama"].ToString());
            }

            cmbKategori.SelectedIndex = 0;
        }

        private void LoadProduk()
        {
            flowProduk.Controls.Clear();
            List<Produk> produkList = produkController.GetAllProduk();

            foreach (var produk in produkList)
            {
                Panel card = CreateClickableProductCard(produk);
                card.Margin = new Padding(10);
                flowProduk.Controls.Add(card);
            }

            if (produkList.Count == 0)
            {
                Label lblEmpty = new Label
                {
                    Text = "Tidak ada produk tersedia",
                    Font = new Font("Segoe UI", 14),
                    ForeColor = Color.Gray,
                    AutoSize = true
                };
                flowProduk.Controls.Add(lblEmpty);
            }
        }

        private Panel CreateClickableProductCard(Produk produk)
        {
            Panel card = new Panel
            {
                Width = 170,
                Height = 230,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Cursor = Cursors.Hand,
                Tag = produk.ProdukId
            };

            // Card shadow effect
            card.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(50, 0, 0, 0), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            // Product Image
            Panel imgPanel = new Panel
            {
                Left = 10,
                Top = 10,
                Width = 150,
                Height = 80,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Check if product has image
            if (!string.IsNullOrEmpty(produk.Gambar) && System.IO.File.Exists(produk.Gambar))
            {
                try
                {
                    PictureBox picProduct = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Image = Image.FromFile(produk.Gambar)
                    };
                    imgPanel.Controls.Add(picProduct);
                }
                catch
                {
                    Label lblImg = new Label
                    {
                        Text = "📦",
                        Font = new Font("Segoe UI", 28),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill
                    };
                    imgPanel.Controls.Add(lblImg);
                }
            }
            else
            {
                Label lblImg = new Label
                {
                    Text = "📦",
                    Font = new Font("Segoe UI", 28),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                imgPanel.Controls.Add(lblImg);
            }

            // Product Name
            Label lblNama = new Label
            {
                Text = produk.NamaProduk,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = UIHelper.DarkColor,
                Left = 10,
                Top = 95,
                Width = 150,
                Height = 35
            };

            // Category
            Label lblKat = new Label
            {
                Text = produk.NamaKategori,
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                Left = 10,
                Top = 130,
                Width = 150
            };

            // Price
            Label lblHarga = new Label
            {
                Text = UIHelper.FormatCurrency(produk.Harga),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = UIHelper.SuccessColor,
                Left = 10,
                Top = 150,
                Width = 150
            };

            // Stock
            Label lblStok = new Label
            {
                Text = produk.Stok > 0 ? $"Stok: {produk.Stok}" : "Habis",
                Font = new Font("Segoe UI", 8),
                ForeColor = produk.Stok > 0 ? Color.Gray : UIHelper.DangerColor,
                Left = 10,
                Top = 175,
                Width = 150
            };

            // Add to Cart Button
            Button btnAdd = new Button
            {
                Text = "🛒 Tambah",
                Left = 10,
                Top = 195,
                Width = 150,
                Height = 28,
                Enabled = produk.Stok > 0
            };
            UIHelper.StyleButton(btnAdd, produk.Stok > 0 ? UIHelper.PrimaryColor : Color.Gray);
            btnAdd.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnAdd.Click += (s, e) => AddToCart(produk.ProdukId);

            // Click event for card and all labels
            Action<object, EventArgs> cardClick = (s, e) => ShowProductPreview(produk);
            card.Click += new EventHandler(cardClick);
            imgPanel.Click += new EventHandler(cardClick);
            lblNama.Click += new EventHandler(cardClick);
            lblKat.Click += new EventHandler(cardClick);
            lblHarga.Click += new EventHandler(cardClick);
            lblStok.Click += new EventHandler(cardClick);

            // Hover effect
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(248, 250, 252);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

            card.Controls.Add(imgPanel);
            card.Controls.Add(lblNama);
            card.Controls.Add(lblKat);
            card.Controls.Add(lblHarga);
            card.Controls.Add(lblStok);
            card.Controls.Add(btnAdd);

            return card;
        }

        private void FilterProduk()
        {
            if (cmbKategori.SelectedIndex == 0)
            {
                LoadProduk();
                return;
            }

            flowProduk.Controls.Clear();
            List<Produk> allProduk = produkController.GetAllProduk();
            var filtered = allProduk.Where(p => p.NamaKategori == cmbKategori.SelectedItem.ToString()).ToList();

            foreach (var produk in filtered)
            {
                Panel card = CreateClickableProductCard(produk);
                card.Margin = new Padding(10);
                flowProduk.Controls.Add(card);
            }
        }

        private void SearchProduk()
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadProduk();
                return;
            }

            flowProduk.Controls.Clear();
            List<Produk> allProduk = produkController.GetAllProduk();
            var filtered = allProduk.Where(p => p.NamaProduk.ToLower().Contains(txtSearch.Text.ToLower())).ToList();

            foreach (var produk in filtered)
            {
                Panel card = CreateClickableProductCard(produk);
                card.Margin = new Padding(10);
                flowProduk.Controls.Add(card);
            }
        }

        private void AddToCart(int produkId)
        {
            AddToCartWithQty(produkId, 1);
        }

        private void AddToCartWithQty(int produkId, int qty)
        {
            Produk produk = produkController.GetProdukById(produkId);
            if (produk == null) return;

            // Check if already in cart
            var existingItem = UserMainForm.Keranjang.FirstOrDefault(k => k.ProdukId == produkId);
            
            if (existingItem != null)
            {
                if (existingItem.Jumlah + qty <= produk.Stok)
                {
                    existingItem.Jumlah += qty;
                    MessageBox.Show($"✓ Jumlah {produk.NamaProduk} ditambah!\n\nJumlah di keranjang: {existingItem.Jumlah}",
                        "Keranjang", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Stok tidak mencukupi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                UserMainForm.Keranjang.Add(new KeranjangItem
                {
                    ProdukId = produk.ProdukId,
                    NamaProduk = produk.NamaProduk,
                    NamaKategori = produk.NamaKategori,
                    Harga = produk.Harga,
                    Jumlah = qty,
                    StokTersedia = produk.Stok
                });
                MessageBox.Show($"✓ {qty}x {produk.NamaProduk} ditambahkan ke keranjang!",
                    "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            UpdateCartBadge();
        }

        private void UpdateCartBadge()
        {
            lblKeranjangCount.Text = $"{UserMainForm.Keranjang.Sum(k => k.Jumlah)} item";
        }
    }
}
