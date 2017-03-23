using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace MvcSqlServerWebBackup
{
    
    public class ServerConnection: CoreObject
    {   
        #region Свойства
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
        #endregion

        public static ServerConnection New()
        {
            ServerConnection v = new ServerConnection();
            v.NewId();
            return v;
        }

        private static object locker = new object();
        public static bool SaveCollection(string location, List<ServerConnection> values)
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
            catch(Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Загрузка данных из файла
        /// </summary>
        /// <param name="location">Расположение файла</param>
        /// <returns></returns>
        public static List<ServerConnection> LoadCollection(string location)
        {
            List<ServerConnection> values = new List<ServerConnection>();
            try
            {
                XmlSerializer x = new XmlSerializer(typeof(List<ServerConnection>));
                StreamReader reader = new StreamReader(location);
                values = (List<ServerConnection>) x.Deserialize(reader);
                
            }
            catch
            {

            }
            return values;
        }

        public static string BuildConnectionString(ServerConnection value)
        {
            if (value == null)
                return null;
            
            if (value.UseAdvancedConnection)
            {
                return value.ConnectionString;
            }
            else
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = value.ServerName;
                builder.InitialCatalog = value.Name;
                builder.IntegratedSecurity = value.IntegratedSecurity;
                if (!value.IntegratedSecurity)
                {
                    builder.Password = value.Password?? string.Empty;
                    builder.UserID = value.Uid?? string.Empty;
                }
                
                return builder.ConnectionString;
            }
        }

        public static bool TryConnect(string value)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(value))
                {
                    cnn.Open();
                    cnn.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }
    }
}