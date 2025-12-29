using System;
using System.Windows.Forms;
using P9_714240047.controller; // Panggil folder controller

namespace P9_714240047.view
{
    public partial class FormLogin : Form
    {
        CekLogin login = new CekLogin(); // Objek controller

        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Validasi kosong
            if (string.IsNullOrWhiteSpace(tbUsername.Text) || string.IsNullOrWhiteSpace(tbPassword.Text))
            {
                MessageBox.Show("Username & Password harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cek ke Database
            if (login.cek_login(tbUsername.Text, tbPassword.Text))
            {
                MessageBox.Show("Login Berhasil!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Buka ParentForm (Menu Utama)
                ParentForm pform = new ParentForm();
                this.Hide();
                pform.Show();
            }
            else
            {
                MessageBox.Show("Username atau Password salah", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}