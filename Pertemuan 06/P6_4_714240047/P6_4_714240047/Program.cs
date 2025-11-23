using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;

namespace P6_4_714240047
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public class MainForm : Form
    {
        // Controls
        TextBox txtNumeric, txtChar, txtRequired, txtRegex, txtCompare1, txtCompare2, txtLength, txtUpper, txtLower;
        Label lblResult;
        Button btnSubmit, btnClear;

        public MainForm()
        {
            Text = "P6_4_714240047 - Form Validasi";
            Width = 720;
            Height = 640;
            StartPosition = FormStartPosition.CenterScreen;

            InitializeControls();
        }

        void InitializeControls()
        {
            int leftLabel = 20, leftInput = 180, top = 20, gapY = 42;
            Font labelFont = new Font("Segoe UI", 9);

            void AddLabel(string text, int y)
            {
                var lbl = new Label() { Text = text, Left = leftLabel, Top = y + 6, Width = 150, Font = labelFont };
                Controls.Add(lbl);
            }

            // Create controls (no PlaceholderText property)
            txtNumeric = new TextBox() { Left = leftInput, Top = top, Width = 460, Name = "txtNumeric" };
            AddLabel("Numeric Textbox:", top);
            Controls.Add(txtNumeric);
            SetPlaceholder(txtNumeric, "Hanya angka (contoh: 12345)");

            top += gapY;
            txtChar = new TextBox() { Left = leftInput, Top = top, Width = 460, Name = "txtChar" };
            AddLabel("Char Textbox:", top);
            Controls.Add(txtChar);
            SetPlaceholder(txtChar, "Hanya huruf (contoh: AlfaBeta)");

            top += gapY;
            txtRequired = new TextBox() { Left = leftInput, Top = top, Width = 460, Name = "txtRequired" };
            AddLabel("Required (wajib):", top);
            Controls.Add(txtRequired);
            SetPlaceholder(txtRequired, "Wajib diisi");

            top += gapY;
            txtRegex = new TextBox() { Left = leftInput, Top = top, Width = 460, Name = "txtRegex" };
            AddLabel("Regex (email):", top);
            Controls.Add(txtRegex);
            SetPlaceholder(txtRegex, "Email valid (contoh: nama@domain.com)");

            top += gapY;
            // For passwords: start with UseSystemPasswordChar = false so placeholder visible.
            txtCompare1 = new TextBox() { Left = leftInput, Top = top, Width = 460, Name = "txtCompare1", UseSystemPasswordChar = false };
            AddLabel("Password:", top);
            Controls.Add(txtCompare1);
            SetPlaceholder(txtCompare1, "Password", isPassword: true);

            top += gapY;
            txtCompare2 = new TextBox() { Left = leftInput, Top = top, Width = 460, Name = "txtCompare2", UseSystemPasswordChar = false };
            AddLabel("Confirm Password:", top);
            Controls.Add(txtCompare2);
            SetPlaceholder(txtCompare2, "Confirm Password", isPassword: true);

            top += gapY;
            txtLength = new TextBox() { Left = leftInput, Top = top, Width = 460, Name = "txtLength" };
            AddLabel("Length (5-10):", top);
            Controls.Add(txtLength);
            SetPlaceholder(txtLength, "Harus 5-10 karakter");

            top += gapY;
            txtUpper = new TextBox() { Left = leftInput, Top = top, Width = 460, Name = "txtUpper" };
            AddLabel("Upper Case Textbox:", top);
            Controls.Add(txtUpper);
            SetPlaceholder(txtUpper, "Akan otomatis UPPERCASE saat keluar");

            top += gapY;
            txtLower = new TextBox() { Left = leftInput, Top = top, Width = 460, Name = "txtLower" };
            AddLabel("Lower Case Textbox:", top);
            Controls.Add(txtLower);
            SetPlaceholder(txtLower, "Akan otomatis lowercase saat keluar");

            // Buttons
            btnSubmit = new Button() { Text = "Submit", Left = leftInput, Top = top + gapY + 8, Width = 140 };
            btnSubmit.Click += BtnSubmit_Click;
            Controls.Add(btnSubmit);

            btnClear = new Button() { Text = "Clear", Left = leftInput + 160, Top = top + gapY + 8, Width = 140 };
            btnClear.Click += (s, e) => ClearFields();
            Controls.Add(btnClear);

            lblResult = new Label() { Left = leftInput, Top = top + gapY + 56, Width = 460, Height = 150, AutoSize = false, BorderStyle = BorderStyle.FixedSingle };
            Controls.Add(lblResult);

            // Event handlers for live behavior
            txtNumeric.KeyPress += TxtNumeric_KeyPress;
            txtChar.KeyPress += TxtChar_KeyPress;
            txtUpper.Leave += (s, e) => { if (!IsPlaceholder(txtUpper)) txtUpper.Text = txtUpper.Text.ToUpperInvariant(); };
            txtLower.Leave += (s, e) => { if (!IsPlaceholder(txtLower)) txtLower.Text = txtLower.Text.ToLowerInvariant(); };
        }

        // Improved Placeholder helper: supports password-type textboxes and stable caret behavior
        void SetPlaceholder(TextBox tb, string placeholder, bool isPassword = false)
        {
            tb.Tag = placeholder; // store placeholder text in Tag
            tb.ForeColor = Color.Gray;
            tb.Text = placeholder;

            // show placeholder unmasked
            if (isPassword)
            {
                tb.UseSystemPasswordChar = false;
                tb.PasswordChar = '\0';
            }

            tb.GotFocus += (s, e) =>
            {
                // jika placeholder aktif, bersihkan dan siap ketik
                if (tb.Text == placeholder)
                {
                    tb.Text = "";
                    tb.ForeColor = Color.Black;

                    if (isPassword)
                    {
                        // aktifkan masking; gunakan PasswordChar agar kompatibel di .NET Framework
                        tb.PasswordChar = '\u25CF'; // bullet char
                        tb.UseSystemPasswordChar = false;
                    }

                    // letakkan kursor di akhir teks (siap untuk mengetik)
                    tb.SelectionStart = tb.Text.Length;
                    tb.SelectionLength = 0;
                }
                else
                {
                    // jika sudah ada teks, pastikan caret di akhir
                    tb.SelectionStart = tb.Text.Length;
                    tb.SelectionLength = 0;
                }
            };

            tb.LostFocus += (s, e) =>
            {
                // jika kosong, kembali ke placeholder dan non-mask
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = placeholder;
                    tb.ForeColor = Color.Gray;

                    if (isPassword)
                    {
                        tb.PasswordChar = '\0';
                        tb.UseSystemPasswordChar = false;
                    }
                }
            };

            // tambahan: saat user menghapus semua teks via keyboard, restore placeholder on next LostFocus
            tb.KeyDown += (s, e) =>
            {
                // nothing special here, but keep handler to ensure events stay wired
            };
        }

        bool IsPlaceholder(TextBox tb)
        {
            return tb.Tag != null && tb.Text == (string)tb.Tag;
        }

        // Update ClearFields (supaya password fields juga diset PasswordChar = '\0')
        private void ClearFields()
        {
            foreach (Control c in Controls)
            {
                if (c is TextBox tb && tb.Tag is string ph)
                {
                    tb.Text = ph;
                    tb.ForeColor = Color.Gray;
                    tb.PasswordChar = '\0';
                    tb.UseSystemPasswordChar = false;
                    tb.SelectionStart = 0;
                }
            }
            lblResult.Text = "";
        }

        private void TxtChar_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allow control keys (backspace) and letters + spaces
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void TxtNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allow control keys (backspace) and digits only
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            var errors = new StringBuilder();

            // Helper get real text (empty if placeholder)
            string GetReal(TextBox tb) => (tb.Tag is string ph && tb.Text == ph) ? "" : tb.Text;

            // Required validator
            if (string.IsNullOrWhiteSpace(GetReal(txtRequired)))
                errors.AppendLine("- Field 'Required' harus diisi.");

            // Numeric
            var numericText = GetReal(txtNumeric);
            if (string.IsNullOrWhiteSpace(numericText) || !Regex.IsMatch(numericText, @"^\d+$"))
                errors.AppendLine("- Numeric textbox harus berisi angka (0-9) dan tidak kosong.");

            // Char textbox (only letters/spaces)
            var charText = GetReal(txtChar);
            if (string.IsNullOrWhiteSpace(charText) || !Regex.IsMatch(charText, @"^[A-Za-z\s]+$"))
                errors.AppendLine("- Char textbox harus berisi huruf (A-Z) dan tidak kosong.");

            // Regex validator (email example) -- safe single-line regex
            string email = GetReal(txtRegex)?.Trim() ?? "";
            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errors.AppendLine("- Regex validator: masukkan email valid (contoh: nama@domain.com).");

            // Comparison validator (passwords match)
            string pass1 = GetReal(txtCompare1);
            string pass2 = GetReal(txtCompare2);
            if (pass1 != pass2)
                errors.AppendLine("- Password dan Confirm Password harus sama.");

            // Length validator (min 5 max 10)
            string lengthText = GetReal(txtLength) ?? "";
            int len = lengthText.Length;
            if (len < 5 || len > 10)
                errors.AppendLine("- Length validator: isi harus antara 5 sampai 10 karakter.");

            // Upper/lower: ensure not empty (optional)
            if (string.IsNullOrWhiteSpace(GetReal(txtUpper)))
                errors.AppendLine("- Upper Case textbox sebaiknya tidak kosong (akan diubah ke UPPERCASE).");

            if (string.IsNullOrWhiteSpace(GetReal(txtLower)))
                errors.AppendLine("- Lower Case textbox sebaiknya tidak kosong (akan diubah ke lowercase).");

            if (errors.Length > 0)
            {
                MessageBox.Show("Terdapat error:\n\n" + errors.ToString(), "Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Jika semua validasi lolos, tampilkan hasil form di MessageBox
            var result = new StringBuilder();
            result.AppendLine("Hasil input form:");
            result.AppendLine("----------------------------");
            result.AppendLine($"Numeric: {numericText}");
            result.AppendLine($"Char: {charText}");
            result.AppendLine($"Required: {GetReal(txtRequired)}");
            result.AppendLine($"Regex (email): {email}");
            result.AppendLine($"Password: {(string.IsNullOrEmpty(pass1) ? "(kosong)" : new string('*', pass1.Length))}");
            result.AppendLine($"Length field: {lengthText}");
            result.AppendLine($"Uppercase: {GetReal(txtUpper).ToUpperInvariant()}");
            result.AppendLine($"Lowercase: {GetReal(txtLower).ToLowerInvariant()}");

            MessageBox.Show(result.ToString(), "Form Submitted", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Tampilkan juga di label bawah form
            lblResult.Text = result.ToString();
        }
    }
}
