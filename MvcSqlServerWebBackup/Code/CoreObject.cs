using System;

namespace MvcSqlServerWebBackup
{
    public abstract class CoreObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        public void NewId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}