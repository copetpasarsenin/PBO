using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Helpers;
using Tubes_Alia_Richard_714240035_714240047.Models;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public class KeranjangForm : Form
    {
        private int userId;
        private DataGridView dgvKeranjang;
        private Label lblTotal;
        private Button btnCheckout;

        public KeranjangForm(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            LoadKeranjang();
        }

        private void InitializeComponent()
        {
            this.Text = "Keranjang Belanja";
            this.Width = 900;
            this.Height = 600;
            this.BackColor = Color.FromArgb(245, 247, 250);

            // Header
            Panel pnlHeader = UIHelper.CreateHeader("🛒 KERANJANG BELANJA", this.Width);
            this.Controls.Add(pnlHeader);

            // DataGridView
            dgvKeranjang = new DataGridView
            {
                Left = 15,
                Top = 65,
                Width = 860,
                Height = 380,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false
            };
            UIHelper.StyleDataGridView(dgvKeranjang);
            dgvKeranjang.ReadOnly = false;
            dgvKeranjang.CellValueChanged += DgvKeranjang_CellValueChanged;
            dgvKeranjang.CellContentClick += DgvKeranjang_CellContentClick;
            this.Controls.Add(dgvKeranjang);

            // Bottom Panel
            Panel pnlBottom = new Panel
            {
                Left = 15,
                Top = 460,
                Width = 860,
                Height = 90,
                BackColor = Color.White
            };

            // Empty Cart Button
            Button btnKosongkan = new Button
            {
                Text = "🗑️ Kosongkan Keranjang",
                Left = 15,
                Top = 30,
                Width = 180,
                Height = 40
            };
            UIHelper.StyleButton(btnKosongkan, UIHelper.DangerColor);
            btnKosongkan.Click += (s, e) =>
            {
                if (UserMainForm.Keranjang.Count > 0)
                {
                    if (MessageBox.Show("Yakin ingin mengosongkan keranjang?", "Konfirmasi",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        UserMainForm.Keranjang.Clear();
                        LoadKeranjang();
                    }
                }
            };

            // Total Label
            Label lblTotalTitle = new Label
            {
                Text = "TOTAL:",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Left = 500,
                Top = 15,
                AutoSize = true
            };

            lblTotal = new Label
            {
                Text = "Rp 0",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = UIHelper.SuccessColor,
                Left = 500,
                Top = 40,
                AutoSize = true
            };

            // Checkout Button
            btnCheckout = new Button
            {
                Text = "💳 CHECKOUT",
                Left = 700,
                Top = 20,
                Width = 150,
                Height = 50
            };
            UIHelper.StyleButton(btnCheckout, UIHelper.SuccessColor);
            btnCheckout.Click += BtnCheckout_Click;

            pnlBottom.Controls.Add(btnKosongkan);
            pnlBottom.Controls.Add(lblTotalTitle);
            pnlBottom.Controls.Add(lblTotal);
            pnlBottom.Controls.Add(btnCheckout);
            this.Controls.Add(pnlBottom);

            // Info label
            Label lblInfo = new Label
            {
                Text = "💡 Klik jumlah untuk mengubah quantity | Klik ❌ untuk menghapus item",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray,
                Left = 15,
                Top = 555,
                AutoSize = true
            };
            this.Controls.Add(lblInfo);
        }

        private void LoadKeranjang()
        {
            dgvKeranjang.Columns.Clear();
            dgvKeranjang.Rows.Clear();

            // Setup columns
            dgvKeranjang.Columns.Add("ProdukId", "ID");
            dgvKeranjang.Columns.Add("NamaProduk", "Produk");
            dgvKeranjang.Columns.Add("Harga", "Harga");

            DataGridViewTextBoxColumn colJumlah = new DataGridViewTextBoxColumn
            {
                Name = "Jumlah",
                HeaderText = "Qty",
                ReadOnly = false
            };
            dgvKeranjang.Columns.Add(colJumlah);

            dgvKeranjang.Columns.Add("Subtotal", "Subtotal");

            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn
            {
                Name = "Hapus",
                HeaderText = "",
                Text = "❌",
                UseColumnTextForButtonValue = true,
                Width = 50
            };
            dgvKeranjang.Columns.Add(btnDelete);

            // Hide ID column
            dgvKeranjang.Columns["ProdukId"].Visible = false;
            dgvKeranjang.Columns["Jumlah"].ReadOnly = false;

            // Add data
            foreach (var item in UserMainForm.Keranjang)
            {
                dgvKeranjang.Rows.Add(
                    item.ProdukId,
                    item.NamaProduk,
                    "Rp " + item.Harga.ToString("N0"),
                    item.Jumlah,
                    "Rp " + item.Subtotal.ToString("N0")
                );
            }

            UpdateTotal();

            // Empty state
            if (UserMainForm.Keranjang.Count == 0)
            {
                btnCheckout.Enabled = false;
            }
            else
            {
                btnCheckout.Enabled = true;
            }
        }

        private void DgvKeranjang_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvKeranjang.Columns[e.ColumnIndex].Name == "Jumlah")
            {
                int produkId = Convert.ToInt32(dgvKeranjang.Rows[e.RowIndex].Cells["ProdukId"].Value);
                int newQty = 1;

                if (int.TryParse(dgvKeranjang.Rows[e.RowIndex].Cells["Jumlah"].Value?.ToString(), out newQty))
                {
                    var item = UserMainForm.Keranjang.FirstOrDefault(k => k.ProdukId == produkId);
                    if (item != null)
                    {
                        if (newQty <= 0)
                        {
                            UserMainForm.Keranjang.Remove(item);
                        }
                        else if (newQty <= item.StokTersedia)
                        {
                            item.Jumlah = newQty;
                        }
                        else
                        {
                            MessageBox.Show($"Stok maksimal: {item.StokTersedia}", "Peringatan",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            item.Jumlah = item.StokTersedia;
                        }
                        LoadKeranjang();
                    }
                }
            }
        }

        private void DgvKeranjang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvKeranjang.Columns[e.ColumnIndex].Name == "Hapus")
            {
                int produkId = Convert.ToInt32(dgvKeranjang.Rows[e.RowIndex].Cells["ProdukId"].Value);
                var item = UserMainForm.Keranjang.FirstOrDefault(k => k.ProdukId == produkId);
                
                if (item != null)
                {
                    UserMainForm.Keranjang.Remove(item);
                    LoadKeranjang();
                }
            }
        }

        private void UpdateTotal()
        {
            decimal total = UserMainForm.Keranjang.Sum(k => k.Subtotal);
            lblTotal.Text = UIHelper.FormatCurrency(total);
        }

        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            if (UserMainForm.Keranjang.Count == 0)
            {
                MessageBox.Show("Keranjang kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CheckoutForm checkoutForm = new CheckoutForm(userId);
            checkoutForm.MdiParent = this.MdiParent;
            checkoutForm.FormClosed += (s, args) => LoadKeranjang();
            checkoutForm.Show();
        }
    }
}
