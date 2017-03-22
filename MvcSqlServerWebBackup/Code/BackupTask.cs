using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MvcSqlServerWebBackup
{
    public class BackupTask : CoreObject
    {
        #region Свойства
        public string ServerName { get; set; }
        public string DbName { get; set; }

        public string LastStatus { get; set; }

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