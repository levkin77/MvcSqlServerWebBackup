using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MvcSqlServerWebBackup
{
    public class BackupTask : CoreObject
    {
        #region Свойства
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
        /// Идентификатор хранилища
        /// </summary>
        public string CloudDriveId { get; set; }
        /// <summary>
        /// Использовать множественные локации для хранения резервной копии
        /// </summary>
        public string UseMultyLocation { get; set; }
        /// <summary>
        /// Имя сервера баз данных
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// Идентификатор строки соединения
        /// </summary>
        public string ConnectionId { get; set; }
        /// <summary>
        /// База данных для резервного копирования
        /// </summary>
        public string DbName { get; set; }
        /// <summary>
        /// Последнее сообщение о статусе резервного копирования
        /// </summary>
        public string LastStatus { get; set; }

        /// <summary>
        /// Дата последнего запуска
        /// </summary>
        public DateTime LastRun { get; set; }
        #endregion

        public static BackupTask New()
        {
            BackupTask v = new BackupTask();
            v.NewId();
            return v;
        }

        private static object locker = new object();
        public static bool SaveCollection(string location, List<BackupTask> values)
        {
            try
            {
                lock (locker)
                {
                    XmlSerializer x = new XmlSerializer(values.GetType());
                    using (StreamWriter writer = new StreamWriter(location))
                    {
                        x.Serialize(writer, values);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Загрузка данных из файла
        /// </summary>
        /// <param name="location">Расположение файла</param>
        /// <returns></returns>
        public static List<BackupTask> LoadCollection(string location)
        {
            List<BackupTask> values = new List<BackupTask>();
            try
            {
                XmlSerializer x = new XmlSerializer(typeof(List<BackupTask>));
                StreamReader reader = new StreamReader(location);
                values = (List<BackupTask>)x.Deserialize(reader);

            }
            catch
            {

            }
            return values;
        }
    }
}