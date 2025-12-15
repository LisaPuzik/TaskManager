using System;
using Kanban.Entities;
using Kanban.Presenter.Infrastructure;
using Task = Kanban.Entities.Task;
using TaskStatus = Kanban.Entities.TaskStatus;

namespace Kanban.Presenter.DTO
{
    public class TaskDto : NotifyPropertyChanged
    {
        private Guid _id;
        private string _title;
        private string _description;
        private DateTime _deadLine;
        private int _priorityLevel;
        private TaskStatus _status;

        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public DateTime DeadLine
        {
            get => _deadLine;
            set
            {
                _deadLine = value;
                OnPropertyChanged(nameof(DeadLine));
            }
        }


        public TaskStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public int Priority
        {
            get => _priorityLevel;
            set { _priorityLevel = value; OnPropertyChanged(nameof(Priority)); }
        }

        public static TaskDto FromEntity(Task task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DeadLine = task.DeadLine,
                Status = task.Status,
                Priority = (int)task.Priority
            };
        }
    }
}