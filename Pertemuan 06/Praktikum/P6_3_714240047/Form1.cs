using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace P6_3_714240047
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SetErrorMessages(TextBox textBox, string warningMessage, string wrongMessage, string correctMessage)
        { 
            epWarning.SetError(textBox, warningMessage);
            epWrong.SetError(textBox, wrongMessage);
            epCorrect.SetError(textBox, correctMessage);
        }

        private void txtHuruf_Leave(object sender, EventArgs e)
        { 

        }
        private void txtHuruf_Leave_1(object sender, EventArgs e)
        {
            if (txtHuruf.Text == "")
            {
                SetErrorMessages(txtHuruf, "TextBox Huruf tidak boleh kosong", "", "");
            }
            else if (txtHuruf.Text.All(Char.IsLetter))
            {
                SetErrorMessages(txtHuruf, "", "", "Betul!");
            }
            else
            {
                SetErrorMessages(txtHuruf, "", "Inputan Hanya Boleh Huruf!", "");
            }
        }

        private void txtAngka_TextChanged(object sender, EventArgs e)
        {
            if (txtAngka.Text == "")
            { 
                SetErrorMessages(txtAngka, "TextBox Angka tidak boleh kosong", "", "");
            }
            else if (txtAngka.Text.All(Char.IsNumber))
            {
                SetErrorMessages(txtAngka, "", "", "Betul!");
            }
            else
            {
                SetErrorMessages(txtAngka, "", "Inputan Hanya Boleh Angka!", "");
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if (txtEmail.Text == "")
            {
                SetErrorMessages(txtEmail, "TextBox Email tidak boleh kosong", "", "");
            }
            else if (Regex.IsMatch(txtEmail.Text, @"^^[^@\s]+@[^@\s]+(\.[^@\s]+)+$"))
            {
                SetErrorMessages(txtEmail, "", "", "Betul!");
            }
            else
            {
                SetErrorMessages(txtEmail, "", "Format Email Salah!\nContoh: a@b.c", "");
            }
        }

        private void txtAngka1_Leave(object sender, EventArgs e)
        {
            if (txtAngka1.Text == "")
            {
                SetErrorMessages(txtAngka1, "Textbox Angka 1 tidak boleh kosong", "", "");
            }
            else if (txtAngka1.Text.All(Char.IsNumber))
            {
                SetErrorMessages(txtAngka1, "", "", "Betul!");

                // Cek apakah Angka 2 sudah diisi
                if (txtAngka2.Text == "")
                {
                    SetErrorMessages(txtAngka2, "Apakah Angka1 sudah terisi atau belum", "", "");
                }
            }
            else
            {
                SetErrorMessages(txtAngka1, "", "Inputan Hanya Boleh Angka!", "");
            }
        }

        private void txtAngka2_Leave(object sender, EventArgs e)
        {
            if (txtAngka2.Text == "")
            {
                SetErrorMessages(txtAngka2, "Textbox Angka 2 tidak boleh kosong", "", "");
                return;
            }

            if (!txtAngka2.Text.All(Char.IsNumber))
            {
                SetErrorMessages(txtAngka2, "", "Inputan Hanya Boleh Angka!", "");
                return;
            }

            // Cek apakah Angka 1 sudah diisi
            if (txtAngka1.Text == "")
            {
                SetErrorMessages(txtAngka1, "Textbox Angka 1 tidak boleh kosong", "", "");
                return;
            }

            // Konversi ke angka
            int angka1 = int.Parse(txtAngka1.Text);
            int angka2 = int.Parse(txtAngka2.Text);

            // Validasi: Angka 1 harus lebih besar dari Angka 2
            if (angka1 > angka2)
            {
                SetErrorMessages(txtAngka2, "", "", "Betul!");
            }
            else
            {
                SetErrorMessages(txtAngka2, "", "Angka 1 harus lebih besar dari Angka 2", "");
            }
        }
    }
}
