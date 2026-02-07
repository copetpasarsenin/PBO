using System;
using System.Threading;
using System.Windows.Forms;
using Tubes_Alia_Richard_714240035_714240047.Views;

namespace Tubes_Alia_Richard_714240035_714240047
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Show Splash Screen with loading animation
            SplashScreen splash = new SplashScreen();
            splash.Show();
            splash.StartLoading();
            
            Application.DoEvents();
            
            System.Threading.Thread.Sleep(3500); // Wait for splash to finish
            splash.Close();
            
            // Show Login Form
            LoginForm loginForm = new LoginForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // Role-based routing
                if (loginForm.CurrentUser.Role.ToLower() == "admin")
                {
                    // Admin goes to MainForm (admin panel)
                    MainForm mainForm = new MainForm(loginForm.CurrentUser);
                    Application.Run(mainForm);
                }
                else
                {
                    // User/Customer goes to UserMainForm (shopping interface)
                    UserMainForm userMainForm = new UserMainForm(loginForm.CurrentUser);
                    Application.Run(userMainForm);
                }
            }
        }
    }
}
