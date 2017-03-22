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
        /// <summary>
        /// Текущий контекст базы данных
        /// </summary>
        public static DbContext Current
        {
            get { return _currentDbContext; }
        }

        #region ServerConnection
        private List<ServerConnection> _collServerConnections;
        public void Delete(ServerConnection value)
        {
            if (_collServerConnections != null)
            {
                var item = _collServerConnections.Find(s => s.Id == value.Id);
                if (item != null)
                {
                    _collServerConnections.Remove(item);
                    SaveServerConnections();
                }
            }
        }

        public void Save(ServerConnection value)
        {
            if (_collServerConnections != null)
            {
                var item = _collServerConnections.Find(s => s.Id == value.Id);
                if (item == null)
                {
                    _collServerConnections.Add(item);
                    SaveServerConnections();
                }
                else
                {
                    item = value;
                    SaveServerConnections();
                }
            }
        }

        
        public List<ServerConnection> GetServerConnections()
        {
            if (_collServerConnections == null)
                _collServerConnections = new List<ServerConnection>();
            if (_collServerConnections.Count == 0)
            {
                List<ServerConnection> coll = new List<ServerConnection>();
                for (int i = 0; i < 10; i++)
                {
                    ServerConnection v = ServerConnection.New();
                    v.CanConnect = (i % 2) > 0;
                    v.ServerName = "ServerName_000" + i;
                    v.Name = "Name_0000000___" + i;
                    _collServerConnections.Add(v);
                }
            }
            return _collServerConnections;
        }
        #endregion

        #region CloudDrive
        private List<CloudDrive> _collCloudDrives;
        public void Delete(CloudDrive value)
        {
            if (_collCloudDrives != null)
            {
                var item = _collCloudDrives.Find(s => s.Id == value.Id);
                if (item != null)
                {
                    _collCloudDrives.Remove(item);
                    SaveCloudDrives();
                }
            }
        }

        public void Save(CloudDrive value)
        {
            if (_collCloudDrives != null)
            {
                var item = _collCloudDrives.Find(s => s.Id == value.Id);
                if (item == null)
                {
                    _collCloudDrives.Add(item);
                    SaveCloudDrives();
                }
                else
                {
                    item = value;
                    SaveCloudDrives();
                }
            }
        }

        

        public List<CloudDrive> GetCloudDrives()
        {
            if (_collCloudDrives == null)
                _collCloudDrives = new List<CloudDrive>();
            if (_collCloudDrives.Count == 0)
            {
                List<CloudDrive> coll = new List<CloudDrive>();
                for (int i = 0; i < 10; i++)
                {
                    CloudDrive v = CloudDrive.New();
                    v.CanConnect = (i % 2) > 0;
                    v.Provider = "Provider" + i;
                    v.Name = "Name_0000000___" + i;
                    _collCloudDrives.Add(v);
                }
            }
            return _collCloudDrives;
        }
        #endregion

        #region BackupTask
        private List<BackupTask> _collBackupTasks;
        public void Delete(BackupTask value)
        {
            if (_collBackupTasks != null)
            {
                var item = _collBackupTasks.Find(s => s.Id == value.Id);
                if (item != null)
                {
                    _collBackupTasks.Remove(item);
                    SaveBackupTasks();
                }
            }
        }

        public void Save(BackupTask value)
        {
            if (_collBackupTasks != null)
            {
                var item = _collBackupTasks.Find(s => s.Id == value.Id);
                if (item == null)
                {
                    _collBackupTasks.Add(item);
                    SaveBackupTasks();
                }
                else
                {
                    item = value;
                    SaveBackupTasks();
                }
            }
        }

        

        public List<BackupTask> GetBackupTasks()
        {
            if (_collBackupTasks == null)
                _collBackupTasks = new List<BackupTask>();
            if (_collBackupTasks.Count == 0)
            {
                List<BackupTask> coll = new List<BackupTask>();
                for (int i = 0; i < 10; i++)
                {
                    BackupTask v = BackupTask.New();
                    v.Name = "BackupTaskName_0000000___" + i;
                    _collBackupTasks.Add(v);
                }
            }
            return _collBackupTasks;
        }
        #endregion

        public void LoadAllData()
        {
            var sPath = System.Web.Hosting.HostingEnvironment.MapPath("/App_Data/ServerConnection.xml");
            if (System.IO.File.Exists(sPath))
            {
                var data = ServerConnection.LoadCollection(sPath);
                _collServerConnections = data;
            }
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("/App_Data/CloudDrives.xml");
            if (System.IO.File.Exists(sPath))
            {
                var data = CloudDrive.LoadCollection(sPath);
                _collCloudDrives = data;
            }
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("/App_Data/BackupTasks.xml");
            if (System.IO.File.Exists(sPath))
            {
                var data = BackupTask.LoadCollection(sPath);
                _collBackupTasks = data;
            }
        }

        public void SaveServerConnections()
        {
            var sPath = System.Web.Hosting.HostingEnvironment.MapPath("/App_Data/ServerConnection.xml");
            ServerConnection.SaveCollection(sPath, _collServerConnections);
        }
        public void SaveCloudDrives()
        {
            var sPath = System.Web.Hosting.HostingEnvironment.MapPath("/App_Data/CloudDrives.xml");
            CloudDrive.SaveCollection(sPath, _collCloudDrives);
        }
        public void SaveBackupTasks()
        {
            var sPath = System.Web.Hosting.HostingEnvironment.MapPath("/App_Data/BackupTasks.xml");
            BackupTask.SaveCollection(sPath, _collBackupTasks);
        }
    }
}