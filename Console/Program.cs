using Kanban.BL;
using Kanban.Entities;
using Ninject;
using System;
using System.Globalization;
using System.Linq;
using Ninject;
using Task = Kanban.Entities.Task;
using TaskStatus = Kanban.Entities.TaskStatus;

namespace Kanban.ConsoleUI
{
    class Program
    {
        private static Logic logic;

        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new SimpleConfigModule());
            logic = kernel.Get<Logic>();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Канбан-доска ---");
                Console.WriteLine("1. Показать все задачи");
                Console.WriteLine("2. Добавить задачу");
                Console.WriteLine("3. Редактировать задачу");
                Console.WriteLine("4. Удалить задачу");
                Console.WriteLine("5. Изменить статус задачи");
                Console.WriteLine("6. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllTasks();
                        break;
                    case "2":
                        AddTask();
                        break;
                    case "3":
                        UpdateTask();
                        break;
                    case "4":
                        DeleteTask();
                        break;
                    case "5":
                        ChangeTaskStatus();
                        break;
                    case "6":
                        Console.WriteLine("До свидания!");
                        return;
                    default:
                        Console.WriteLine("Неверный ввод. Пожалуйста, выберите пункт меню.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Отображает все задачи, сгруппированные по статусу.
        /// </summary>
        private static void ShowAllTasks()
        {
            Console.Clear();
            var allTasks = logic.GetAllTasks();

            if (!allTasks.Any())
            {
                Console.WriteLine("Задач пока нет. Вы можете добавить новую.");
                return;
            }

            var groupedTasks = allTasks.GroupBy(t => t.Status);

            foreach (var group in groupedTasks.OrderBy(g => g.Key))
            {
                switch (group.Key)
                {
                    case TaskStatus.ToDo:
                        Console.WriteLine("--- К ВЫПОЛНЕНИЮ ---");
                        break;
                    case TaskStatus.InProgress:
                        Console.WriteLine("--- В ПРОЦЕССЕ ---");
                        break;
                    case TaskStatus.Done:
                        Console.WriteLine("--- ГОТОВО ---");
                        break;
                }

                foreach (var task in group)
                {
                    string priorityMarker = task.Priority == Priority.High ? "[!!!]" : (task.Priority == Priority.Medium ? "[!!]" : "[!]");
                    Console.WriteLine($"{priorityMarker} [{task.Id}] {task.Title} (до {task.DeadLine:dd.MM.yyyy})");
                    Console.WriteLine($"\tОписание: {task.Description}");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Запрашивает у пользователя данные для новой задачи и добавляет ее.
        /// </summary>
        private static void AddTask()
        {
            Console.Clear();
            Console.WriteLine("--- Добавление новой задачи ---");

            Console.Write("Введите название: ");
            string title = Console.ReadLine();

            Console.Write("Введите описание: ");
            string description = Console.ReadLine();

            DateTime deadLine;
            while (true)
            {
                Console.Write("Введите срок выполнения (в формате дд.мм.гггг): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out deadLine))
                {
                    break;
                }
                Console.WriteLine("Неверный формат даты. Попробуйте еще раз.");
            }
            Priority priority = AskForPriority();
            logic.AddTask(title, description, deadLine, priority);
            Console.WriteLine("Задача успешно добавлена!");
        }

        /// <summary>
        /// Запрашивает ID задачи и новые данные для ее обновления.
        /// </summary>
        private static void UpdateTask()
        {
            Console.Clear();
            Console.WriteLine("--- Редактирование задачи ---");
            ShowAllTasks();
            Console.Write("\nВведите ID задачи, которую хотите изменить: ");

            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Неверный формат ID.");
                return;
            }

            var task = logic.GetTaskById(id);
            if (task == null)
            {
                Console.WriteLine("Задача с таким ID не найдена.");
                return;
            }

            Console.WriteLine($"Текущее название: '{task.Title}'. Введите новое или нажмите Enter, чтобы оставить без изменений:");
            string newTitle = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newTitle)) newTitle = task.Title;

            Console.WriteLine($"Текущее описание: '{task.Description}'. Введите новое или нажмите Enter:");
            string newDescription = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newDescription)) newDescription = task.Description;

            DateTime newDeadLine;
            while (true)
            {
                Console.WriteLine($"Текущий срок: {task.DeadLine:dd.MM.yyyy}. Введите новый (дд.мм.гггг) или нажмите Enter:");
                string dateInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(dateInput))
                {
                    newDeadLine = task.DeadLine;
                    break;
                }
                if (DateTime.TryParseExact(dateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out newDeadLine))
                {
                    break;
                }
                Console.WriteLine("Неверный формат даты.");
            }
            Priority newPriority = AskForPriority(task.Priority);
            logic.UpdateTask(id, newTitle, newDescription, newDeadLine, newPriority);
            Console.WriteLine("Задача успешно обновлена!");
        }

        /// <summary>
        /// Запрашивает ID задачи для удаления.
        /// </summary>
        private static void DeleteTask()
        {
            Console.Clear();
            Console.WriteLine("--- Удаление задачи ---");
            ShowAllTasks();
            Console.Write("\nВведите ID задачи, которую хотите удалить: ");

            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Неверный формат ID.");
                return;
            }

            if (logic.GetTaskById(id) == null)
            {
                Console.WriteLine("Задача с таким ID не найдена.");
                return;
            }

            logic.DeleteTask(id);
            Console.WriteLine("Задача успешно удалена!");
        }

        /// <summary>
        /// Запрашивает ID задачи и новый статус для нее.
        /// </summary>
        private static void ChangeTaskStatus()
        {
            Console.Clear();
            Console.WriteLine("--- Изменение статуса задачи ---");
            ShowAllTasks();
            Console.Write("\nВведите ID задачи для изменения статуса: ");

            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                Console.WriteLine("Неверный формат ID.");
                return;
            }

            if (logic.GetTaskById(id) == null)
            {
                Console.WriteLine("Задача с таким ID не найдена.");
                return;
            }

            Console.WriteLine("Выберите новый статус:");
            Console.WriteLine($"{(int)TaskStatus.ToDo} - {TaskStatus.ToDo}");
            Console.WriteLine($"{(int)TaskStatus.InProgress} - {TaskStatus.InProgress}");
            Console.WriteLine($"{(int)TaskStatus.Done} - {TaskStatus.Done}");

            if (!int.TryParse(Console.ReadLine(), out int statusInt) || !Enum.IsDefined(typeof(TaskStatus), statusInt))
            {
                Console.WriteLine("Неверный выбор статуса.");
                return;
            }

            logic.ChangeTaskStatus(id, (TaskStatus)statusInt);
            Console.WriteLine("Статус задачи успешно изменен!");
        }

        /// <summary>
        /// Запрашивает у пользователя выбор приоритета.
        /// </summary>
        private static Priority AskForPriority(Priority? current = null)
        {
            Console.WriteLine($"Выберите приоритет (текущий: {current?.ToString() ?? "не задан"}):");
            Console.WriteLine($"{(int)Priority.Low} - {Priority.Low}");
            Console.WriteLine($"{(int)Priority.Medium} - {Priority.Medium}");
            Console.WriteLine($"{(int)Priority.High} - {Priority.High}");

            while (true)
            {
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input) && current.HasValue)
                {
                    return current.Value;
                }
                if (int.TryParse(input, out int priorityInt) && Enum.IsDefined(typeof(Priority), priorityInt))
                {
                    return (Priority)priorityInt;
                }
                Console.WriteLine("Неверный ввод. Попробуйте еще раз.");
            }
        }
    }
}