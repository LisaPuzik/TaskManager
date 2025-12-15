using System;
using Ninject;
using Kanban.BL;
using Kanban.Presenter.ViewModels;
using Kanban.Presenter.Interfaces;

namespace Kanban.Presenter
{
    public class VMManager
    {
        private readonly ILogicAll _logic;

        public event Action<object> ViewModelReady;

        public VMManager()
        {
            var kernel = new StandardKernel();
            kernel.Load(new SimpleConfigModule());
            _logic = kernel.Get<ILogicAll>();
        }

        public void CreateMainViewModel(IViewManager viewManager)
        {
            var vm = new MainViewModel(_logic, viewManager);
            OnViewModelReady(vm);
        }

        public void CreateTaskEditorViewModel(DTO.TaskDto taskToEdit = null)
        {
            var vm = new TaskEditorViewModel(taskToEdit);
            OnViewModelReady(vm);
        }

        private void OnViewModelReady(object viewModel)
        {
            ViewModelReady?.Invoke(viewModel);
        }
    }
}