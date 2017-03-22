namespace MvcSqlServerWebBackup.Models
{
    /// <summary>
    /// Модель облачного провайдера
    /// </summary>
    public class ModelCloudDriveView:ModelBase
    {
        /// <summary>
        /// Типы провайдеров: MEGA, Mail.ru, Yandex, GoogleDrive, SkyDrive
        /// </summary>
        public string Provider { get; set; }
        public bool CanConnect { get; set; }
    }

    /// <summary>
    /// Модель облачного провайдера
    /// </summary>
    public class ModelCloudDriveViewEdit : ModelCloudDriveView
    {
        
    }

}