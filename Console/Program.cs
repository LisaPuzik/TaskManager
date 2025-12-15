using System;
using System.Linq;
using System.Globalization;
using Kanban.Presenter.DTO;
using Kanban.Presenter.ViewModels;
using TaskStatus = Kanban.Entities.TaskStatus;
using Priority = Kanban.Entities.Priority;

namespace Kanban.ConsoleUI
{
    public class ConsoleApp
    {
        private readonly MainViewModel _vm;
        private bool _running = true;

        public ConsoleApp(MainViewModel vm)
        {
            _vm = vm;
        }

        public void Stop() => _running = false;

        public void Run()
        {
            while (_running)
            {
                Console.Clear();
                Console.WriteLine("--- Канбан-доска ---");
                Console.WriteLine("1. Показать все задачи");
                Console.WriteLine("2. Добавить задачу");
                Console.WriteLine("3. Редактировать задачу");
                Console.WriteLine("4. Удалить задачу");
                Console.WriteLine("5. Изменить статус задачи");
                Console.WriteLine("6. Изменить приоритетность задачи");
                Console.WriteLine("7. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": ShowAllTasksInternal(); break;
                        case "2": ProcessAddTask(); break;
                        case "3": ProcessUpdateTask(); break;
                        case "4": ProcessDeleteTask(); break;
                        case "5": ProcessChangeStatus(); break;
                        case "6": ProcessChangePriority(); break;
                        case "7": _running = false; return;
                        default: Console.WriteLine("Неверный ввод."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                if (_running)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }
        }

        private void ShowAllTasksInternal()
        {
            Console.Clear();
            PrintGroup(TaskStatus.ToDo, _vm.ToDoTasks);
            PrintGroup(TaskStatus.InProgress, _vm.InProgressTasks);
            PrintGroup(TaskStatus.Done, _vm.DoneTasks);
        }

        private void PrintGroup(TaskStatus status, System.ComponentModel.BindingList<TaskDto> list)
        {
            Console.WriteLine($"--- {status} ---");
            foreach (var task in list)
            {
                string pMark = task.Priority == (int)Priority.High ? "[!!!]" : (task.Priority == (int)Priority.Medium ? "[!!]" : "[!]");
                Console.WriteLine($"{pMark} [{task.Id}] {task.Title} (до {task.DeadLine:dd.MM.yyyy})");
                Console.WriteLine($"\t{task.Description}");
            }
            Console.WriteLine();
        }

        private void ProcessAddTask()
        {
            if (_vm.AddCommand.CanExecute(null))
            {
                _vm.AddCommand.Execute(null);
                Console.WriteLine("Запрос отправлен.");
            }
        }

        private void ProcessUpdateTask()
        {
            ShowAllTasksInternal();
            Console.Write("\nID для редактирования: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id)) return;
            var task = FindTask(id);
            if (task == null) { Console.WriteLine("Не найдено."); return; }
            _vm.SelectedTask = task;
            if (_vm.UpdateCommand.CanExecute(null))
            {
                _vm.UpdateCommand.Execute(null);
                Console.WriteLine("Запрос обновления отправлен.");
            }
        }

        private void ProcessDeleteTask()
        {
            ShowAllTasksInternal();
            Console.Write("\nID для удаления: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                var task = FindTask(id);
                if (task != null)
                {
                    _vm.SelectedTask = task;
                    _vm.DeleteCommand.Execute(null);
                    Console.WriteLine("Запрос удаления отправлен.");
                }
                else Console.WriteLine("Не найдено.");
            }
        }

        private void ProcessChangeStatus()
        {
            ShowAllTasksInternal();
            Console.Write("\nID для статуса: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id)) return;
            var task = FindTask(id);
            if (task == null) { Console.WriteLine("Не найдено."); return; }
            Console.WriteLine($"{(int)TaskStatus.ToDo}-ToDo, {(int)TaskStatus.InProgress}-InProg, {(int)TaskStatus.Done}-Done");
            if (int.TryParse(Console.ReadLine(), out int s) && Enum.IsDefined(typeof(TaskStatus), s))
            {
                _vm.SetTaskStatus(task, (TaskStatus)s);
                Console.WriteLine("Статус изменен.");
            }
        }

        private void ProcessChangePriority()
        {
            Console.Clear();
            Console.WriteLine("--- Изменение приоритета задачи ---");
            ShowAllTasksInternal();
            Console.Write("\nВведите ID задачи для изменения приоритета: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id)) { Console.WriteLine("Неверный формат ID."); return; }
            var task = FindTask(id);
            if (task == null) { Console.WriteLine("Задача с таким ID не найдена."); return; }
            Console.WriteLine("Выберите новый приоритет:");
            Console.WriteLine($"0 - Low");
            Console.WriteLine($"1 - Medium");
            Console.WriteLine($"2 - High");
            Console.Write("Ваш выбор: ");
            if (int.TryParse(Console.ReadLine(), out int priorityInt) && Enum.IsDefined(typeof(Priority), priorityInt))
            {
                _vm.SetTaskPriority(task, (Priority)priorityInt);
                Console.WriteLine($"Запрос на смену приоритета отправлен!");
            }
            else Console.WriteLine("Неверный выбор приоритета.");
        }

        public bool RunTaskEditor(TaskEditorViewModel editorVm)
        {
            Console.Clear();
            Console.WriteLine("--- Ввод данных задачи ---");
            string titlePrompt = string.IsNullOrEmpty(editorVm.Title) ? "Название: " : $"Название ({editorVm.Title}): ";
            Console.Write(titlePrompt);
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title)) editorVm.Title = title;
            string descPrompt = string.IsNullOrEmpty(editorVm.Description) ? "Описание: " : $"Описание ({editorVm.Description}): ";
            Console.Write(descPrompt);
            string desc = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(desc)) editorVm.Description = desc;

            editorVm.DeadLine = AskForDate(editorVm.DeadLine);
            editorVm.Priority = AskForPriority(editorVm.Priority);

            editorVm.SaveCommand.Execute(null);
            return editorVm.IsSaved;
        }

        private DateTime AskForDate(DateTime defaultDate)
        {
            Console.Write($"Срок ({defaultDate:dd.MM.yyyy}) [Enter - оставить]: ");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return defaultDate;
            if (DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d)) return d;
            return AskForDate(defaultDate);
        }

        private int AskForPriority(int defaultPriority)
        {
            Console.WriteLine("0-Low, 1-Medium, 2-High");
            Console.Write($"Приоритет ({defaultPriority}): ");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return defaultPriority;

            if (int.TryParse(input, out int p) && (p >= 0 && p <= 2))
            {
                return p;
            }

            return AskForPriority(defaultPriority);
        }

        private TaskDto FindTask(Guid id)
        {
            var t = _vm.ToDoTasks.FirstOrDefault(x => x.Id == id);
            if (t != null) return t;
            t = _vm.InProgressTasks.FirstOrDefault(x => x.Id == id);
            if (t != null) return t;
            return _vm.DoneTasks.FirstOrDefault(x => x.Id == id);
        }
    }
}