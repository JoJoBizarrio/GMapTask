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

            Form1 form1 = new Form1();
            Markers markers = new Markers();
            Presenter presenter = new Presenter(form1, markers);
            Application.Run(form1);
        }
    }
}