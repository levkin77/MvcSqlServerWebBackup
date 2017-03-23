using System;
using System.Collections.Generic;

namespace MvcSqlServerWebBackup.Models
{
    public static class ModelHelper
    {
        
        static ModelHelper()
        {
            
        } 
        public static List<ModelConnectionView> GetConnectionView()
        {
            List<ModelConnectionView> returnList = new List<ModelConnectionView>();
            foreach (ServerConnection connection in DbContext.Current.GetServerConnections())
            {
                ModelConnectionView v = new ModelConnectionView();
                v.Id = connection.Id;
                v.Name = connection.Name;
                v.Memo = connection.Memo;
                v.ServerName = connection.ServerName;
                returnList.Add(v);
            }
            return returnList;
        }
        public static List<ModelCloudDriveView> GetCloudDriveView()
        {
            List<ModelCloudDriveView> returnList = new List<ModelCloudDriveView>();
            foreach (CloudDrive connection in DbContext.Current.GetCloudDrives())
            {
                ModelCloudDriveView v = new ModelCloudDriveView();
                v.Id = connection.Id;
                v.Name = connection.Name;
                v.Memo = connection.Memo;
                v.Provider = connection.Provider;
                returnList.Add(v);
            }
            return returnList;
        }

        public static ModelBackupTaskViewDetail GetDetailModel(BackupTask value)
        {
            ModelBackupTaskViewDetail model = new ModelBackupTaskViewDetail();
            model.Id = value.Id;
            model.Name = value.Name;
            model.Memo = value.Memo;
            model.DbName = value.DbName;
            model.LastRun = value.LastRun;
            model.LastStatus = value.LastStatus;
            model.CloudDriveId = value.CloudDriveId;
            model.CopyOnly = value.CopyOnly;
            model.Compression = value.Compression;
            model.UseZip = value.UseZip;
            model.AddCurrentDateTime = value.AddCurrentDateTime;
            var itemDrive = DbContext.Current.GetCloudDrives().Find(s => s.Id == value.CloudDriveId);
            if (itemDrive != null)
            {
                model.CloudDriveInfo = string.Format("Провайдер:{0}{1}Наименование:{2}{1}Описание:{3}", itemDrive.Provider, @"<br/>", itemDrive.Name, itemDrive.Memo);
            }
            else
            {
                model.CloudDriveInfo = string.Empty;
            }
            
            model.ConnectionId = value.ConnectionId;
            var itemCnn = DbContext.Current.GetServerConnections().Find(s => s.Id == value.ConnectionId);
            if (itemCnn != null)
            {
                model.ConnectionInfo = string.Format("Сервер:{0}{1}База данных по умолчанию:{2}{1}Описание:{3}", itemCnn.ServerName, @"<br/>", itemCnn.Name, itemCnn.Memo);
            }
            else
            {
                model.ConnectionInfo = string.Empty;
            }
            return model;
        }

        public static string GetBackupTastValidationCaption(string key)
        {
            var keys = new Dictionary<string, string>();
            keys.Add("ValidateConnectionId", "Проверка идентификатора соединения с базой данных");
            keys.Add("ValidateCloudDriveId", "Проверка идентификатора соединения с хранилищем");
            keys.Add("ValidateCloudDriveCanConnect", "Проверка соединения с хранилищем");
            keys.Add("ValidateConnectionCanConnect", "Проверка соединения с сервером баз данных");
            if (keys.ContainsKey(key))
                return keys[key];
            else
            {
                return key;
            }
        }
    }
}