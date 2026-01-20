using System;
using System.Drawing;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Controllers;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public partial class ChangePasswordForm : Form
    {
        private UserController userController = new UserController();
        private int userId;

        public ChangePasswordForm(int userId)
        {
            this.userId = userId;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Ganti Password";
            this.Width = 500;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // Panel Header
            Panel pnlHeader = new Panel
            {
                Left = 0,
                Top = 0,
                Width = this.Width,
                Height = 70,
                BackColor = Color.FromArgb(41, 128, 185),
                Dock = DockStyle.Top
            };

            Label lblTitle = new Label
            {
                Text = "GANTI PASSWORD",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            pnlHeader.Controls.Add(lblTitle);

            // Panel Main
            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(30)
            };

            int topPos = 20;
            int spacing = 60;

            // Old Password
            Label lblOldPassword = new Label
            {
                Text = "Password Lama:",
                AutoSize = true,
                Left = 30,
                Top = topPos,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            TextBox txtOldPassword = new TextBox
            {
                Left = 30,
                Top = topPos + 25,
                Width = 400,
                Height = 35,
                Font = new Font("Arial", 10),
                PasswordChar = '*',
                BorderStyle = BorderStyle.FixedSingle
            };

            topPos += spacing;

            // New Password
            Label lblNewPassword = new Label
            {
                Text = "Password Baru:",
                AutoSize = true,
                Left = 30,
                Top = topPos,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            TextBox txtNewPassword = new TextBox
            {
                Left = 30,
                Top = topPos + 25,
                Width = 400,
                Height = 35,
                Font = new Font("Arial", 10),
                PasswordChar = '*',
                BorderStyle = BorderStyle.FixedSingle
            };

            topPos += spacing;

            // Confirm New Password
            Label lblConfirmPassword = new Label
            {
                Text = "Konfirmasi Password Baru:",
                AutoSize = true,
                Left = 30,
                Top = topPos,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            TextBox txtConfirmPassword = new TextBox
            {
                Left = 30,
                Top = topPos + 25,
                Width = 400,
                Height = 35,
                Font = new Font("Arial", 10),
                PasswordChar = '*',
                BorderStyle = BorderStyle.FixedSingle
            };

            topPos += spacing;

            // Buttons
            Button btnSave = new Button
            {
                Text = "SIMPAN",
                Left = 30,
                Top = topPos,
                Width = 190,
                Height = 40,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;

            Button btnCancel = new Button
            {
                Text = "BATAL",
                Left = 240,
                Top = topPos,
                Width = 190,
                Height = 40,
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            pnlMain.Controls.Add(lblOldPassword);
            pnlMain.Controls.Add(txtOldPassword);
            pnlMain.Controls.Add(lblNewPassword);
            pnlMain.Controls.Add(txtNewPassword);
            pnlMain.Controls.Add(lblConfirmPassword);
            pnlMain.Controls.Add(txtConfirmPassword);
            pnlMain.Controls.Add(btnSave);
            pnlMain.Controls.Add(btnCancel);

            this.Controls.Add(pnlMain);
            this.Controls.Add(pnlHeader);

            // Event Handlers
            btnSave.Click += (s, e) =>
            {
                if (!ValidateInput(txtOldPassword, txtNewPassword, txtConfirmPassword))
                    return;

                if (userController.ChangePassword(userId, txtOldPassword.Text, txtNewPassword.Text))
                {
                    MessageBox.Show("Password berhasil diubah!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            };

            btnCancel.Click += (s, e) => this.Close();
        }

        private bool ValidateInput(TextBox txtOldPassword, TextBox txtNewPassword, TextBox txtConfirmPassword)
        {
            if (string.IsNullOrEmpty(txtOldPassword.Text))
            {
                MessageBox.Show("Password lama tidak boleh kosong!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOldPassword.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtNewPassword.Text) || txtNewPassword.Text.Length < 6)
            {
                MessageBox.Show("Password baru minimal 6 karakter!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return false;
            }

            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Konfirmasi password tidak cocok!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return false;
            }

            if (txtOldPassword.Text == txtNewPassword.Text)
            {
                MessageBox.Show("Password baru tidak boleh sama dengan password lama!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}