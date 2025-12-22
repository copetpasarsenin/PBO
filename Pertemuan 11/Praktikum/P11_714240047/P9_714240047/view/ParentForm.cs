using System;
using System.Windows.Forms;

namespace P9_714240047.view
{
    public partial class ParentForm : Form
    {
        public ParentForm()
        {
            InitializeComponent();
        }

        // Menu Exit [cite: 199-201]
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Menu Data Mahasiswa [cite: 208-212]
        private void dataMahasiswaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 FormMhs = new Form1();
            FormMhs.MdiParent = this; // Form1 menjadi anak dari ParentForm
            FormMhs.Show();
        }

        // Menu Data Nilai [cite: 540-544]
        private void dataNilaiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNilai FrmNilai = new FormNilai();
            FrmNilai.MdiParent = this;
            FrmNilai.Show();
        }

        }
    }
