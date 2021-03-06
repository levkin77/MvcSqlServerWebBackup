﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MvcSqlServerWebBackup
{
    public class AppConfig
    {
        public const string DefaultBackupLocation = @"C:\Backup";
        #region Свойства

        public AppConfig()
        {
            _backupLocation = DefaultBackupLocation;
        }
        /// <summary>
        /// Место создания резервных копий по умолчанию
        /// </summary>
        public string BackupLocation
        {
            get { return _backupLocation; }
            set { _backupLocation = value; }
        }

        /// <summary>
        /// Пароль zip архивов
        /// </summary>
        public string ZipPassword { get; set; }
        #endregion

        private static object locker = new object();
        private string _backupLocation;

        public static bool Save(string location, AppConfig values)
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
        public static AppConfig Load(string location)
        {
            AppConfig values = new AppConfig();
            try
            {
                XmlSerializer x = new XmlSerializer(typeof(List<CloudDrive>));
                StreamReader reader = new StreamReader(location);
                values = (AppConfig)x.Deserialize(reader);

            }
            catch
            {

            }
            return values;
        }
    }
}