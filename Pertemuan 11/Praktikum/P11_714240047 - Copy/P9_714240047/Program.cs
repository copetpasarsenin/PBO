using P9_714240047.view;
using System;
using System.Windows.Forms;

namespace P9_714240047
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ParentForm()); // Pastikan tidak error merah di sini
        }
    }
}