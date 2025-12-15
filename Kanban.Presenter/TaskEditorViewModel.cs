using System;
using System.Collections.Generic;
using Kanban.Presenter.DTO;
using Kanban.Presenter.Infrastructure;

namespace Kanban.Presenter.ViewModels
{
    public class TaskEditorViewModel : NotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine { get; set; }
        public int Priority { get; set; }
        public Dictionary<int, string> PrioritiesList { get; } = new Dictionary<int, string>
        {
            { 0, "Low" },
            { 1, "Medium" },
            { 2, "High" }
        };
        public object PrioritiesListForForms => PrioritiesList.ToList();
        public RelayCommand SaveCommand { get; }

        public event Action RequestClose;
        public bool IsSaved { get; private set; } = false;

        public TaskEditorViewModel(TaskDto task = null)
        {
            if (task != null)
            {
                Title = task.Title;
                Description = task.Description;
                DeadLine = task.DeadLine;
                Priority = task.Priority;
            }
            else
            {
                Title = "";
                Description = "";
                DeadLine = DateTime.Now.Date;
                Priority = 0;
            }

            SaveCommand = new RelayCommand(_ => Save());
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Title)) return;
            IsSaved = true;
            RequestClose?.Invoke();
        }
    }
}