using Kanban.BL;
using Kanban.Shared;
using System;

namespace Kanban.Presenter
{
    public class MainPresenter
    {
        private readonly ILogicAll _model; 
        private readonly IMainView _view;

        public MainPresenter(ILogicAll model, IMainView view)
        {
            _model = model;
            _view = view;

            _view.ViewReady += (s, e) => RefreshView();
            _view.CreateRequest += OnCreateRequest;
            _view.UpdateRequest += OnUpdateRequest;
            _view.DeleteRequest += OnDeleteRequest;
            _view.ChangeStatusRequest += OnChangeStatusRequest;
            _view.ChangePriorityRequest += OnChangePriorityRequest;

            _model.ModelUpdated += (s, e) => RefreshView();
        }

        private void RefreshView()
        {
            var tasks = _model.GetAllTasks();
            _view.SetTaskList(tasks);
        }


        private void OnCreateRequest(object sender, TaskEventArgs e)
        {
            try {
                _model.AddTask(e.Title, e.Description, e.DeadLine, e.Priority);
            } catch (Exception ex) { _view.ShowMessage(ex.Message); }
        }

        private void OnUpdateRequest(object sender, TaskEventArgs e)
        {
            if (e.Id.HasValue)
                _model.UpdateTask(e.Id.Value, e.Title, e.Description, e.DeadLine, e.Priority);
        }

        private void OnDeleteRequest(object sender, TaskIdEventArgs e)
        {
            _model.DeleteTask(e.Id);
        }

        private void OnChangeStatusRequest(object sender, TaskStatusEventArgs e)
        {
            _model.ChangeTaskStatus(e.Id, e.NewStatus);
        }
        private void OnChangePriorityRequest(object sender, TaskPriorityEventArgs e)
        {
            _model.ChangePriority(e.Id, e.NewPriority);
        }
    }
}