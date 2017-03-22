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
        
    }
}