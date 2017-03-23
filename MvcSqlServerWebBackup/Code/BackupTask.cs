using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MvcSqlServerWebBackup
{
    public class BackupTask : CoreObject
    {
        #region ��������
        /// <summary>
        /// ��������� ������� ����� � ���� � ����� �����
        /// </summary>
        public bool AddCurrentDateTime { get; set; }
        /// <summary>
        /// ������������ ������ � zip �����
        /// </summary>
        public bool UseZip { get; set; }
        /// <summary>
        /// ������ ��������� ����� �� ������ �������
        /// </summary>
        public int Compression { get; set; }
        /// <summary>
        /// ����� CopyOnly ��� ��������� �����
        /// </summary>
        public bool CopyOnly { get; set; }
        /// <summary>
        /// ������������� ���������
        /// </summary>
        public string CloudDriveId { get; set; }
        /// <summary>
        /// ������������ ������������� ������� ��� �������� ��������� �����
        /// </summary>
        public string UseMultyLocation { get; set; }
        /// <summary>
        /// ��� ������� ��� ������
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// ������������� ������ ����������
        /// </summary>
        public string ConnectionId { get; set; }
        /// <summary>
        /// ���� ������ ��� ���������� �����������
        /// </summary>
        public string DbName { get; set; }
        /// <summary>
        /// ��������� ��������� � ������� ���������� �����������
        /// </summary>
        public string LastStatus { get; set; }

        /// <summary>
        /// ���� ���������� �������
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
        /// �������� ������ �� �����
        /// </summary>
        /// <param name="location">������������ �����</param>
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