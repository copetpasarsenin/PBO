using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Controllers;
using Tubes_Alia_Richard_714240035_714240047.Helpers;
using Tubes_Alia_Richard_714240035_714240047.Models;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public class CheckoutForm : Form
    {
        private PesananController pesananController = new PesananController();
        private MidtransService midtransService = new MidtransService();
        private int userId;
        private string userEmail;
        private string userName;
        private TextBox txtAlamat;
        private TextBox txtCatatan;
        private Label lblTotal;
        private RadioButton rbCOD;
        private RadioButton rbMidtrans;
        private DataGridView dgvSummary;

        public CheckoutForm(int userId, string userName = "Customer", string userEmail = "customer@email.com")
        {
            this.userId = userId;
            this.userName = userName;
            this.userEmail = userEmail;
            InitializeComponent();
            LoadOrderSummary();
        }

        public CheckoutForm(int userId) : this(userId, "Customer", "customer@email.com")
        {
        }

        private void InitializeComponent()
        {
            this.Text = "Checkout";
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.WindowState = FormWindowState.Maximized;
            this.Padding = new Padding(20);

            // Main container with scroll
            Panel mainContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(245, 247, 250)
            };

            // Header
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = UIHelper.PrimaryColor
            };
            
            Label lblHeader = new Label
            {
                Text = "💳 CHECKOUT",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Left = 20,
                Top = 12
            };
            pnlHeader.Controls.Add(lblHeader);

            // Content Panel
            FlowLayoutPanel contentPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                Padding = new Padding(20)
            };

            // === RINGKASAN PESANAN ===
            Panel pnlSummary = CreateSection("📋 Ringkasan Pesanan", 180);
            
            dgvSummary = new DataGridView
            {
                Left = 15,
                Top = 45,
                Width = 900,
                Height = 120,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvSummary.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvSummary.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvSummary.ColumnHeadersDefaultCellStyle.BackColor = UIHelper.PrimaryColor;
            dgvSummary.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSummary.EnableHeadersVisualStyles = false;
            
            dgvSummary.Columns.Add("Produk", "Produk");
            dgvSummary.Columns.Add("Harga", "Harga");
            dgvSummary.Columns.Add("Qty", "Qty");
            dgvSummary.Columns.Add("Subtotal", "Subtotal");
            
            dgvSummary.Columns["Produk"].Width = 300;
            dgvSummary.Columns["Qty"].Width = 80;

            if (UserMainForm.Keranjang != null)
            {
                foreach (var item in UserMainForm.Keranjang)
                {
                    dgvSummary.Rows.Add(
                        item.NamaProduk,
                        UIHelper.FormatCurrency(item.Harga),
                        item.Jumlah,
                        UIHelper.FormatCurrency(item.Subtotal)
                    );
                }
            }
            pnlSummary.Controls.Add(dgvSummary);
            contentPanel.Controls.Add(pnlSummary);

            // === INFORMASI PENGIRIMAN ===
            Panel pnlShipping = CreateSection("📍 Informasi Pengiriman", 120);
            
            Label lblAlamat = new Label
            {
                Text = "Alamat Pengiriman: *",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Left = 15,
                Top = 45,
                AutoSize = true
            };
            
            txtAlamat = new TextBox
            {
                Left = 15,
                Top = 70,
                Width = 900,
                Height = 40,
                Multiline = true,
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };
            
            pnlShipping.Controls.Add(lblAlamat);
            pnlShipping.Controls.Add(txtAlamat);
            contentPanel.Controls.Add(pnlShipping);

            // === METODE PEMBAYARAN ===
            Panel pnlPayment = CreateSection("💰 Metode Pembayaran", 110);
            pnlPayment.BackColor = Color.FromArgb(255, 250, 230);

            rbCOD = new RadioButton
            {
                Text = "💵 COD (Cash on Delivery) - Bayar saat barang sampai",
                Font = new Font("Segoe UI", 11),
                Left = 15,
                Top = 45,
                Width = 500,
                Checked = true
            };

            rbMidtrans = new RadioButton
            {
                Text = "💳 Bayar Online (Midtrans) - Credit Card, Bank Transfer, e-Wallet",
                Font = new Font("Segoe UI", 11),
                Left = 15,
                Top = 75,
                Width = 600
            };

            pnlPayment.Controls.Add(rbCOD);
            pnlPayment.Controls.Add(rbMidtrans);
            contentPanel.Controls.Add(pnlPayment);

            // === TOTAL & KONFIRMASI ===
            Panel pnlTotal = new Panel
            {
                Width = 950,
                Height = 100,
                BackColor = UIHelper.PrimaryColor,
                Margin = new Padding(0, 10, 0, 0)
            };

            Label lblTotalTitle = new Label
            {
                Text = "TOTAL PEMBAYARAN",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Left = 20,
                Top = 15,
                AutoSize = true
            };

            lblTotal = new Label
            {
                Text = "Rp 0",
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.White,
                Left = 20,
                Top = 40,
                AutoSize = true
            };

            Button btnKonfirmasi = new Button
            {
                Text = "✓ KONFIRMASI PESANAN",
                Left = 700,
                Top = 25,
                Width = 230,
                Height = 55,
                BackColor = UIHelper.SuccessColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnKonfirmasi.FlatAppearance.BorderSize = 0;
            btnKonfirmasi.Click += BtnKonfirmasi_Click;

            pnlTotal.Controls.Add(lblTotalTitle);
            pnlTotal.Controls.Add(lblTotal);
            pnlTotal.Controls.Add(btnKonfirmasi);
            contentPanel.Controls.Add(pnlTotal);

            // Back button
            Button btnBatal = new Button
            {
                Text = "← Kembali",
                Width = 120,
                Height = 40,
                Margin = new Padding(0, 15, 0, 0)
            };
            UIHelper.StyleButton(btnBatal, UIHelper.DarkColor);
            btnBatal.Click += (s, e) => this.Close();
            contentPanel.Controls.Add(btnBatal);

            mainContainer.Controls.Add(contentPanel);
            
            this.Controls.Add(mainContainer);
            this.Controls.Add(pnlHeader);
        }

        private Panel CreateSection(string title, int height)
        {
            Panel panel = new Panel
            {
                Width = 950,
                Height = height,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 15)
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Left = 15,
                Top = 12,
                AutoSize = true
            };
            panel.Controls.Add(lblTitle);

            return panel;
        }

        private void LoadOrderSummary()
        {
            decimal total = UserMainForm.Keranjang.Sum(k => k.Subtotal);
            lblTotal.Text = UIHelper.FormatCurrency(total);
        }

        private async void BtnKonfirmasi_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAlamat.Text))
            {
                MessageBox.Show("Alamat pengiriman wajib diisi!", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAlamat.Focus();
                return;
            }

            if (UserMainForm.Keranjang.Count == 0)
            {
                MessageBox.Show("Keranjang kosong!", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string paymentMethod = rbMidtrans.Checked ? "Midtrans" : "COD";
            string confirmMsg = rbMidtrans.Checked 
                ? "Anda akan diarahkan ke halaman pembayaran Midtrans.\n\nLanjutkan?" 
                : "Yakin ingin melakukan pemesanan dengan COD?";

            if (MessageBox.Show(confirmMsg, "Konfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            decimal totalHarga = UserMainForm.Keranjang.Sum(k => k.Subtotal);

            if (rbMidtrans.Checked)
            {
                await ProcessMidtransPayment(totalHarga);
            }
            else
            {
                ProcessCODPayment(totalHarga);
            }
        }

        private async System.Threading.Tasks.Task ProcessMidtransPayment(decimal totalHarga)
        {
            try
            {
                string orderId = $"ORDER-{DateTime.Now:yyyyMMddHHmmss}-{userId}";

                var items = new List<MidtransItemDetail>();
                foreach (var item in UserMainForm.Keranjang)
                {
                    items.Add(new MidtransItemDetail
                    {
                        Id = item.ProdukId.ToString(),
                        Name = item.NamaProduk,
                        Price = item.Harga,
                        Quantity = item.Jumlah
                    });
                }

                this.Cursor = Cursors.WaitCursor;
                this.Enabled = false;

                var snapResponse = await midtransService.CreateSnapTransactionAsync(
                    orderId, totalHarga, userName, userEmail, items);

                this.Cursor = Cursors.Default;
                this.Enabled = true;

                if (snapResponse.Success)
                {
                    using (var paymentForm = new PaymentForm(snapResponse.RedirectUrl, orderId))
                    {
                        paymentForm.ShowDialog();

                        if (paymentForm.PaymentSuccess)
                        {
                            Pesanan pesanan = new Pesanan
                            {
                                UserId = userId,
                                TotalHarga = totalHarga,
                                AlamatPengiriman = txtAlamat.Text.Trim(),
                                Catatan = txtCatatan?.Text?.Trim() ?? "",
                                PaymentMethod = "Midtrans",
                                MidtransTransactionId = paymentForm.TransactionId,
                                PaymentStatus = "Paid"
                            };

                            int pesananId = pesananController.CreatePesananWithPayment(
                                pesanan, UserMainForm.Keranjang.ToList(), "Diproses");

                            if (pesananId > 0)
                            {
                                MessageBox.Show($"🎉 Pesanan dan Pembayaran Berhasil!\n\n" +
                                    $"ID Pesanan: #{pesananId}\n" +
                                    $"Total: {UIHelper.FormatCurrency(totalHarga)}\n" +
                                    $"Metode: Midtrans\n" +
                                    $"Status: Diproses",
                                    "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                UserMainForm.Keranjang.Clear();
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Pembayaran dibatalkan atau gagal.", "Info",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Gagal membuat transaksi:\n{snapResponse.ErrorMessage}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                this.Enabled = true;
                MessageBox.Show($"Error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessCODPayment(decimal totalHarga)
        {
            Pesanan pesanan = new Pesanan
            {
                UserId = userId,
                TotalHarga = totalHarga,
                AlamatPengiriman = txtAlamat.Text.Trim(),
                Catatan = txtCatatan?.Text?.Trim() ?? "",
                PaymentMethod = "COD",
                PaymentStatus = "Pending"
            };

            int pesananId = pesananController.CreatePesanan(pesanan, UserMainForm.Keranjang.ToList());

            if (pesananId > 0)
            {
                MessageBox.Show($"🎉 Pesanan berhasil dibuat!\n\n" +
                    $"ID Pesanan: #{pesananId}\n" +
                    $"Total: {UIHelper.FormatCurrency(pesanan.TotalHarga)}\n" +
                    $"Metode: COD (Bayar di tempat)\n" +
                    $"Status: Pending",
                    "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                UserMainForm.Keranjang.Clear();
                this.Close();
            }
        }
    }
}
