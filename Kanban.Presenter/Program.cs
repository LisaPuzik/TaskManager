using Kanban.BL;
using Kanban.Shared;
using Ninject;
using System;
using System.Windows.Forms;

namespace Kanban.Presenter
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var kernel = new StandardKernel(new SimpleConfigModule());
            var logic = kernel.Get<ILogicAll>();

            IMainView view = null;
            bool isWinForms = true;

             Console.WriteLine("Press 'c' for Console, any other key for WinForms");
             if (Console.ReadKey().Key == ConsoleKey.C) isWinForms = false;

            if (isWinForms)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                view = new WinForms.MainForm();
            }
            else
            {
                view = new Kanban.ConsoleUI.ConsoleView();
            }

            var presenter = new MainPresenter(logic, view);

            if (isWinForms)
            {
                Application.Run((Form)view);
            }
            else
            {
                ((Kanban.ConsoleUI.ConsoleView)view).Run();
            }
        }
    }
}