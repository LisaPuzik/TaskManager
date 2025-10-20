using Kanban.BL;
using Kanban.Entities;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Task = Kanban.Entities.Task;
using TaskStatus = Kanban.Entities.TaskStatus;

namespace WinForms
{
    public partial class MainForm : Form
    {
        private readonly Logic logic = new Logic(RepositoryType.Dapper);

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Вызывается при загрузке формы.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshBoard();
        }

        /// <summary>
        /// Полностью обновляет все списки на доске, загружая данные из 'logic'.
        /// </summary>
        private void RefreshBoard()
        {
            toDoListBox.Items.Clear();
            inProgressListBox.Items.Clear();
            doneListBox.Items.Clear();

            var allTasks = logic.GetAllTasks();

            foreach (var task in allTasks)
            {
                switch (task.Status)
                {
                    case TaskStatus.ToDo:
                        toDoListBox.Items.Add(new TaskDisplayItem(task));
                        break;
                    case TaskStatus.InProgress:
                        inProgressListBox.Items.Add(new TaskDisplayItem(task));
                        break;
                    case TaskStatus.Done:
                        doneListBox.Items.Add(new TaskDisplayItem(task));
                        break;
                }
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Добавить".
        /// </summary>
        private void addButton_Click(object sender, EventArgs e)
        {
            using (var taskForm = new TaskForm())
            {
                if (taskForm.ShowDialog() == DialogResult.OK)
                {
                    logic.AddTask(taskForm.TaskTitle, taskForm.TaskDescription, taskForm.TaskDeadLine, taskForm.TaskPriority);
                    RefreshBoard();
                }
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Редактировать".
        /// </summary>
        private void editButton_Click(object sender, EventArgs e)
        {
            var selectedItem = toDoListBox.SelectedItem ?? inProgressListBox.SelectedItem ?? doneListBox.SelectedItem;

            if (selectedItem is TaskDisplayItem selectedTaskItem)
            {
                var taskToEdit = logic.GetTaskById(selectedTaskItem.Id);
                if (taskToEdit != null)
                {
                    using (var taskForm = new TaskForm(taskToEdit))
                    {
                        if (taskForm.ShowDialog() == DialogResult.OK)
                        {
                            logic.UpdateTask(taskToEdit.Id, taskForm.TaskTitle, taskForm.TaskDescription, taskForm.TaskDeadLine, taskForm.TaskPriority);
                            RefreshBoard();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите задачу для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Удалить".
        /// </summary>
        private void deleteButton_Click(object sender, EventArgs e)
        {
            var selectedItem = toDoListBox.SelectedItem ?? inProgressListBox.SelectedItem ?? doneListBox.SelectedItem;

            if (selectedItem is TaskDisplayItem selectedTaskItem)
            {
                var confirmation = MessageBox.Show($"Вы уверены, что хотите удалить задачу '{selectedTaskItem.Title}'?",
                                                   "Подтверждение удаления",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Question);

                if (confirmation == DialogResult.Yes)
                {
                    logic.DeleteTask(selectedTaskItem.Id);
                    RefreshBoard();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите задачу для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        /// Вспомогательный класс для отображения задач в ListBox.
        /// </summary>
        private class TaskDisplayItem
        {
            public Guid Id { get; }
            public string Title { get; }

            public TaskDisplayItem(Task task)
            {
                Id = task.Id;
                Title = task.Title;
            }

            public override string ToString()
            {
                return Title;
            }
        }

        /// <summary>
        /// Срабатывает при нажатии кнопки мыши.Выбирает элемент под курсором и "запоминает" его для возможного перетаскивания.
        /// </summary>
        private void listBox_MouseDown(object sender, MouseEventArgs e)
        {
            ListBox sourceListBox = sender as ListBox;
            int index = sourceListBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                sourceListBox.SelectedItem = sourceListBox.Items[index];
            }
        }

        /// <summary>
        /// Срабатывает, когда перетаскиваемый объект входит в границы ListBox.
        /// </summary>
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

        /// <summary>
        /// Срабатывает, когда пользователь отпускает кнопку мыши над списком, тобишь помещает задачу в ListBox.
        /// </summary>
        private void listBox_DragDrop(object sender, DragEventArgs e)
        {
            TaskDisplayItem draggedItem = (TaskDisplayItem)e.Data.GetData(typeof(TaskDisplayItem));

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

            logic.ChangeTaskStatus(draggedItem.Id, newStatus);
            RefreshBoard();
        }

        private void toDoListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Срабатывает при движении мыши над списком.
        /// Если левая кнопка зажата и есть выбранный элемент, НАЧИНАЕТ перетаскивание.
        /// </summary>
        private void listBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && (sender as ListBox).SelectedItem != null)
            {
                ListBox sourceListBox = sender as ListBox;
                sourceListBox.DoDragDrop(sourceListBox.SelectedItem, DragDropEffects.Move);
            }
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ListBox listBox = sender as ListBox;
            TaskDisplayItem item = listBox.Items[e.Index] as TaskDisplayItem;
            Task task = logic.GetTaskById(item.Id);

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
    }
}