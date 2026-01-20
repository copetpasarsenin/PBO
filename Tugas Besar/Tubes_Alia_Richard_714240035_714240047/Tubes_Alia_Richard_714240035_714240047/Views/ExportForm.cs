using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ClosedXML.Excel;
using Tubes_Alia_Richard_714240035_714240047.Controllers;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public partial class ExportForm : Form
    {
        private ProdukController produkController = new ProdukController();

        public ExportForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Export Data ke Excel";
            this.Width = 600;
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // Panel Header
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(41, 128, 185),
                Padding = new Padding(20)
            };

            Label lblTitle = new Label
            {
                Text = "EXPORT DATA KE EXCEL",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            pnlHeader.Controls.Add(lblTitle);

            // Panel Main
            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(30)
            };

            Label lblInfo = new Label
            {
                Text = "Pilih data yang ingin diekspor ke file Excel:",
                Font = new Font("Arial", 11),
                AutoSize = true,
                Left = 30,
                Top = 30
            };

            Button btnExportProduk = new Button
            {
                Text = "Export Data Produk",
                Left = 30,
                Top = 90,
                Width = 300,
                Height = 50,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExportProduk.FlatAppearance.BorderSize = 0;

            Label lblProdukDesc = new Label
            {
                Text = "Ekspor semua data produk dengan detail kategori, harga, dan stok",
                Font = new Font("Arial", 9),
                ForeColor = Color.FromArgb(127, 140, 141),
                Left = 30,
                Top = 145,
                Width = 300,
                Height = 40
            };

            Button btnExportKategori = new Button
            {
                Text = "Export Data Kategori",
                Left = 30,
                Top = 200,
                Width = 300,
                Height = 50,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExportKategori.FlatAppearance.BorderSize = 0;

            Label lblKategoriDesc = new Label
            {
                Text = "Ekspor semua kategori produk yang tersedia",
                Font = new Font("Arial", 9),
                ForeColor = Color.FromArgb(127, 140, 141),
                Left = 30,
                Top = 255,
                Width = 300,
                Height = 40
            };

            Button btnCancel = new Button
            {
                Text = "TUTUP",
                Left = 30,
                Top = 310,
                Width = 300,
                Height = 45,
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            pnlMain.Controls.Add(lblInfo);
            pnlMain.Controls.Add(btnExportProduk);
            pnlMain.Controls.Add(lblProdukDesc);
            pnlMain.Controls.Add(btnExportKategori);
            pnlMain.Controls.Add(lblKategoriDesc);
            pnlMain.Controls.Add(btnCancel);

            this.Controls.Add(pnlMain);
            this.Controls.Add(pnlHeader);

            // Event Handlers
            btnExportProduk.Click += (s, e) => ExportProdukToExcel();

            btnExportKategori.Click += (s, e) => ExportKategoriToExcel();

            btnCancel.Click += (s, e) => this.Close();
        }

        private void ExportProdukToExcel()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveFileDialog.FileName = $"Data_Produk_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dt = produkController.GetProdukDataTable();

                    var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Produk");

                    // Add header
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = dt.Columns[i].ColumnName;
                        worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                        worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(41, 128, 185);
                        worksheet.Cell(1, i + 1).Style.Font.FontColor = XLColor.White;
                    }

                    // Add data
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            worksheet.Cell(i + 2, j + 1).Value = dt.Rows[i][j];
                        }
                    }

                    worksheet.Columns().AdjustToContents();

                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show($"Data berhasil diekspor ke:\n{saveFileDialog.FileName}", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportKategoriToExcel()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveFileDialog.FileName = $"Data_Kategori_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var kategoriController = new KategoriController();
                    var data = kategoriController.GetAllKategori();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID");
                    dt.Columns.Add("Nama Kategori");
                    dt.Columns.Add("Deskripsi");

                    foreach (var item in data)
                    {
                        dt.Rows.Add(item.KategoriId, item.NamaKategori, item.Deskripsi);
                    }

                    var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Kategori");

                    // Add header
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = dt.Columns[i].ColumnName;
                        worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                        worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(41, 128, 185);
                        worksheet.Cell(1, i + 1).Style.Font.FontColor = XLColor.White;
                    }

                    // Add data
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            worksheet.Cell(i + 2, j + 1).Value = dt.Rows[i][j];
                        }
                    }

                    worksheet.Columns().AdjustToContents();

                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show($"Data berhasil diekspor ke:\n{saveFileDialog.FileName}", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}