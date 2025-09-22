namespace WinForms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            editButton = new Button();
            addButton = new Button();
            deleteButton = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            tableLayoutPanel1 = new TableLayoutPanel();
            toDoListBox = new ListBox();
            inProgressListBox = new ListBox();
            doneListBox = new ListBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            flowLayoutPanel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // editButton
            // 
            editButton.Location = new Point(169, 3);
            editButton.Name = "editButton";
            editButton.Size = new Size(163, 57);
            editButton.TabIndex = 1;
            editButton.Text = "Редактировать";
            editButton.UseVisualStyleBackColor = true;
            editButton.Click += editButton_Click;
            // 
            // addButton
            // 
            addButton.Location = new Point(3, 3);
            addButton.Name = "addButton";
            addButton.Size = new Size(160, 57);
            addButton.TabIndex = 0;
            addButton.Text = "Добавить";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += addButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.Location = new Point(338, 3);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(172, 57);
            deleteButton.TabIndex = 2;
            deleteButton.Text = "Удалить";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(addButton);
            flowLayoutPanel1.Controls.Add(editButton);
            flowLayoutPanel1.Controls.Add(deleteButton);
            flowLayoutPanel1.Dock = DockStyle.Top;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(831, 62);
            flowLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel1.Controls.Add(toDoListBox, 0, 1);
            tableLayoutPanel1.Controls.Add(inProgressListBox, 1, 1);
            tableLayoutPanel1.Controls.Add(doneListBox, 2, 1);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(label2, 1, 0);
            tableLayoutPanel1.Controls.Add(label3, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 62);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(831, 448);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // toDoListBox
            // 
            toDoListBox.AllowDrop = true;
            toDoListBox.Dock = DockStyle.Fill;
            toDoListBox.FormattingEnabled = true;
            toDoListBox.Location = new Point(3, 44);
            toDoListBox.Name = "toDoListBox";
            toDoListBox.Size = new Size(270, 401);
            toDoListBox.TabIndex = 0;
            toDoListBox.SelectedIndexChanged += toDoListBox_SelectedIndexChanged;
            toDoListBox.DragDrop += listBox_DragDrop;
            toDoListBox.DragEnter += listBox_DragEnter;
            toDoListBox.MouseDown += listBox_MouseDown;
            // 
            // inProgressListBox
            // 
            inProgressListBox.AllowDrop = true;
            inProgressListBox.Dock = DockStyle.Fill;
            inProgressListBox.FormattingEnabled = true;
            inProgressListBox.Location = new Point(279, 44);
            inProgressListBox.Name = "inProgressListBox";
            inProgressListBox.Size = new Size(271, 401);
            inProgressListBox.TabIndex = 1;
            inProgressListBox.DragDrop += listBox_DragDrop;
            inProgressListBox.DragEnter += listBox_DragEnter;
            inProgressListBox.MouseDown += listBox_MouseDown;
            // 
            // doneListBox
            // 
            doneListBox.AllowDrop = true;
            doneListBox.Dock = DockStyle.Fill;
            doneListBox.FormattingEnabled = true;
            doneListBox.Location = new Point(556, 44);
            doneListBox.Name = "doneListBox";
            doneListBox.Size = new Size(272, 401);
            doneListBox.TabIndex = 2;
            doneListBox.DragDrop += listBox_DragDrop;
            doneListBox.DragEnter += listBox_DragEnter;
            doneListBox.MouseDown += listBox_MouseDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(270, 41);
            label1.TabIndex = 3;
            label1.Text = "Сделать";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(279, 0);
            label2.Name = "label2";
            label2.Size = new Size(271, 41);
            label2.TabIndex = 4;
            label2.Text = "В процессе";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(556, 0);
            label3.Name = "label3";
            label3.Size = new Size(272, 41);
            label3.TabIndex = 5;
            label3.Text = "Выполнено";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(831, 510);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(flowLayoutPanel1);
            Name = "MainForm";
            Text = "Form1";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            flowLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button editButton;
        private Button addButton;
        private Button deleteButton;
        private FlowLayoutPanel flowLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel1;
        private ListBox toDoListBox;
        private ListBox inProgressListBox;
        private ListBox doneListBox;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}
