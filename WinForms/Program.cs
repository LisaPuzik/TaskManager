using Ninject;
using Kanban.BL;
using System;
using System.Windows.Forms;

namespace WinForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            IKernel kernel = new StandardKernel(new SimpleConfigModule());
            Logic logic = kernel.Get<Logic>();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainForm(logic));
        }
    }
}