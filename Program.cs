using System;
using System.Windows.Forms;

namespace GMapTask
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GMap gMap = new GMap();
            Model model = new Model();
            _ = new Presenter(gMap, model);
            Application.Run(gMap);
        }
    }
}