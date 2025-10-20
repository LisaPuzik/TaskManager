using Kanban.BL;
using Kanban.Entities;
using System;
using System.Windows.Forms;
using Task = Kanban.Entities.Task;

namespace WinForms
{
    public partial class TaskForm : Form
    {
        public string TaskTitle => titleTextBox.Text;
        public string TaskDescription => descriptionTextBox.Text;
        public DateTime TaskDeadLine => deadlinePicker.Value;
        public Priority TaskPriority => (Priority)priorityComboBox.SelectedItem;

        /// <summary>
        /// Конструктор для создания новой задачи (пустая форма).
        /// </summary>
        public TaskForm()
        {
            InitializeComponent();
            priorityComboBox.DataSource = Enum.GetValues(typeof(Priority));
        }

        /// <summary>
        /// Конструктор для редактирования существующей задачи.
        /// </summary>
        /// <param name="taskToEdit">Задача, данные которой нужно загрузить в форму.</param>
        public TaskForm(Task taskToEdit)
        {
            InitializeComponent();
            priorityComboBox.DataSource = Enum.GetValues(typeof(Priority));
            titleTextBox.Text = taskToEdit.Title;
            descriptionTextBox.Text = taskToEdit.Description;
            deadlinePicker.Value = taskToEdit.DeadLine;
            priorityComboBox.SelectedItem = taskToEdit.Priority;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(titleTextBox.Text))
            {
                MessageBox.Show("Название задачи не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void TaskForm_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}