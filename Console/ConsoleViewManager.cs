using System;
using System.Collections.Generic;
using Kanban.Presenter;
using Kanban.Presenter.Interfaces;
using Kanban.Presenter.ViewModels;

namespace Kanban.ConsoleUI
{
    public class ConsoleViewManager : IViewManager
    {
        private readonly VMManager _vmManager;
        private readonly Dictionary<Type, Action<object>> _viewMapping;
        private ConsoleApp _app;

        public ConsoleViewManager()
        {
            _vmManager = new VMManager();
            _vmManager.ViewModelReady += OnViewModelReady;

            _viewMapping = new Dictionary<Type, Action<object>>
            {
                { typeof(MainViewModel), vm => { _app = new ConsoleApp((MainViewModel)vm); _app.Run(); } }
            };
        }

        public void Run() => _vmManager.CreateMainViewModel(this);

        private void OnViewModelReady(object viewModel)
        {
            if (_viewMapping.TryGetValue(viewModel.GetType(), out var action))
                action(viewModel);
        }

        public void Close() => _app?.Stop();
        public void ShowMessage(string message)
        {
            Console.WriteLine($"\n[INFO]: {message}");
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        public bool OpenTaskEditor(TaskEditorViewModel vm)
        {
            return _app.RunTaskEditor(vm);
        }
    }
}