namespace MvcSqlServerWebBackup.Models
{
    /// <summary>
    /// Модель облачного провайдера
    /// </summary>
    public class ModelCloudDriveView:ModelBase
    {
        public string Provider { get; set; }
        public bool CanConnect { get; set; }
    }
}