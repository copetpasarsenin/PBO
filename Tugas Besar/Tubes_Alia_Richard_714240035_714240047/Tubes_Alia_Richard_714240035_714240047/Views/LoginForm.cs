using System;
using System.Drawing;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Controllers;
using Tubes_Alia_Richard_714240035_714240047.Models;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public partial class LoginForm : Form
    {
        private UserController userController = new UserController();
        public User CurrentUser { get; set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Login - Sistem Manajemen Produk";
            this.Width = 500;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Panel utama
            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(30)
            };

            // Panel header dengan warna
            Panel pnlHeader = new Panel
            {
                Left = 0,
                Top = 0,
                Width = this.Width,
                Height = 80,
                BackColor = Color.FromArgb(41, 128, 185),
                Dock = DockStyle.Top
            };

            Label lblTitle = new Label
            {
                Text = "LOGIN",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            pnlHeader.Controls.Add(lblTitle);

            // Username
            Label lblUsername = new Label
            {
                Text = "Username:",
                AutoSize = true,
                Left = 30,
                Top = 110,
                Font = new Font("Arial", 11, FontStyle.Bold)
            };

            TextBox txtUsername = new TextBox
            {
                Left = 30,
                Top = 135,
                Width = 420,
                Height = 35,
                Font = new Font("Arial", 11),
                Padding = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Password
            Label lblPassword = new Label
            {
                Text = "Password:",
                AutoSize = true,
                Left = 30,
                Top = 185,
                Font = new Font("Arial", 11, FontStyle.Bold)
            };

            TextBox txtPassword = new TextBox
            {
                Left = 30,
                Top = 210,
                Width = 420,
                Height = 35,
                Font = new Font("Arial", 11),
                PasswordChar = '*',
                Padding = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Tombol Login
            Button btnLogin = new Button
            {
                Text = "LOGIN",
                Left = 30,
                Top = 270,
                Width = 200,
                Height = 45,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;

            // Tombol Register
            Button btnRegister = new Button
            {
                Text = "REGISTER",
                Left = 250,
                Top = 270,
                Width = 200,
                Height = 45,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Font = new Font("Arial", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;

            this.Controls.Add(pnlHeader);
            pnlMain.Controls.Add(lblUsername);
            pnlMain.Controls.Add(txtUsername);
            pnlMain.Controls.Add(lblPassword);
            pnlMain.Controls.Add(txtPassword);
            pnlMain.Controls.Add(btnLogin);
            pnlMain.Controls.Add(btnRegister);
            this.Controls.Add(pnlMain);

            // Event Handlers
            btnLogin.Click += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
                {
                    MessageBox.Show("Username dan password tidak boleh kosong!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                CurrentUser = userController.LoginUser(txtUsername.Text, txtPassword.Text);
                if (CurrentUser != null)
                {
                    MessageBox.Show($"Login berhasil! Selamat datang {CurrentUser.FullName}", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username atau password salah!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtUsername.Focus();
                }
            };

            btnRegister.Click += (s, e) =>
            {
                RegisterForm registerForm = new RegisterForm();
                registerForm.ShowDialog();
            };

            txtPassword.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Return)
                {
                    btnLogin.PerformClick();
                    e.SuppressKeyPress = true;
                }
            };
        }
    }
}