using Kanban.Entities;
using Kanban.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Task = Kanban.Entities.Task;
using TaskStatus = Kanban.Entities.TaskStatus;

namespace WinForms
{
    public partial class MainForm : Form, IMainView
    {
        public event EventHandler ViewReady;
        public event EventHandler<TaskEventArgs> CreateRequest;
        public event EventHandler<TaskEventArgs> UpdateRequest;
        public event EventHandler<TaskIdEventArgs> DeleteRequest;
        public event EventHandler<TaskStatusEventArgs> ChangeStatusRequest;
        public event EventHandler<TaskPriorityEventArgs> ChangePriorityRequest;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ViewReady?.Invoke(this, EventArgs.Empty);
        }

        public void SetTaskList(IEnumerable<Task> tasks)
        {
            toDoListBox.Items.Clear();
            inProgressListBox.Items.Clear();
            doneListBox.Items.Clear();

            foreach (var task in tasks)
            {
                var item = new TaskDisplayItem(task);

                switch (task.Status)
                {
                    case TaskStatus.ToDo:
                        toDoListBox.Items.Add(item);
                        break;
                    case TaskStatus.InProgress:
                        inProgressListBox.Items.Add(item);
                        break;
                    case TaskStatus.Done:
                        doneListBox.Items.Add(item);
                        break;
                }
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            using (var taskForm = new TaskForm())
            {
                if (taskForm.ShowDialog() == DialogResult.OK)
                {
                    CreateRequest?.Invoke(this, new TaskEventArgs
                    {
                        Title = taskForm.TaskTitle,
                        Description = taskForm.TaskDescription,
                        DeadLine = taskForm.TaskDeadLine,
                        Priority = taskForm.TaskPriority
                    });
                }
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            var selectedItem = GetSelectedItem();

            if (selectedItem != null)
            {
                var taskToEdit = selectedItem.TaskObject;

                using (var taskForm = new TaskForm(taskToEdit))
                {
                    if (taskForm.ShowDialog() == DialogResult.OK)
                    {
                        UpdateRequest?.Invoke(this, new TaskEventArgs
                        {
                            Id = taskToEdit.Id,
                            Title = taskForm.TaskTitle,
                            Description = taskForm.TaskDescription,
                            DeadLine = taskForm.TaskDeadLine,
                            Priority = taskForm.TaskPriority
                        });
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите задачу для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var selectedItem = GetSelectedItem();

            if (selectedItem != null)
            {
                var confirmation = MessageBox.Show($"Вы уверены, что хотите удалить задачу '{selectedItem.TaskObject.Title}'?",
                                                   "Подтверждение удаления",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Question);

                if (confirmation == DialogResult.Yes)
                {
                    DeleteRequest?.Invoke(this, new TaskIdEventArgs { Id = selectedItem.TaskObject.Id });
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите задачу для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private TaskDisplayItem GetSelectedItem()
        {
            return (toDoListBox.SelectedItem ?? inProgressListBox.SelectedItem ?? doneListBox.SelectedItem) as TaskDisplayItem;
        }

        private void listBox_MouseDown(object sender, MouseEventArgs e)
        {
            ListBox sourceListBox = sender as ListBox;
            int index = sourceListBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                sourceListBox.SelectedItem = sourceListBox.Items[index];
            }
        }

        private void listBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && (sender as ListBox).SelectedItem != null)
            {
                ListBox sourceListBox = sender as ListBox;
                sourceListBox.DoDragDrop(sourceListBox.SelectedItem, DragDropEffects.Move);
            }
        }

        private void listBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TaskDisplayItem)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listBox_DragDrop(object sender, DragEventArgs e)
        {
            TaskDisplayItem draggedItem = (TaskDisplayItem)e.Data.GetData(typeof(TaskDisplayItem));
            if (draggedItem == null) return;

            ListBox targetListBox = sender as ListBox;
            TaskStatus newStatus;

            if (targetListBox == toDoListBox)
            {
                newStatus = TaskStatus.ToDo;
            }
            else if (targetListBox == inProgressListBox)
            {
                newStatus = TaskStatus.InProgress;
            }
            else
            {
                newStatus = TaskStatus.Done;
            }

            if (draggedItem.TaskObject.Status != newStatus)
            {
                ChangeStatusRequest?.Invoke(this, new TaskStatusEventArgs
                {
                    Id = draggedItem.TaskObject.Id,
                    NewStatus = newStatus
                });
            }
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ListBox listBox = sender as ListBox;
            TaskDisplayItem item = listBox.Items[e.Index] as TaskDisplayItem;

            Task task = item.TaskObject;

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            Color backgroundColor = Color.White;
            if (task.Priority == Priority.High)
            {
                backgroundColor = Color.LightCoral;
            }
            else if (task.Priority == Priority.Medium)
            {
                backgroundColor = Color.LightYellow;
            }

            Color finalBackgroundColor = isSelected ? SystemColors.Highlight : backgroundColor;

            e.Graphics.FillRectangle(new SolidBrush(finalBackgroundColor), e.Bounds);

            Brush textColor = isSelected ? Brushes.White : Brushes.Black;

            if (task.DeadLine.Date < DateTime.Now.Date && task.Status != TaskStatus.Done)
            {
                textColor = Brushes.DarkRed;
            }

            e.Graphics.DrawString(
                item.Title,
                e.Font,
                textColor,
                e.Bounds,
                StringFormat.GenericDefault
            );
            e.DrawFocusRectangle();
        }

        private void toDoListBox_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }

        private class TaskDisplayItem
        {
            public Task TaskObject { get; }
            public string Title => TaskObject.Title;

            public TaskDisplayItem(Task task)
            {
                TaskObject = task;
            }

            public override string ToString()
            {
                return Title;
            }
        }
    }
}