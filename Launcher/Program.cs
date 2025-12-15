using System;
using Kanban.View.WPF;
using WinForms;
using Kanban.ConsoleUI;

namespace Kanban.Launcher
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Select UI:");
            Console.WriteLine("1. WPF");
            Console.WriteLine("2. WinForms");
            Console.WriteLine("3. Console");
            var k = Console.ReadKey(true).Key;

            if (k == ConsoleKey.D1)
            {
                var app = new System.Windows.Application();
                var mgr = new WpfViewManager();
                mgr.Run();
                app.Run();
            }
            else if (k == ConsoleKey.D2)
            {
                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

                var mgr = new WinFormsViewManager();
                mgr.Run();
            }
            else if (k == ConsoleKey.D3)
            {
                var mgr = new ConsoleViewManager();
                mgr.Run();
            }
        }
    }
}