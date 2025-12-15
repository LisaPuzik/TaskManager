using System;
using System.ComponentModel;
using Kanban.BL;
using Kanban.Entities;
using Kanban.Presenter.DTO;
using Kanban.Presenter.Infrastructure;
using Kanban.Presenter.Interfaces;
using Task = Kanban.Entities.Task;
using TaskStatus = Kanban.Entities.TaskStatus;

namespace Kanban.Presenter.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        private readonly ILogicAll _logic;
        private readonly IViewManager _viewManager;

        public BindingList<TaskDto> ToDoTasks { get; set; }
        public BindingList<TaskDto> InProgressTasks { get; set; }
        public BindingList<TaskDto> DoneTasks { get; set; }

        private TaskDto _selectedTask;
        public TaskDto SelectedTask
        {
            get => _selectedTask;
            set { _selectedTask = value; OnPropertyChanged(nameof(SelectedTask)); }
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand MoveNextCommand { get; }
        public RelayCommand MovePrevCommand { get; }

        public MainViewModel(ILogicAll logic, IViewManager viewManager)
        {
            _logic = logic;
            _viewManager = viewManager;

            ToDoTasks = new BindingList<TaskDto>();
            InProgressTasks = new BindingList<TaskDto>();
            DoneTasks = new BindingList<TaskDto>();

            _logic.ModelUpdated += (s, e) => LoadData();

            AddCommand = new RelayCommand(_ => AddTask());
            UpdateCommand = new RelayCommand(_ => UpdateTask(), _ => SelectedTask != null);
            DeleteCommand = new RelayCommand(_ => DeleteTask(), _ => SelectedTask != null);

            MoveNextCommand = new RelayCommand(obj => MoveTask((TaskDto)obj, 1));
            MovePrevCommand = new RelayCommand(obj => MoveTask((TaskDto)obj, -1));

            LoadData();
        }

        private void LoadData()
        {
            var tasksEntities = _logic.GetAllTasks();
            ToDoTasks.Clear(); InProgressTasks.Clear(); DoneTasks.Clear();

            foreach (var t in tasksEntities)
            {
                var dto = TaskDto.FromEntity(t);
                switch (dto.Status)
                {
                    case TaskStatus.ToDo: ToDoTasks.Add(dto); break;
                    case TaskStatus.InProgress: InProgressTasks.Add(dto); break;
                    case TaskStatus.Done: DoneTasks.Add(dto); break;
                }
            }
        }

        private void MoveTask(TaskDto task, int direction)
        {
            if (task == null) return;
            int newStatusInt = (int)task.Status + direction;
            if (newStatusInt >= 0 && newStatusInt <= 2)
            {
                _logic.ChangeTaskStatus(task.Id, (TaskStatus)newStatusInt);
            }
        }

        private void AddTask()
        {
            var editorVm = new TaskEditorViewModel();

            if (_viewManager.OpenTaskEditor(editorVm))
            {
                var priorityAsEnum = (Priority)editorVm.Priority;
                _logic.AddTask(editorVm.Title, editorVm.Description, editorVm.DeadLine, priorityAsEnum);
            }
        }

        private void UpdateTask()
        {
            if (SelectedTask == null) return;

            var editorVm = new TaskEditorViewModel(SelectedTask);

            if (_viewManager.OpenTaskEditor(editorVm))
            {
                var priorityAsEnum = (Priority)editorVm.Priority;
                _logic.UpdateTask(SelectedTask.Id, editorVm.Title, editorVm.Description, editorVm.DeadLine, priorityAsEnum);
            }
        }

        private void DeleteTask()
        {
            if (SelectedTask == null) return;
            _logic.DeleteTask(SelectedTask.Id);
            SelectedTask = null;
        }

        public void SetTaskStatus(TaskDto task, TaskStatus newStatus)
        {
            if (task == null || task.Status == newStatus) return;

            try
            {
                _logic.ChangeTaskStatus(task.Id, newStatus);
            }
            catch (Exception ex)
            {
                _viewManager.ShowMessage(ex.Message);
            }
        }

        public void SetTaskPriority(TaskDto task, Priority newPriority)
        {
            if (task == null || task.Priority == (int)newPriority) return;
            try
            {
                _logic.ChangePriority(task.Id, newPriority);
            }
            catch (Exception ex)
            {
                _viewManager.ShowMessage(ex.Message);
            }
        }
    }
}