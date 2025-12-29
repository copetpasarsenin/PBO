using System;
using System.Windows.Forms;
using P9_714240047.view; // Pastikan namespace view terpanggil

namespace P9_714240047
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1. Jalankan Splash Screen dulu
            Application.Run(new StartUp());

            // 2. Setelah Splash Screen nutup, jalankan Form Login
            Application.Run(new FormLogin());
        }
    }
}