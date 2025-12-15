using Kanban.Presenter.ViewModels;

namespace Kanban.Presenter.Interfaces
{
    public interface IViewManager
    {
        void Close();
        void ShowMessage(string message);
        bool OpenTaskEditor(TaskEditorViewModel viewModel);
    }
}