using System;
using System.Collections.Generic;
using System.Data;
using System.IO; // Tambahan untuk mengecek file
using System.Windows.Forms;
using P9_714240047.controller;
using P9_714240047.model;
using P9_714240047.lib; // Tambahan agar bisa memanggil class Excel

namespace P9_714240047.view
{
    public partial class FormTransaksi : Form
    {
        Transaksi controller = new Transaksi();
        string idTransaksi = "";

        public FormTransaksi()
        {
            InitializeComponent();
        }

        private void TampilData()
        {
            dataGridView1.DataSource = controller.TampilTransaksi();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Format Rupiah di Tabel
            dataGridView1.Columns["harga"].DefaultCellStyle.Format = "Rp #,###";
            dataGridView1.Columns["total"].DefaultCellStyle.Format = "Rp #,###";
        }

        private void LoadBarangCmb()
        {
            DataTable dt = controller.LoadBarang();
            cmbIdBarang.DataSource = dt;

            cmbIdBarang.DisplayMember = "id_barang";
            cmbIdBarang.ValueMember = "id_barang";

            cmbIdBarang.SelectedIndex = -1;
        }

        private void FormTransaksi_Load(object sender, EventArgs e)
        {
            TampilData();
            LoadBarangCmb();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                idTransaksi = row.Cells["id_transaksi"].Value.ToString();
                cmbIdBarang.SelectedValue = Convert.ToInt32(row.Cells["id_barang"].Value);
                txtNamaBarang.Text = row.Cells["nama_barang"].Value.ToString();

                string hargaStr = row.Cells["harga"].Value.ToString();
                txtHargaBarang.Text = hargaStr;

                txtQty.Text = row.Cells["qty"].Value.ToString();
                txtTotal.Text = row.Cells["total"].Value.ToString();

                btnSimpan.Enabled = false;
                btnUbah.Enabled = true;
                btnHapus.Enabled = true;
            }
        }

        private void cmbIdBarang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbIdBarang.SelectedValue != null)
            {
                int id;
                if (int.TryParse(cmbIdBarang.SelectedValue.ToString(), out id))
                {
                    string nama, harga;
                    controller.GetBarangDetail(id.ToString(), out nama, out harga);

                    txtNamaBarang.Text = nama;
                    txtHargaBarang.Text = harga;
                    HitungTotal();
                }
            }
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            HitungTotal();
        }

        private void HitungTotal()
        {
            int harga = 0;
            int qty = 0;
            string hargaClean = txtHargaBarang.Text.Replace("Rp", "").Replace(".", "").Replace(",", "").Trim();

            int.TryParse(hargaClean, out harga);
            int.TryParse(txtQty.Text, out qty);

            int total = harga * qty;
            txtTotal.Text = total.ToString();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (cmbIdBarang.SelectedIndex == -1 || txtQty.Text == "")
            {
                MessageBox.Show("Data tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string idBarang = cmbIdBarang.SelectedValue.ToString();

            if (controller.CekBarangAda(idBarang))
            {
                MessageBox.Show("Data Sudah Ada, Silahkan Lakukan Update/Ubah", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            M_transaksi tr = new M_transaksi();
            tr.Id_barang = int.Parse(idBarang);
            tr.Qty = int.Parse(txtQty.Text);
            tr.Total = int.Parse(txtTotal.Text);

            if (controller.Insert(tr))
            {
                MessageBox.Show("Data Berhasil Disimpan", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnRefresh_Click(sender, e);
            }
        }

        private void btnUbah_Click(object sender, EventArgs e)
        {
            if (idTransaksi == "")
            {
                MessageBox.Show("Pilih data dulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            M_transaksi tr = new M_transaksi();
            tr.Id_barang = int.Parse(cmbIdBarang.SelectedValue.ToString());
            tr.Qty = int.Parse(txtQty.Text);
            tr.Total = int.Parse(txtTotal.Text);

            if (controller.Update(tr, idTransaksi))
            {
                MessageBox.Show("Data Berhasil Diubah", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnRefresh_Click(sender, e);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (idTransaksi == "")
            {
                MessageBox.Show("Pilih data dulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Yakin ingin menghapus?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (controller.Delete(idTransaksi))
                {
                    MessageBox.Show("Data Berhasil Dihapus", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnRefresh_Click(sender, e);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            TampilData();
            LoadBarangCmb();
            cmbIdBarang.SelectedIndex = -1;
            txtNamaBarang.Text = "";
            txtHargaBarang.Text = "";
            txtQty.Text = "";
            txtTotal.Text = "";
            txtCari.Text = "";
            idTransaksi = "";

            btnSimpan.Enabled = true;
            btnUbah.Enabled = false;
            btnHapus.Enabled = false;
        }

        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource != null)
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("nama_barang LIKE '%{0}%'", txtCari.Text);
            }
        }

        // --- TAMBAHAN UNTUK EXPORT EXCEL ---
        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel Documents (*.xlsx)|*.xlsx";
            save.FileName = "Report Transaksi Barang.xlsx";
            save.OverwritePrompt = false;

            if (save.ShowDialog() == DialogResult.OK)
            {
                string filePath = save.FileName;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                Excel excel_lib = new Excel();
                excel_lib.ExportToExcel(dataGridView1, filePath);

                MessageBox.Show(
                    "Data berhasil diekspor ke file Excel.",
                    "Informasi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }
    }
}