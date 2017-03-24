using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;

namespace MvcSqlServerWebBackup
{
    public class ZipData
    {
        public static void CreateZip(string source, string destination, string zippwd=null, bool deleteSource=true)
        {
            //Crc32 crc = new Crc32();
            ZipOutputStream s = new ZipOutputStream(File.Create(destination));

            s.SetLevel(9); // 0 - store only to 9 - means best compression
            if(!string.IsNullOrEmpty(zippwd))
                s.Password = zippwd;

            FileInfo fi = new FileInfo(source);
            //FileStream fs = File.OpenRead(source);

            //byte[] buffer = new byte[fs.Length];
            //fs.Read(buffer, 0, buffer.Length);
            ZipEntry entry = new ZipEntry(System.IO.Path.GetFileName(source));

            entry.DateTime = fi.LastWriteTime; //DateTime.Now;
            entry.Size = fi.Length;

            s.PutNextEntry(entry);
            byte[] buffer = new byte[4096];
            using (FileStream streamReader = File.OpenRead(source))
            {
                StreamUtils.Copy(streamReader, s, buffer);
            }
            s.CloseEntry();
            s.Finish();
            s.Close();

            if (deleteSource)
            {
                if (System.IO.File.Exists(source))
                {
                    try
                    {
                        System.IO.File.Delete(source);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}