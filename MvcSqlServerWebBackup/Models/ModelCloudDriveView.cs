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
        /// <summary>
        /// Текущее расположение
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Токен
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Расположение файла сертификата
        /// </summary>
        public string CertificateFile { get; set; }
        /// <summary>
        /// Пароль сертификата
        /// </summary>
        public string CertificatePassword { get; set; }
        /// <summary>
        /// Сервисный email 
        /// </summary>
        public string ServiceAccountEmail { get; set; }
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        public string ClientId { get; set; }
    }

}