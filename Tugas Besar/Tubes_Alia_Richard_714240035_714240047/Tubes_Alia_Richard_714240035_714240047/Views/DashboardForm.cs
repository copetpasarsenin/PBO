using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Controllers;
using Tubes_Alia_Richard_714240035_714240047.Helpers;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public class DashboardForm : Form
    {
        private ProdukController produkController = new ProdukController();
        private PesananController pesananController = new PesananController();

        public DashboardForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DashboardForm
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "DashboardForm";
            this.ResumeLayout(false);

        }

        private Panel CreateStatCard(string title, string value, Color bgColor, int left, int top)
        {
            Panel card = new Panel
            {
                Left = left,
                Top = top,
                Width = 280,
                Height = 150,
                BackColor = bgColor,
                BorderStyle = BorderStyle.None,
                Cursor = Cursors.Hand
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Left = 15,
                Top = 20
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Arial", 28, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Left = 15,
                Top = 50
            };

            card.Paint += (s, e) =>
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(200, 200, 200), 1), 0, 0, card.Width - 1, card.Height - 1);
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            return card;
        }
    }
}