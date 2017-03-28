using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcSqlServerWebBackup.GoogleDriveUploader
{


    public class GoogleDriveFile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OriginalFilename { get; set; }
        public string ThumbnailLink { get; set; }
        public string IconLink { get; set; }
        public string WebContentLink { get; set; }
        public DateTime ? CreatedDate { get; set; }
        public DateTime ? ModifiedDate { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
    }


}
