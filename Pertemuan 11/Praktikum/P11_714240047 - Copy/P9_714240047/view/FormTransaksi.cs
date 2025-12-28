using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using P9_714240047.controller;
using P9_714240047.model;

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

            // --- PERUBAHAN DI SINI ---
            cmbIdBarang.DisplayMember = "id_barang"; // Sekarang yang tampil ANGKA ID-nya
            cmbIdBarang.ValueMember = "id_barang";   // Nilai di belakang layar juga ID
                                                     // -------------------------

            cmbIdBarang.SelectedIndex = -1;
        }

        private void FormTransaksi_Load(object sender, EventArgs e)
        {
            TampilData();
            LoadBarangCmb();
        }

        // --- LOGIC KLIK TABEL (DATA MASUK KE FORM) ---
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // 1. Ambil ID Transaksi 
                idTransaksi = row.Cells["id_transaksi"].Value.ToString();

                // 2. Set ComboBox ke ID Barang yang sesuai
                // Karena DisplayMember sekarang ID, dia akan otomatis nampilin angkanya
                cmbIdBarang.SelectedValue = Convert.ToInt32(row.Cells["id_barang"].Value);

                // 3. Isi Nama & Harga manual dari tabel (biar cepat muncul)
                txtNamaBarang.Text = row.Cells["nama_barang"].Value.ToString();

                // Bersihkan format Rp biar rapi
                string hargaStr = row.Cells["harga"].Value.ToString();
                txtHargaBarang.Text = hargaStr;

                // 4. Isi Qty & Total
                txtQty.Text = row.Cells["qty"].Value.ToString();
                txtTotal.Text = row.Cells["total"].Value.ToString();

                // Atur Tombol
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
                    // Ambil detail barang berdasarkan ID yang dipilih
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
            // Bersihkan format non-angka dari Harga
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
                btnRefresh_Click(sender, e); // Reset form setelah simpan
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
    }
}