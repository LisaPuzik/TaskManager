using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Model
{
    public class Logic
    {
        private List<Task> _tasks;
        /// <summary>
        /// Возвращает полный путь к файлу с данными в папке AppData.
        /// </summary>
        private string GetFilePath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string appFolder = Path.Combine(appDataPath, "MyKanbanApp");

            Directory.CreateDirectory(appFolder);

            return Path.Combine(appFolder, "tasks.json");
        }
        public Logic()
        {
            _tasks = new List<Task>();
            Load();
        }

        /// <summary>
        /// Добавляет новую задачу в список.
        /// </summary>
        /// <param name="title">Заголовок задачи.</param>
        /// <param name="description">Описание задачи.</param>
        /// <param name="deadLine">Срок выполнения задачи.</param>
        public void AddTask(string title, string description, DateTime deadLine)
        {
            var task = new Task
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                DeadLine = deadLine,
                Status = TaskStatus.ToDo
            };
            _tasks.Add(task);
            Save();
        }

        /// <summary>
        /// Обновляет существующую задачу.
        /// </summary>
        /// <param name="id">ID задачи для обновления.</param>
        /// <param name="title">Новый заголовок.</param>
        /// <param name="description">Новое описание.</param>
        /// <param name="deadLine">Новый срок выполнения.</param>
        public void UpdateTask(Guid id, string title, string description, DateTime deadLine)
        {
            var task = GetTaskById(id);
            if (task != null)
            {
                task.Title = title;
                task.Description = description;
                task.DeadLine = deadLine;
                Save();
            }
        }

        /// <summary>
        /// Удаляет задачу по ее ID.
        /// </summary>
        /// <param name="id">ID задачи для удаления.</param>
        public void DeleteTask(Guid id)
        {
            var task = GetTaskById(id);
            if (task != null)
            {
                _tasks.Remove(task);
                Save();
            }
        }

        /// <summary>
        /// Изменяет статус задачи.
        /// </summary>
        /// <param name="id">ID задачи.</param>
        /// <param name="newStatus">Новый статус задачи.</param>
        public void ChangeTaskStatus(Guid id, TaskStatus newStatus)
        {
            var task = GetTaskById(id);
            if (task != null)
            {
                task.Status = newStatus;
                Save();
            }
        }

        /// <summary>
        /// Возвращает список всех задач.
        /// </summary>
        /// <returns>Список всех задач.</returns>
        public List<Task> GetAllTasks()
        {
            return _tasks;
        }

        /// <summary>
        /// Возвращает задачу по ее ID.
        /// </summary>
        /// <param name="id">ID задачи.</param>
        /// <returns>Найденная задача или null.</returns>
        public Task GetTaskById(Guid id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Сохраняет список задач в JSON-файл.
        /// </summary>
        public void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_tasks, options);
            File.WriteAllText(GetFilePath(), json);
        }

        /// <summary>
        /// Загружает список задач из JSON-файла.
        /// </summary>
        public void Load()
        {
            string filePath = GetFilePath();
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    var loadedTasks = JsonSerializer.Deserialize<List<Task>>(json);
                    if (loadedTasks != null)
                    {
                        _tasks = loadedTasks;
                    }
                }
                catch (JsonException)
                {
                    _tasks = new List<Task>();
                }
            }
        }
    }
}
