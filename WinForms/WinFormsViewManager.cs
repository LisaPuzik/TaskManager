using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Kanban.Presenter;
using Kanban.Presenter.Interfaces;
using Kanban.Presenter.ViewModels;

namespace WinForms
{
    public class WinFormsViewManager : IViewManager
    {
        private readonly VMManager _vmManager;
        private readonly Dictionary<Type, Func<object, Form>> _viewMapping;

        public WinFormsViewManager()
        {
            _vmManager = new VMManager();
            _vmManager.ViewModelReady += OnViewModelReady;

            _viewMapping = new Dictionary<Type, Func<object, Form>>
            {
                { typeof(MainViewModel), vm => new MainForm((MainViewModel)vm) },
                { typeof(TaskEditorViewModel), vm => new TaskForm((TaskEditorViewModel)vm) }
            };
        }
        public void Run()
        {
            _vmManager.CreateMainViewModel(this);
        }

        private void OnViewModelReady(object viewModel)
        {
            var type = viewModel.GetType();
            if (_viewMapping.TryGetValue(type, out var createForm))
            {
                Form form = createForm(viewModel);

                if (form is MainForm)
                {
                    Application.Run(form);
                }
                else
                {
                    form.ShowDialog();
                }
            }
        }

        public void Close() => Application.Exit();
        public void ShowMessage(string message) => MessageBox.Show(message);

        public bool OpenTaskEditor(TaskEditorViewModel viewModel)
        {
            if (_viewMapping.TryGetValue(typeof(TaskEditorViewModel), out var createForm))
            {
                using (var form = createForm(viewModel))
                {
                    viewModel.RequestClose += () => { form.DialogResult = DialogResult.OK; form.Close(); };

                    form.ShowDialog();
                    return viewModel.IsSaved;
                }
            }
            return false;
        }
    }
}