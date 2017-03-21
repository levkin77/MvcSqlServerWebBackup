using System;

namespace MvcSqlServerWebBackup.Models
{
    /// <summary>
    /// Модель задания
    /// </summary>
    public class ModelTaskView: ModelBase
    {
        public string ServerName { get; set; }
        public string DbName { get; set; }

        public string LastStatus { get; set; }

        public DateTime LastRun { get; set; }
    }
}