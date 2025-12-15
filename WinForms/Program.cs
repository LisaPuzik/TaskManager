using Ninject;
using System;
using System.Windows.Forms;
using Kanban.BL;
using Kanban.Presenter.ViewModels;
using Kanban.Presenter.Interfaces;

namespace WinForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var kernel = new StandardKernel();
            kernel.Load(new SimpleConfigModule());
            var logic = kernel.Get<ILogicAll>();
            var viewManager = new WinFormsViewManager();
            var mainViewModel = new MainViewModel(logic, viewManager);
            Application.Run(new MainForm(mainViewModel));
        }
    }
}