using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace P5_4_714240047
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // setting format tanggal
            dateTimePicker.Format = DateTimePickerFormat.Custom;
            dateTimePicker.CustomFormat = "dd-MM-yyyy";
        }

        private void btnTampilkan_Click(object sender, EventArgs e)
        {
            // VALIDASI JENIS KELAMIN (COMBOBOX)
            if (comboJnsKelamin.Text == "")
            {
                MessageBox.Show("Jenis kelamin belum diisi!", "Warning");
                return;
            }

            // VALIDASI KELAS (CHECKBOX OLAHRAGA)
            List<string> kelasDipilih = new List<string>();

            if (checkBoxSepakBola.Checked) kelasDipilih.Add(checkBoxSepakBola.Text);
            if (checkBoxRenang.Checked) kelasDipilih.Add(checkBoxRenang.Text);
            if (checkBoxTenis.Checked) kelasDipilih.Add(checkBoxTenis.Text);
            if (checkBoxYoga.Checked) kelasDipilih.Add(checkBoxYoga.Text);
            if (checkBoxBasket.Checked) kelasDipilih.Add(checkBoxBasket.Text);
            if (checkBoxBulutangkis.Checked) kelasDipilih.Add(checkBoxBulutangkis.Text);
            if (checkBoxVoli.Checked) kelasDipilih.Add(checkBoxVoli.Text);
            if (checkBoxPanahan.Checked) kelasDipilih.Add(checkBoxPanahan.Text);

            if (kelasDipilih.Count == 0)
            {
                MessageBox.Show("Pilihan kelas belum dipilih!", "Warning");
                return;
            }

            // VALIDASI JADWAL (RADIO)
            string jadwal = "";

            if (radioButton1.Checked) jadwal = radioButton1.Text;
            else if (radioButton2.Checked) jadwal = radioButton2.Text;
            else if (radioButton3.Checked) jadwal = radioButton3.Text;
            else if (radioButton4.Checked) jadwal = radioButton4.Text;

            if (jadwal == "")
            {
                MessageBox.Show("Pilihan jadwal belum dipilih!", "Warning");
                return;
            }

            // OUTPUT
            string result =
                $"Nama : {textNama.Text}\n" +
                $"Jenis Kelamin : {comboJnsKelamin.Text}\n" +
                $"Tanggal Lahir : {dateTimePicker.Value:dd-MM-yyyy}\n" +
                $"Kelas : {string.Join(", ", kelasDipilih)}\n" +
                $"Jadwal : {jadwal}";

            MessageBox.Show(result, "DATA PENDAFTARAN");
        }

        private void btnSelesai_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}