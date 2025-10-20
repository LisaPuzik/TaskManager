namespace WinForms
{
    partial class TaskForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            titleTextBox = new TextBox();
            descriptionTextBox = new TextBox();
            label3 = new Label();
            deadlinePicker = new DateTimePicker();
            saveButton = new Button();
            cancelButton = new Button();
            label4 = new Label();
            priorityComboBox = new ComboBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(77, 20);
            label1.TabIndex = 0;
            label1.Text = "Название";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 42);
            label2.Name = "label2";
            label2.Size = new Size(79, 20);
            label2.TabIndex = 1;
            label2.Text = "Описание";
            // 
            // titleTextBox
            // 
            titleTextBox.Location = new Point(163, 6);
            titleTextBox.Name = "titleTextBox";
            titleTextBox.Size = new Size(250, 27);
            titleTextBox.TabIndex = 2;
            // 
            // descriptionTextBox
            // 
            descriptionTextBox.Location = new Point(163, 39);
            descriptionTextBox.Multiline = true;
            descriptionTextBox.Name = "descriptionTextBox";
            descriptionTextBox.Size = new Size(250, 46);
            descriptionTextBox.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 91);
            label3.Name = "label3";
            label3.Size = new Size(135, 20);
            label3.TabIndex = 4;
            label3.Text = "Срок выполнения";
            // 
            // deadlinePicker
            // 
            deadlinePicker.Location = new Point(163, 91);
            deadlinePicker.Name = "deadlinePicker";
            deadlinePicker.Size = new Size(250, 27);
            deadlinePicker.TabIndex = 5;
            // 
            // saveButton
            // 
            saveButton.Location = new Point(12, 166);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(94, 29);
            saveButton.TabIndex = 6;
            saveButton.Text = "Сохранить";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(12, 201);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(94, 29);
            cancelButton.TabIndex = 7;
            cancelButton.Text = "Отмена";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 131);
            label4.Name = "label4";
            label4.Size = new Size(85, 20);
            label4.TabIndex = 8;
            label4.Text = "Приоритет";
            label4.Click += label4_Click;
            // 
            // priorityComboBox
            // 
            priorityComboBox.FormattingEnabled = true;
            priorityComboBox.Location = new Point(163, 131);
            priorityComboBox.Name = "priorityComboBox";
            priorityComboBox.Size = new Size(151, 28);
            priorityComboBox.TabIndex = 9;
            // 
            // TaskForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(471, 322);
            Controls.Add(priorityComboBox);
            Controls.Add(label4);
            Controls.Add(cancelButton);
            Controls.Add(saveButton);
            Controls.Add(deadlinePicker);
            Controls.Add(label3);
            Controls.Add(descriptionTextBox);
            Controls.Add(titleTextBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "TaskForm";
            Text = "TaskForm";
            Load += TaskForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox titleTextBox;
        private TextBox descriptionTextBox;
        private Label label3;
        private DateTimePicker deadlinePicker;
        private Button saveButton;
        private Button cancelButton;
        private Label label4;
        private ComboBox priorityComboBox;
    }
}