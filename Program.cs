using System;
using System.Windows.Forms;
using studentManagementSyatem.Database;
using studentManagementSyatem.Forms;

namespace studentManagementSyatem
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Database and  tables initialization
            DatabaseInitializer.Initialize();

            // Start from the Login form 
            Application.Run(new LoginForm());
        }
    }
}