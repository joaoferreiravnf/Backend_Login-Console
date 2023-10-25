using ProjetoCLS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoWIN
{
    internal static class Program
    {
        //Variáveis globais que acompanham toda as janelas do meu projeto
        public static string appId = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
        public static AppConfig appInfo = AppConfig.AppInfo();
        public static AppUser user = new AppUser();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Default());
        }
    }
}
