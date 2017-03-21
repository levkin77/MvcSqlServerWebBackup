using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcSqlServerWebBackup.Models
{
    /// <summary>
    /// Модель соединения с базами данных для представления в списке
    /// </summary>
    public class ModelConnectionView: ModelBase
    {
        /// <summary>
        /// Сервер баз данных
        /// </summary>
        public string ServerName { get; set; }
        
    }

    /// <summary>
    /// Модель соединения с базами данных для редактирования
    /// </summary>
    public class ModelConnectionViewEdit: ModelConnectionView
    {
        /// <summary>
        /// Использовать данную строку соединения, вместо указанных данных
        /// </summary>
        public bool UseAdvancedConnection { get; set; }
        public bool IntegratedSecurity { get; set; }
        public string Password { get; set; }
        public string Uid { get; set; }
        public string ConnectionString { get; set; }
    }
}