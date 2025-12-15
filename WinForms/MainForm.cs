using System;
using System.Drawing;
using System.Windows.Forms;
using Kanban.Entities;
using Kanban.Presenter.DTO;
using Kanban.Presenter.ViewModels;
using TaskStatus = Kanban.Entities.TaskStatus;

namespace WinForms
{
    public partial class MainForm : Form
    {
        private readonly MainViewModel _viewModel;

        public MainForm(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;

            BindListBox(toDoListBox, _viewModel.ToDoTasks);
            BindListBox(inProgressListBox, _viewModel.InProgressTasks);
            BindListBox(doneListBox, _viewModel.DoneTasks);

            addButton.Click += (s, e) => _viewModel.AddCommand.Execute(null);
            editButton.Click += (s, e) => _viewModel.UpdateCommand.Execute(null);
            deleteButton.Click += (s, e) => _viewModel.DeleteCommand.Execute(null);

            toDoListBox.SelectedIndexChanged += OnSelectionChanged;
            inProgressListBox.SelectedIndexChanged += OnSelectionChanged;
            doneListBox.SelectedIndexChanged += OnSelectionChanged;

            SetupContextMenu();

            SetupDragAndDrop(toDoListBox);
            SetupDragAndDrop(inProgressListBox);
            SetupDragAndDrop(doneListBox);
        }

        private void BindListBox(ListBox listBox, object dataSource)
        {
            listBox.DataSource = dataSource;
            listBox.DisplayMember = "Title";
        }

        private void SetupDragAndDrop(ListBox listBox)
        {
            listBox.AllowDrop = true;

            listBox.MouseDown += ListBox_MouseDown;
            listBox.DragEnter += ListBox_DragEnter;
            listBox.DragDrop += ListBox_DragDrop;
        }

        private void ListBox_MouseDown(object sender, MouseEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null) return;

            int index = listBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                listBox.SelectedIndex = index;
                var item = listBox.Items[index];

                if (item != null)
                {
                    listBox.DoDragDrop(item, DragDropEffects.Move);
                }
            }
        }

        private void ListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TaskDto)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void ListBox_DragDrop(object sender, DragEventArgs e)
        {
            var targetListBox = sender as ListBox;
            var task = (TaskDto)e.Data.GetData(typeof(TaskDto));

            if (targetListBox != null && task != null)
            {
                TaskStatus newStatus;

                if (targetListBox == toDoListBox) newStatus = TaskStatus.ToDo;
                else if (targetListBox == inProgressListBox) newStatus = TaskStatus.InProgress;
                else newStatus = TaskStatus.Done;
                _viewModel.SetTaskStatus(task, newStatus);
            }
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            var lb = sender as ListBox;
            if (lb?.SelectedItem is TaskDto task)
            {
                if (lb != toDoListBox) toDoListBox.ClearSelected();
                if (lb != inProgressListBox) inProgressListBox.ClearSelected();
                if (lb != doneListBox) doneListBox.ClearSelected();
                _viewModel.SelectedTask = task;
            }
        }

        private void SetupContextMenu()
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add("Move Next ->", null, (s, e) => _viewModel.MoveNextCommand.Execute(_viewModel.SelectedTask));
            menu.Items.Add("<- Move Prev", null, (s, e) => _viewModel.MovePrevCommand.Execute(_viewModel.SelectedTask));

            toDoListBox.ContextMenuStrip = menu;
            inProgressListBox.ContextMenuStrip = menu;
            doneListBox.ContextMenuStrip = menu;
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            var lb = sender as ListBox;
            if (!(lb.Items[e.Index] is TaskDto task)) return;

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color bg = Color.White;
            if (task.Priority == (int)Priority.High) bg = Color.LightCoral;
            else if (task.Priority == (int)Priority.Medium) bg = Color.LightYellow;

            e.Graphics.FillRectangle(new SolidBrush(isSelected ? SystemColors.Highlight : bg), e.Bounds);
            Brush text = isSelected ? Brushes.White : Brushes.Black;
            e.Graphics.DrawString(task.Title, e.Font, text, e.Bounds);
            e.DrawFocusRectangle();
        }
        private void listBox_MouseMove(object sender, MouseEventArgs e) { }
    }
}