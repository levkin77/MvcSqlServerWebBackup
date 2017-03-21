using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSqlServerWebBackup.Models;

namespace MvcSqlServerWebBackup
{
    public class DbContext
    {
        private static DbContext _currentDbContext;

        static DbContext()
        {
            if(_currentDbContext==null)
                _currentDbContext = new DbContext();
        }
        public DbContext()
        {
            
        }

        public static DbContext Current
        {
            get { return _currentDbContext; }
        }

        public void Delete(ServerConnection value)
        {
            if (collConnectionViews != null)
            {
                var item = collConnectionViews.Find(s => s.Id == value.Id);
                if (item != null)
                {
                    collConnectionViews.Remove(item);
                }
            }
        }
        private List<ServerConnection> collConnectionViews;
        
        public List<ServerConnection> GetServerConnections()
        {
            if (collConnectionViews == null)
                collConnectionViews = new List<ServerConnection>();
            if (collConnectionViews.Count == 0)
            {
                List<ServerConnection> coll = new List<ServerConnection>();
                for (int i = 0; i < 10; i++)
                {
                    ServerConnection v = ServerConnection.New();
                    v.CanConnect = (i % 2) > 0;
                    v.ServerName = "ServerName_000" + i;
                    v.Name = "Name_0000000___" + i;
                    collConnectionViews.Add(v);
                }
            }
            return collConnectionViews;
        }
    }

    public abstract class CoreObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        public void NewId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class ServerConnection: CoreObject
    {
        /// <summary>
        /// Сервер баз данных
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// Текущее соединение доступно ли 
        /// </summary>
        public bool CanConnect { get; set; }
        /// <summary>
        /// Используется ли внешнее соединений
        /// </summary>
        public bool IsExternal { get; set; }

        public bool UseAdvancedConnection { get; set; }
        public bool IntegratedSecurity { get; set; }
        public string Password { get; set; }
        public string Uid { get; set; }
        public string ConnectionString { get; set; }

        public static ServerConnection New()
        {
            ServerConnection v = new ServerConnection();
            v.NewId();
            return v;
        }
    }
}