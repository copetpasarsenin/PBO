using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using P9_714240047.view;

namespace P9_714240047.view
{
    public partial class ParentForm : Form
    {
        public ParentForm()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // MENU 1: MASTER BARANG
        private void dataMasterBarangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Pastikan FormMasterBarang sudah dibuat sebelumnya (sesuai langkah sebelumnya)
            FormMasterBarang form = new FormMasterBarang();
            form.MdiParent = this;
            form.Show();
        }

        // MENU 2: TRANSAKSI
        private void dataTransaksiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ini akan memanggil FormTransaksi yang kodenya baru saja kita perbaiki di atas
            FormTransaksi form = new FormTransaksi();
            form.MdiParent = this;
            form.Show();
        }

        private void dataMasterBarangToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FormMasterBarang form = new FormMasterBarang();
            form.MdiParent = this;
            form.Show();
        }

        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

            private void dataMahasiswaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 FormMhs = new Form1();
            FormMhs.MdiParent = this; // Form1 menjadi anak dari ParentForm
            FormMhs.Show();
        }

        private void dataNilaiToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FormNilai FrmNilai = new FormNilai();
            FrmNilai.MdiParent = this;
            FrmNilai.Show();
        }
    }
    }
