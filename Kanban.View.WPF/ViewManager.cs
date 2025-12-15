using System;
using System.Collections.Generic;
using System.Windows;
using Kanban.Presenter;
using Kanban.Presenter.Interfaces;
using Kanban.Presenter.ViewModels;

namespace Kanban.View.WPF
{
    public class WpfViewManager : IViewManager
    {
        private readonly VMManager _vmManager;

        private readonly Dictionary<Type, Func<Window>> _viewMapping;

        public WpfViewManager()
        {
            _vmManager = new VMManager();
            _vmManager.ViewModelReady += OnViewModelReady;
            _viewMapping = new Dictionary<Type, Func<Window>>
            {
                { typeof(MainViewModel), () => new MainWindow() },
                { typeof(TaskEditorViewModel), () => new TaskWindow() }
            };
        }

        public void Run()
        {
            _vmManager.CreateMainViewModel(this);
        }

        private void OnViewModelReady(object viewModel)
        {
                var vmType = viewModel.GetType();

                if (_viewMapping.TryGetValue(vmType, out var createView))
                {
                    Window window = createView();
                    window.DataContext = viewModel;

                    if (window is MainWindow)
                    {
                        Application.Current.MainWindow = window;
                        window.Show();
                    }
                    else
                    {
                        if (Application.Current.MainWindow != null &&
                            Application.Current.MainWindow != window)
                        {
                            window.Owner = Application.Current.MainWindow;
                        }

                        window.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show($"Не найдено представление для {vmType.Name}");
                }
        }

        public void Close()
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win.IsActive) { win.Close(); break; }
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool OpenTaskEditor(TaskEditorViewModel viewModel)
        {

            if (_viewMapping.TryGetValue(typeof(TaskEditorViewModel), out var createView))
            {
                var window = createView();
                window.DataContext = viewModel;
                viewModel.RequestClose += () => window.DialogResult = true;
                window.ShowDialog();
                return viewModel.IsSaved;
            }
            return false;
        }
    }
}