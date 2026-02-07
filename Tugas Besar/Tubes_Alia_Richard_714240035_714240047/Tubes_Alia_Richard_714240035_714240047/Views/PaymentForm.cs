using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Helpers;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public class PaymentForm : Form
    {
        private MidtransService midtransService;
        private string orderId;
        private Timer checkStatusTimer;
        private int checkCount = 0;
        private Label lblStatus;
        private Button btnCheckStatus;
        
        public bool PaymentSuccess { get; private set; } = false;
        public string TransactionId { get; private set; }

        public PaymentForm(string snapRedirectUrl, string orderId)
        {
            this.orderId = orderId;
            this.midtransService = new MidtransService();
            InitializeComponent(snapRedirectUrl);
        }

        private void InitializeComponent(string snapRedirectUrl)
        {
            this.Text = "Pembayaran Midtrans";
            this.Width = 450;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // Header
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(0, 150, 136)
            };

            Label lblTitle = new Label
            {
                Text = "🔒 Pembayaran Aman",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Left = 15,
                Top = 15
            };
            pnlHeader.Controls.Add(lblTitle);
            this.Controls.Add(pnlHeader);

            // Content Panel
            Panel pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30)
            };

            Label lblInfo = new Label
            {
                Text = "Browser akan terbuka untuk proses pembayaran.\n\n" +
                       "Setelah selesai membayar, klik tombol\n'Cek Status Pembayaran' di bawah.",
                Font = new Font("Segoe UI", 11),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 100
            };
            pnlContent.Controls.Add(lblInfo);

            lblStatus = new Label
            {
                Text = "⏳ Menunggu pembayaran...",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 152, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50,
                Top = 110
            };
            pnlContent.Controls.Add(lblStatus);

            // Buttons Panel
            Panel pnlButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 130
            };

            btnCheckStatus = new Button
            {
                Text = "🔄 Cek Status Pembayaran",
                Width = 280,
                Height = 45,
                Left = 55,
                Top = 10,
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCheckStatus.FlatAppearance.BorderSize = 0;
            btnCheckStatus.Click += BtnCheckStatus_Click;
            pnlButtons.Controls.Add(btnCheckStatus);

            Button btnOpenBrowser = new Button
            {
                Text = "🌐 Buka Halaman Pembayaran",
                Width = 280,
                Height = 40,
                Left = 55,
                Top = 60,
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnOpenBrowser.FlatAppearance.BorderSize = 0;
            btnOpenBrowser.Click += (s, e) => OpenBrowser(snapRedirectUrl);
            pnlButtons.Controls.Add(btnOpenBrowser);

            pnlContent.Controls.Add(pnlButtons);
            this.Controls.Add(pnlContent);

            // Footer
            Panel pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            Button btnCancel = new Button
            {
                Text = "✖ Batalkan",
                Width = 150,
                Height = 35,
                Left = 135,
                Top = 8,
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => 
            {
                if (MessageBox.Show("Yakin ingin membatalkan pembayaran?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    PaymentSuccess = false;
                    this.Close();
                }
            };
            pnlFooter.Controls.Add(btnCancel);
            this.Controls.Add(pnlFooter);

            // Auto-open browser
            OpenBrowser(snapRedirectUrl);

            // Timer untuk check status otomatis
            checkStatusTimer = new Timer { Interval = 5000 };
            checkStatusTimer.Tick += async (s, e) =>
            {
                checkCount++;
                if (checkCount > 36) // 3 menit
                {
                    checkStatusTimer.Stop();
                    return;
                }
                await CheckPaymentStatusAsync(false);
            };
            checkStatusTimer.Start();

            this.FormClosing += (s, e) =>
            {
                checkStatusTimer?.Stop();
                checkStatusTimer?.Dispose();
            };
        }

        private void OpenBrowser(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal membuka browser: {ex.Message}\n\nSilakan copy URL ini:\n{url}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnCheckStatus_Click(object sender, EventArgs e)
        {
            btnCheckStatus.Enabled = false;
            btnCheckStatus.Text = "⏳ Mengecek...";
            
            await CheckPaymentStatusAsync(true);
            
            btnCheckStatus.Enabled = true;
            btnCheckStatus.Text = "🔄 Cek Status Pembayaran";
        }

        private async System.Threading.Tasks.Task CheckPaymentStatusAsync(bool showMessage)
        {
            try
            {
                var status = await midtransService.GetTransactionStatusAsync(orderId);
                
                if (midtransService.IsPaymentSuccess(status.TransactionStatus, status.FraudStatus))
                {
                    checkStatusTimer.Stop();
                    PaymentSuccess = true;
                    TransactionId = status.TransactionId;
                    
                    lblStatus.Text = "✅ Pembayaran Berhasil!";
                    lblStatus.ForeColor = Color.FromArgb(76, 175, 80);
                    
                    MessageBox.Show("🎉 Pembayaran berhasil!\n\n" +
                        $"ID Transaksi: {status.TransactionId}\n" +
                        $"Metode: {status.PaymentType}",
                        "Pembayaran Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else if (midtransService.IsPaymentPending(status.TransactionStatus))
                {
                    lblStatus.Text = "⏳ Menunggu pembayaran...";
                    lblStatus.ForeColor = Color.FromArgb(255, 152, 0);
                    
                    if (showMessage)
                    {
                        MessageBox.Show("Pembayaran belum selesai.\n\n" +
                            "Silakan selesaikan pembayaran di browser, lalu cek status lagi.",
                            "Status Pending", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (showMessage)
                {
                    MessageBox.Show($"Status: {status.TransactionStatus}\n\n" +
                        "Pembayaran belum berhasil.",
                        "Status Pembayaran", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                if (showMessage)
                {
                    MessageBox.Show($"Gagal mengecek status: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
