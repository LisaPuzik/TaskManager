using Kanban.Entities;
using Kanban.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Task = Kanban.Entities.Task;
using TaskStatus = Kanban.Entities.TaskStatus;

namespace Kanban.ConsoleUI
{
    public class ConsoleView : IMainView
    {
        private List<Task> _cachedTasks = new List<Task>();

        public event EventHandler ViewReady;
        public event EventHandler<TaskEventArgs> CreateRequest;
        public event EventHandler<TaskEventArgs> UpdateRequest;
        public event EventHandler<TaskIdEventArgs> DeleteRequest;
        public event EventHandler<TaskStatusEventArgs> ChangeStatusRequest;
        public event EventHandler<TaskPriorityEventArgs> ChangePriorityRequest;

        public void Run()
        {
            ViewReady?.Invoke(this, EventArgs.Empty);

            while (true)
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
                        case "7": return;
                        default: Console.WriteLine("Неверный ввод."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        public void SetTaskList(IEnumerable<Task> tasks)
        {
            _cachedTasks = tasks.ToList();
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine($"[INFO]: {message}");
        }

        private void ShowAllTasksInternal()
        {
            Console.Clear();
            if (!_cachedTasks.Any())
            {
                Console.WriteLine("Задач пока нет.");
                return;
            }

            var groupedTasks = _cachedTasks.GroupBy(t => t.Status);
            foreach (var group in groupedTasks.OrderBy(g => g.Key))
            {
                Console.WriteLine($"--- {group.Key} ---");
                foreach (var task in group)
                {
                    string pMark = task.Priority == Priority.High ? "[!!!]" : (task.Priority == Priority.Medium ? "[!!]" : "[!]");
                    Console.WriteLine($"{pMark} [{task.Id}] {task.Title} (до {task.DeadLine:dd.MM.yyyy})");
                    Console.WriteLine($"\t{task.Description}");
                }
                Console.WriteLine();
            }
        }

        private void ProcessAddTask()
        {
            Console.Clear();
            Console.WriteLine("--- Добавление ---");
            Console.Write("Название: "); string title = Console.ReadLine();
            Console.Write("Описание: "); string desc = Console.ReadLine();
            DateTime date = AskForDate();
            Priority prio = AskForPriority();

            CreateRequest?.Invoke(this, new TaskEventArgs { Title = title, Description = desc, DeadLine = date, Priority = prio });
            Console.WriteLine("Запрос отправлен.");
        }

        private void ProcessUpdateTask()
        {
            ShowAllTasksInternal();
            Console.Write("\nID для редактирования: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id)) return;
            var task = _cachedTasks.FirstOrDefault(t => t.Id == id);
            if (task == null) { Console.WriteLine("Не найдено."); return; }

            Console.WriteLine($"Название ({task.Title}):");
            string title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title)) title = task.Title;

            Console.WriteLine($"Описание ({task.Description}):");
            string desc = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(desc)) desc = task.Description;
            UpdateRequest?.Invoke(this, new TaskEventArgs
            {
                Id = id,
                Title = title,
                Description = desc,
                DeadLine = task.DeadLine,
                Priority = task.Priority
            });
            Console.WriteLine("Запрос обновления отправлен.");
        }

        private void ProcessDeleteTask()
        {
            ShowAllTasksInternal();
            Console.Write("\nID для удаления: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                DeleteRequest?.Invoke(this, new TaskIdEventArgs { Id = id });
                Console.WriteLine("Запрос удаления отправлен.");
            }
        }

        private void ProcessChangeStatus()
        {
            ShowAllTasksInternal();
            Console.Write("\nID для статуса: ");
            if (!Guid.TryParse(Console.ReadLine(), out Guid id)) return;

            Console.WriteLine($"{(int)TaskStatus.ToDo}-ToDo, {(int)TaskStatus.InProgress}-InProg, {(int)TaskStatus.Done}-Done");
            if (int.TryParse(Console.ReadLine(), out int s) && Enum.IsDefined(typeof(TaskStatus), s))
            {
                ChangeStatusRequest?.Invoke(this, new TaskStatusEventArgs { Id = id, NewStatus = (TaskStatus)s });
                Console.WriteLine("Статус изменен.");
            }
        }

        private void ProcessChangePriority()
        {
            Console.Clear();
            Console.WriteLine("--- Изменение приоритета задачи ---");
            ShowAllTasksInternal();
            Console.Write("\nВведите ID задачи для изменения приоритета: ");

            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Неверный формат ID.");
                return;
            }

            if (!_cachedTasks.Any(t => t.Id == id))
            {
                Console.WriteLine("Задача с таким ID не найдена.");
                return;
            }

            Console.WriteLine("Выберите новый приоритет:");
            Console.WriteLine($"{(int)Priority.Low} - Low");
            Console.WriteLine($"{(int)Priority.Medium} - Medium");
            Console.WriteLine($"{(int)Priority.High} - High");
            Console.Write("Ваш выбор: ");

            if (int.TryParse(Console.ReadLine(), out int priorityInt) && Enum.IsDefined(typeof(Priority), priorityInt))
            {
                ChangePriorityRequest?.Invoke(this, new TaskPriorityEventArgs
                {
                    Id = id,
                    NewPriority = (Priority)priorityInt
                });

                Console.WriteLine($"Запрос на смену приоритета отправлен!");
            }
            else
            {
                Console.WriteLine("Неверный выбор приоритета.");
            }
        }

        private DateTime AskForDate()
        {
            while (true)
            {
                Console.Write("Срок (дд.мм.гггг): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d)) return d;
            }
        }

        private Priority AskForPriority()
        {
            Console.WriteLine("0-Low, 1-Medium, 2-High");
            while (true)
            {
                string i = Console.ReadLine();
                if (int.TryParse(i, out int p) && Enum.IsDefined(typeof(Priority), p)) return (Priority)p;
            }
        }
    }
}