using System;
using System.Drawing;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Controllers;
using Tubes_Alia_Richard_714240035_714240047.Models;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public partial class RegisterForm : Form
    {
        private UserController userController = new UserController();

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Register - Sistem Manajemen Produk";
            this.Width = 550;
            this.Height = 650;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // Panel header
            Panel pnlHeader = new Panel
            {
                Left = 0,
                Top = 0,
                Width = this.Width,
                Height = 80,
                BackColor = Color.FromArgb(52, 152, 219),
                Dock = DockStyle.Top
            };

            Label lblTitle = new Label
            {
                Text = "REGISTRASI USER BARU",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            pnlHeader.Controls.Add(lblTitle);

            // Panel utama
            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(30),
                AutoScroll = true
            };

            int topPosition = 20;
            int controlHeight = 35;
            int spacing = 50;

            // Full Name
            Label lblFullName = new Label
            {
                Text = "Nama Lengkap:",
                AutoSize = true,
                Left = 30,
                Top = topPosition,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            TextBox txtFullName = new TextBox
            {
                Left = 30,
                Top = topPosition + 25,
                Width = 450,
                Height = controlHeight,
                Font = new Font("Arial", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            topPosition += spacing;

            // Username
            Label lblUsername = new Label
            {
                Text = "Username:",
                AutoSize = true,
                Left = 30,
                Top = topPosition,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            TextBox txtUsername = new TextBox
            {
                Left = 30,
                Top = topPosition + 25,
                Width = 450,
                Height = controlHeight,
                Font = new Font("Arial", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            topPosition += spacing;

            // Email
            Label lblEmail = new Label
            {
                Text = "Email:",
                AutoSize = true,
                Left = 30,
                Top = topPosition,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            TextBox txtEmail = new TextBox
            {
                Left = 30,
                Top = topPosition + 25,
                Width = 450,
                Height = controlHeight,
                Font = new Font("Arial", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            topPosition += spacing;

            // Password
            Label lblPassword = new Label
            {
                Text = "Password:",
                AutoSize = true,
                Left = 30,
                Top = topPosition,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            TextBox txtPassword = new TextBox
            {
                Left = 30,
                Top = topPosition + 25,
                Width = 450,
                Height = controlHeight,
                Font = new Font("Arial", 10),
                PasswordChar = '*',
                BorderStyle = BorderStyle.FixedSingle
            };

            topPosition += spacing;

            // Confirm Password
            Label lblConfirmPassword = new Label
            {
                Text = "Konfirmasi Password:",
                AutoSize = true,
                Left = 30,
                Top = topPosition,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            TextBox txtConfirmPassword = new TextBox
            {
                Left = 30,
                Top = topPosition + 25,
                Width = 450,
                Height = controlHeight,
                Font = new Font("Arial", 10),
                PasswordChar = '*',
                BorderStyle = BorderStyle.FixedSingle
            };

            topPosition += spacing;

            // Role
            Label lblRole = new Label
            {
                Text = "Role:",
                AutoSize = true,
                Left = 30,
                Top = topPosition,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            ComboBox cmbRole = new ComboBox
            {
                Left = 30,
                Top = topPosition + 25,
                Width = 450,
                Height = controlHeight,
                Font = new Font("Arial", 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };
            cmbRole.Items.AddRange(new string[] { "Admin", "User" });
            cmbRole.SelectedIndex = 1;

            topPosition += spacing;

            // Tombol Register
            Button btnRegister = new Button
            {
                Text = "REGISTER",
                Left = 30,
                Top = topPosition,
                Width = 210,
                Height = 45,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;

            // Tombol Cancel
            Button btnCancel = new Button
            {
                Text = "CANCEL",
                Left = 270,
                Top = topPosition,
                Width = 210,
                Height = 45,
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            pnlMain.Controls.Add(lblFullName);
            pnlMain.Controls.Add(txtFullName);
            pnlMain.Controls.Add(lblUsername);
            pnlMain.Controls.Add(txtUsername);
            pnlMain.Controls.Add(lblEmail);
            pnlMain.Controls.Add(txtEmail);
            pnlMain.Controls.Add(lblPassword);
            pnlMain.Controls.Add(txtPassword);
            pnlMain.Controls.Add(lblConfirmPassword);
            pnlMain.Controls.Add(txtConfirmPassword);
            pnlMain.Controls.Add(lblRole);
            pnlMain.Controls.Add(cmbRole);
            pnlMain.Controls.Add(btnRegister);
            pnlMain.Controls.Add(btnCancel);

            this.Controls.Add(pnlMain);
            this.Controls.Add(pnlHeader);

            // Event Handlers
            btnRegister.Click += (s, e) =>
            {
                if (!ValidateRegisterForm(txtFullName, txtUsername, txtEmail, txtPassword, txtConfirmPassword))
                    return;

                var user = new User
                {
                    FullName = txtFullName.Text,
                    Username = txtUsername.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Text,
                    Role = "User"
                };

                if (userController.RegisterUser(user))
                {
                    MessageBox.Show("Registrasi berhasil! Silakan login.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            };

            btnCancel.Click += (s, e) => this.Close();
        }

        private bool ValidateRegisterForm(TextBox txtFullName, TextBox txtUsername, TextBox txtEmail, TextBox txtPassword, TextBox txtConfirmPassword)
        {
            if (string.IsNullOrEmpty(txtFullName.Text))
            {
                MessageBox.Show("Nama lengkap tidak boleh kosong!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtUsername.Text) || txtUsername.Text.Length < 3)
            {
                MessageBox.Show("Username minimal 3 karakter!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email tidak valid!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtPassword.Text) || txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Password minimal 6 karakter!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Password tidak cocok!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}