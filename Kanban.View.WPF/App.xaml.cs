using System.Windows;

namespace Kanban.View.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var manager = new WpfViewManager();

            manager.Run();
        }
    }
}