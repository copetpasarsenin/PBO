using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Tubes_Alia_Richard_714240035_714240047.Views
{
    public class SplashScreen : Form
    {
        private ProgressBar progressBar;
        private BackgroundWorker backgroundWorker;
        private int progressValue = 0;

        public SplashScreen()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Width = 500;
            this.Height = 350;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(41, 128, 185);
            this.ControlBox = false;
            this.TopMost = true;

            // Panel untuk konten
            Panel pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(41, 128, 185)
            };

            // Label Title
            Label lblTitle = new Label
            {
                Text = "SISTEM MANAJEMEN\nPRODUK",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 100,
                Padding = new Padding(10)
            };

            // Label Version
            Label lblVersion = new Label
            {
                Text = "Version 1.0\nDibuat oleh: Alia & Richard",
                Font = new Font("Arial", 10),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(5)
            };

            // Progress Bar
            progressBar = new ProgressBar
            {
                Left = 50,
                Top = 180,
                Width = 400,
                Height = 20,
                Style = ProgressBarStyle.Continuous,
                Value = 0
            };

            // Label Loading
            Label lblLoading = new Label
            {
                Text = "Loading...",
                Font = new Font("Arial", 11, FontStyle.Italic),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Left = 50,
                Top = 210,
                Width = 400,
                Height = 30
            };

            pnlContent.Controls.Add(lblTitle);
            pnlContent.Controls.Add(lblVersion);
            pnlContent.Controls.Add(progressBar);
            pnlContent.Controls.Add(lblLoading);

            this.Controls.Add(pnlContent);

            // Setup BackgroundWorker
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.WorkerReportsProgress = true;
        }

        public void StartLoading()
        {
            backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
                progressValue = i;
                backgroundWorker.ReportProgress(i);
                System.Threading.Thread.Sleep(30); // Non-blocking delay
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }
    }
}