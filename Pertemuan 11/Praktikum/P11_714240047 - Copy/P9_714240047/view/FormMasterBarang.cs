using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using P9_714240047.controller;
using P9_714240047.model;

namespace P9_714240047.view
{
    public partial class FormMasterBarang : Form
    {
        Barang controller = new Barang();
        string idBarang = "";

        public FormMasterBarang()
        {
            InitializeComponent();
        }

        private void Tampil()
        {
            dataGridView1.DataSource = controller.Tampil();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ResetForm()
        {
            txtNamaBarang.Text = "";
            txtHarga.Text = "";
            txtCari.Text = "";
            idBarang = "";
            btnSimpan.Enabled = true;
            btnUbah.Enabled = false;
            btnHapus.Enabled = false;
        }

        private void FormMasterBarang_Load(object sender, EventArgs e)
        {
            Tampil();
            ResetForm();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ResetForm();
            Tampil();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (txtNamaBarang.Text == "" || txtHarga.Text == "")
            {
                MessageBox.Show("Data tidak boleh kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                M_barang brg = new M_barang();
                brg.Nama_barang = txtNamaBarang.Text;
                brg.Harga = txtHarga.Text;

                if (controller.Insert(brg))
                {
                    MessageBox.Show("Data Berhasil Ditambahkan", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetForm();
                    Tampil();
                }
            }
        }

        private void btnUbah_Click(object sender, EventArgs e)
        {
            if (idBarang == "" || txtNamaBarang.Text == "" || txtHarga.Text == "")
            {
                MessageBox.Show("Pilih data dulu dari tabel!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                M_barang brg = new M_barang();
                brg.Nama_barang = txtNamaBarang.Text;
                brg.Harga = txtHarga.Text;

                if (controller.Update(brg, idBarang))
                {
                    MessageBox.Show("Data Berhasil Diubah", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetForm();
                    Tampil();
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (idBarang == "")
            {
                MessageBox.Show("Pilih data dulu untuk dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (controller.Delete(idBarang))
                    {
                        MessageBox.Show("Data Berhasil Dihapus", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetForm();
                        Tampil();
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                idBarang = row.Cells["id_barang"].Value.ToString();
                txtNamaBarang.Text = row.Cells["nama_barang"].Value.ToString();
                txtHarga.Text = row.Cells["harga"].Value.ToString();

                btnSimpan.Enabled = false;
                btnUbah.Enabled = true;
                btnHapus.Enabled = true;
            }
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