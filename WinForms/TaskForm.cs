using System.Windows.Forms;
using Kanban.Presenter.ViewModels;

namespace WinForms
{
    public partial class TaskForm : Form
    {
        private readonly TaskEditorViewModel _vm;

        public TaskForm(TaskEditorViewModel vm)
        {
            InitializeComponent();
            _vm = vm;

            titleTextBox.DataBindings.Add("Text", _vm, nameof(_vm.Title), false, DataSourceUpdateMode.OnPropertyChanged);
            descriptionTextBox.DataBindings.Add("Text", _vm, nameof(_vm.Description), false, DataSourceUpdateMode.OnPropertyChanged);
            deadlinePicker.DataBindings.Add("Value", _vm, nameof(_vm.DeadLine), false, DataSourceUpdateMode.OnPropertyChanged);

            priorityComboBox.DataSource = _vm.PrioritiesListForForms;
            priorityComboBox.DisplayMember = "Value";
            priorityComboBox.ValueMember = "Key";
            priorityComboBox.DataBindings.Add("SelectedValue", _vm, nameof(_vm.Priority), false, DataSourceUpdateMode.OnPropertyChanged);
            priorityComboBox.DataBindings.Add("SelectedItem", _vm, nameof(_vm.Priority), false, DataSourceUpdateMode.OnPropertyChanged);

            saveButton.Click += (s, e) => _vm.SaveCommand.Execute(null);
            cancelButton.Click += (s, e) => this.Close();
        }
    }
}