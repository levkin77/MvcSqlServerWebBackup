using System;

namespace MvcSqlServerWebBackup.Models
{
    /// <summary>
    /// Модель задания
    /// </summary>
    public class ModelBackupTaskView : ModelBase
    {
        public string ServerName { get; set; }
        public string DbName { get; set; }

        public string LastStatus { get; set; }

        public DateTime LastRun { get; set; }
    }

    public class ModelBackupTaskViewEdit : ModelBackupTaskView
    {
        /// <summary>
        /// Добавлять текущее время и дату к имени файла
        /// </summary>
        public bool AddCurrentDateTime { get; set; }
        /// <summary>
        /// Использовать сжатие в zip архив
        /// </summary>
        public bool UseZip { get; set; }
        /// <summary>
        /// Сжатие резервной копии на уровне сервера
        /// </summary>
        public int Compression { get; set; }
        /// <summary>
        /// Опция CopyOnly для резервной копии
        /// </summary>
        public bool CopyOnly { get; set; }
        /// <summary>
        /// Идентификатор строки соединения
        /// </summary>
        public string ConnectionId { get; set; }
        /// <summary>
        /// Идентификатор хранилища
        /// </summary>
        public string CloudDriveId { get; set; }
    }

    public class ModelBackupTaskViewDetail : ModelBackupTaskView
    {
        /// <summary>
        /// Добавлять текущее время и дату к имени файла
        /// </summary>
        public bool AddCurrentDateTime { get; set; }
        /// <summary>
        /// Использовать сжатие в zip архив
        /// </summary>
        public bool UseZip { get; set; }
        /// <summary>
        /// Сжатие резервной копии на уровне сервера
        /// </summary>
        public int Compression { get; set; }
        /// <summary>
        /// Опция CopyOnly для резервной копии
        /// </summary>
        public bool CopyOnly { get; set; }
        /// <summary>
        /// Идентификатор строки соединения
        /// </summary>
        public string ConnectionId { get; set; }
        /// <summary>
        /// Идентификатор строки соединения
        /// </summary>
        public string CloudDriveId { get; set; }
        /// <summary>
        /// Информация о хранилище
        /// </summary>
        public string CloudDriveInfo { get; set; }
        /// <summary>
        /// Информация строки соединения
        /// </summary>
        public string ConnectionInfo { get; set; }
    }
}