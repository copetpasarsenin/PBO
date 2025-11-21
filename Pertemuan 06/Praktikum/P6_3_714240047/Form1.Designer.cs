namespace P6_3_714240047
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtHuruf = new System.Windows.Forms.TextBox();
            this.Huruf = new System.Windows.Forms.Label();
            this.Angka = new System.Windows.Forms.Label();
            this.Email = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Angka1 = new System.Windows.Forms.Label();
            this.Angka2 = new System.Windows.Forms.Label();
            this.txtAngka = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtAngka1 = new System.Windows.Forms.TextBox();
            this.txtAngka2 = new System.Windows.Forms.TextBox();
            this.epWarning = new System.Windows.Forms.ErrorProvider(this.components);
            this.epWrong = new System.Windows.Forms.ErrorProvider(this.components);
            this.epCorrect = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.epWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epWrong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epCorrect)).BeginInit();
            this.SuspendLayout();
            // 
            // txtHuruf
            // 
            this.txtHuruf.Location = new System.Drawing.Point(165, 61);
            this.txtHuruf.Name = "txtHuruf";
            this.txtHuruf.Size = new System.Drawing.Size(100, 20);
            this.txtHuruf.TabIndex = 0;
            this.txtHuruf.Leave += new System.EventHandler(this.txtHuruf_Leave_1);
            // 
            // Huruf
            // 
            this.Huruf.AutoSize = true;
            this.Huruf.Location = new System.Drawing.Point(70, 64);
            this.Huruf.Name = "Huruf";
            this.Huruf.Size = new System.Drawing.Size(33, 13);
            this.Huruf.TabIndex = 1;
            this.Huruf.Text = "Huruf";
            this.Huruf.Leave += new System.EventHandler(this.txtHuruf_Leave);
            // 
            // Angka
            // 
            this.Angka.AutoSize = true;
            this.Angka.Location = new System.Drawing.Point(70, 100);
            this.Angka.Name = "Angka";
            this.Angka.Size = new System.Drawing.Size(38, 13);
            this.Angka.TabIndex = 2;
            this.Angka.Text = "Angka";
            // 
            // Email
            // 
            this.Email.AutoSize = true;
            this.Email.Location = new System.Drawing.Point(70, 145);
            this.Email.Name = "Email";
            this.Email.Size = new System.Drawing.Size(32, 13);
            this.Email.TabIndex = 3;
            this.Email.Text = "Email";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(70, 202);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "*Angka 1 > Angka 2";
            // 
            // Angka1
            // 
            this.Angka1.AutoSize = true;
            this.Angka1.Location = new System.Drawing.Point(70, 253);
            this.Angka1.Name = "Angka1";
            this.Angka1.Size = new System.Drawing.Size(47, 13);
            this.Angka1.TabIndex = 5;
            this.Angka1.Text = "Angka 1";
            // 
            // Angka2
            // 
            this.Angka2.AutoSize = true;
            this.Angka2.Location = new System.Drawing.Point(70, 303);
            this.Angka2.Name = "Angka2";
            this.Angka2.Size = new System.Drawing.Size(47, 13);
            this.Angka2.TabIndex = 6;
            this.Angka2.Text = "Angka 2";
            // 
            // txtAngka
            // 
            this.txtAngka.Location = new System.Drawing.Point(165, 100);
            this.txtAngka.Name = "txtAngka";
            this.txtAngka.Size = new System.Drawing.Size(100, 20);
            this.txtAngka.TabIndex = 7;
            this.txtAngka.TextChanged += new System.EventHandler(this.txtAngka_TextChanged);
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(165, 145);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(100, 20);
            this.txtEmail.TabIndex = 8;
            this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
            // 
            // txtAngka1
            // 
            this.txtAngka1.Location = new System.Drawing.Point(165, 253);
            this.txtAngka1.Name = "txtAngka1";
            this.txtAngka1.Size = new System.Drawing.Size(100, 20);
            this.txtAngka1.TabIndex = 9;
            this.txtAngka1.Leave += new System.EventHandler(this.txtAngka1_Leave);
            // 
            // txtAngka2
            // 
            this.txtAngka2.Location = new System.Drawing.Point(165, 300);
            this.txtAngka2.Name = "txtAngka2";
            this.txtAngka2.Size = new System.Drawing.Size(100, 20);
            this.txtAngka2.TabIndex = 10;
            this.txtAngka2.Leave += new System.EventHandler(this.txtAngka2_Leave);
            // 
            // epWarning
            // 
            this.epWarning.ContainerControl = this;
            this.epWarning.Icon = ((System.Drawing.Icon)(resources.GetObject("epWarning.Icon")));
            // 
            // epWrong
            // 
            this.epWrong.ContainerControl = this;
            this.epWrong.Icon = ((System.Drawing.Icon)(resources.GetObject("epWrong.Icon")));
            // 
            // epCorrect
            // 
            this.epCorrect.ContainerControl = this;
            this.epCorrect.Icon = ((System.Drawing.Icon)(resources.GetObject("epCorrect.Icon")));
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 381);
            this.Controls.Add(this.txtAngka2);
            this.Controls.Add(this.txtAngka1);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtAngka);
            this.Controls.Add(this.Angka2);
            this.Controls.Add(this.Angka1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Email);
            this.Controls.Add(this.Angka);
            this.Controls.Add(this.Huruf);
            this.Controls.Add(this.txtHuruf);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.epWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epWrong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epCorrect)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHuruf;
        private System.Windows.Forms.Label Huruf;
        private System.Windows.Forms.Label Angka;
        private System.Windows.Forms.Label Email;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Angka1;
        private System.Windows.Forms.Label Angka2;
        private System.Windows.Forms.TextBox txtAngka;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtAngka1;
        private System.Windows.Forms.TextBox txtAngka2;
        private System.Windows.Forms.ErrorProvider epWarning;
        private System.Windows.Forms.ErrorProvider epWrong;
        private System.Windows.Forms.ErrorProvider epCorrect;
    }
}

