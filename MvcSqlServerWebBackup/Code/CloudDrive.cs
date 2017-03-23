using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MvcSqlServerWebBackup
{
    public class CloudDrive : CoreObject
    {
        public const string PROVIDER_GOOGLEDRIVE = "GoogleDrive";
        public const string PROVIDER_MEGA = "Mega";
        public const string PROVIDER_FILESYSTEM = "FileSystem";
        #region Свойства
        /// <summary>
        /// Типы провайдеров: MEGA, Mail.ru, Yandex, GoogleDrive, SkyDrive
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// Текущее расположение
        /// </summary>
        public string Location { get; set; }
        public string Uid { get; set; }
        public string Password { get; set; }
        public bool CanConnect { get; set; } 
        #endregion

        public static CloudDrive New()
        {
            CloudDrive v = new CloudDrive();
            v.NewId();
            return v;
        }

        private static object locker = new object();
        public static bool SaveCollection(string location, List<CloudDrive> values)
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
        public static List<CloudDrive> LoadCollection(string location)
        {
            List<CloudDrive> values = new List<CloudDrive>();
            try
            {
                XmlSerializer x = new XmlSerializer(typeof(List<CloudDrive>));
                StreamReader reader = new StreamReader(location);
                values = (List<CloudDrive>)x.Deserialize(reader);

            }
            catch
            {

            }
            return values;
        }
    }
}