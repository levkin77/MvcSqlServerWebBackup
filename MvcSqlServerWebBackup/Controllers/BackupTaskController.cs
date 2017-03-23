using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MvcSqlServerWebBackup.Models;

namespace MvcSqlServerWebBackup.Controllers
{
    public class BackupTaskController : Controller
    {
        /// <summary>
        /// Настройка соединений
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Message = "Настройка заданий резервного копирования.";
            var data = DbContext.Current.GetBackupTasks();

            return View(data.Select(s => new ModelBackupTaskView() { Id = s.Id, Name = s.Name, DbName = s.DbName}));
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = id.Equals(Guid.Empty.ToString()) ? new BackupTask():  DbContext.Current.GetBackupTasks().Find(s => s.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(new ModelBackupTaskViewEdit() { Id = item.Id, Name = item.Name, Memo = item.Memo,
                DbName =  item.DbName,
                ConnectionId = item.ConnectionId,
                CloudDriveId = item.CloudDriveId,
                CopyOnly = item.CopyOnly,
                Compression = item.Compression,
                UseZip = item.UseZip,
                AddCurrentDateTime = item.AddCurrentDateTime
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Memo,DbName,ConnectionId,CloudDriveId,CopyOnly,Compression,UseZip,AddCurrentDateTime")] ModelBackupTaskViewEdit item)
        {
            if (ModelState.IsValid)
            {

                var v = item.Id.Equals(Guid.Empty.ToString())? null:  DbContext.Current.GetBackupTasks().Find(s => s.Id == item.Id);
                if (v != null)
                {
                    v.Name = item.Name;
                    v.Memo = item.Memo;
                    v.DbName = item.DbName;
                    v.ConnectionId = item.ConnectionId;
                    v.CloudDriveId = item.CloudDriveId;
                    v.CopyOnly = item.CopyOnly;
                    v.Compression = item.Compression;
                    v.UseZip = item.UseZip;
                    v.AddCurrentDateTime = item.AddCurrentDateTime;
                }
                else
                {
                    v = new BackupTask();
                    v.Id = item.Id;
                    v.Name = item.Name;
                    v.Memo = item.Memo;
                    v.DbName = item.DbName;
                    v.ConnectionId = item.ConnectionId;
                    v.CloudDriveId = item.CloudDriveId;
                    v.CopyOnly = item.CopyOnly;
                    v.Compression = item.Compression;
                    v.UseZip = item.UseZip;
                    v.AddCurrentDateTime = item.AddCurrentDateTime;
                    if(item.Id.Equals(Guid.Empty.ToString()))
                        v.NewId();
                }
                DbContext.Current.Save(v);
                return RedirectToAction("Index");
            }
            return View(item);
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = DbContext.Current.GetBackupTasks().Find(s => s.Id == id);

            if (item == null)
            {
                return HttpNotFound();
            }
            return View(ModelHelper.GetDetailModel(item));
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = DbContext.Current.GetBackupTasks().Find(s => s.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(ModelHelper.GetDetailModel(item));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var item = DbContext.Current.GetBackupTasks().Find(s => s.Id == id);
            if (item != null)
            {
                DbContext.Current.Delete(item);
            }
            return RedirectToAction("Index");
        }

        
        public ActionResult ClearStatus(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = DbContext.Current.GetBackupTasks().Find(s => s.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(ModelHelper.GetDetailModel(item));
        }

        [HttpPost, ActionName("ClearStatus")]
        [ValidateAntiForgeryToken]
        public ActionResult ClearStatusConfirmed(string id)
        {
            var item = DbContext.Current.GetBackupTasks().Find(s => s.Id == id);
            if (item != null)
            {
                item.LastRun = new DateTime();
                item.LastStatus = string.Empty;
                DbContext.Current.Save(item);
            }
            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult TryValidate(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = DbContext.Current.GetBackupTasks().Find(s => s.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }

            Dictionary<string, Tuple<bool,string>> v = new Dictionary<string, Tuple<bool, string>>();
            v.Add("ValidateConnectionId", new Tuple<bool, string>(true, "Выполнено!"));
            v.Add("ValidateCloudDriveId", new Tuple<bool, string>(true, "Выполнено!"));
            v.Add("ValidateCloudDriveCanConnect", new Tuple<bool, string>(true, "Выполнено!"));
            v.Add("ValidateConnectionCanConnect", new Tuple<bool, string>(true, "Выполнено!"));


            var allModels = new Tuple<ModelBackupTaskViewDetail, Dictionary<string, Tuple<bool, string>>>
                (ModelHelper.GetDetailModel(item), v);
    
            return View(allModels);
        }

        public ActionResult TryRun(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = DbContext.Current.GetBackupTasks().Find(s => s.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            Dictionary<string, Tuple<bool, string>> v = new Dictionary<string, Tuple<bool, string>>();

            var connection = DbContext.Current.GetServerConnections().FirstOrDefault(s => s.Id == item.ConnectionId);
            if (connection == null)
            {
                v.Add("ValidateConnectionId", new Tuple<bool, string>(false, "Не выполнено!"));
            }
            else
            {
                v.Add("ValidateConnectionId", new Tuple<bool, string>(true, "Выполнено!"));

                string cnnString = ServerConnection.BuildConnectionString(connection);
                if (ServerConnection.TryConnect(cnnString))
                {
                    v.Add("ValidateConnectionCanConnect", new Tuple<bool, string>(true, "Выполнено!"));

                }
                else
                {
                    v.Add("ValidateConnectionCanConnect", new Tuple<bool, string>(false, "Не выполнено!"));
                }

                var drive = DbContext.Current.GetCloudDrives().FirstOrDefault(s => s.Id == item.CloudDriveId);
                if (drive != null)
                {
                    v.Add("ValidateCloudDriveId", new Tuple<bool, string>(true, "Выполнено!"));
                    if (drive.Provider == CloudDrive.PROVIDER_FILESYSTEM)
                    {
                        //Regex.Unescape(drive.Location)
                        // проверка доступности файловой системы
                        if (!Directory.Exists(drive.Location))
                        {
                            Directory.CreateDirectory(drive.Location);
                        }
                        v.Add("ValidateCloudDriveCanConnect", new Tuple<bool, string>(true, "Выполнено!"));

                        

                        var task = DbContext.Current.GetBackupTasks().FirstOrDefault(s => s.Id == id);
                        string dbName = task.DbName;
                        if (!dbName.StartsWith("["))
                        {
                            dbName = string.Format("[{0}]", dbName);
                        }
                        string dbNameClean = dbName.Replace("[", string.Empty).Replace("]", string.Empty);
                        DateTime dt = DateTime.Now;
                        string dbNameWithDate = dbNameClean + "_" + dt.ToString("yyyyMMddhhmmss");
                        string fileNameBak = (item.AddCurrentDateTime ? dbNameWithDate : dbNameClean) + ".bak";
                        var location = System.IO.Path.Combine(drive.Location, fileNameBak);


                        try
                        {
                            using (SqlConnection cnnConnection = new SqlConnection(cnnString))
                            {
                                using (SqlCommand cmd = cnnConnection.CreateCommand())
                                {

                                    string cmdText= string.Format(@"BACKUP DATABASE {0} TO  DISK = N'{1}' WITH #REPLACE_COPYONLY NOFORMAT, NOINIT,  NAME = N'{2}-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, #COMPRESSION STATS = 10",

                                        // Сжатие
                                        //BACKUP DATABASE[smartstore] TO  DISK = N'C:\Backup\smartstore.bak' WITH NOFORMAT, NOINIT, NAME = N'smartstore-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, COMPRESSION, STATS = 10
                                        //BACKUP DATABASE [smartstore] TO  DISK = N'C:\Backup\smartstore.bak' WITH NOFORMAT, NOINIT,  NAME = N'smartstore-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, NO_COMPRESSION,  STATS = 10

                                        // CopyOnly
                                        //BACKUP DATABASE [smartstore] TO  DISK = N'C:\Backup\smartstore.bak' WITH  COPY_ONLY, NOFORMAT, NOINIT,  NAME = N'smartstore-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10
                                        dbName, location, dbNameClean);
                                    if (item.CopyOnly)
                                    {
                                        cmdText = cmdText.Replace("#REPLACE_COPYONLY", "COPY_ONLY,");
                                    }
                                    else
                                    {
                                        cmdText = cmdText.Replace("#REPLACE_COPYONLY", "");
                                    }
                                    switch (item.Compression)
                                    {
                                        case 1:
                                            cmdText = cmdText.Replace("#COMPRESSION", "COMPRESSION,");
                                            break;
                                        case 2:
                                            cmdText = cmdText.Replace("#COMPRESSION", "NO_COMPRESSION,");
                                            break;
                                        default:
                                            cmdText = cmdText.Replace("#COMPRESSION", "");
                                            break;
                                    }
                                    cmd.CommandText = cmdText;
                                    cmd.Connection.Open();
                                    cmd.ExecuteNonQuery();
                                    cmd.Connection.Close();
                                    v.Add("CreateBackupDone", new Tuple<bool, string>(true, "Выполнено!"));
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            v.Add("CreateBackupDone", new Tuple<bool, string>(false, "Не выполнено!"));
                            v.Add("CreateBackupDoneError", new Tuple<bool, string>(false, e.Message));
                            Console.WriteLine(e);
                            
                        }
                        
                        /*
                     BACKUP DATABASE [smartstore] TO  DISK = N'E:\SQLDATABASE\BACKUP\smartstore.bak' WITH NOFORMAT, NOINIT,  NAME = N'smartstore-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10    
                     */
                    }
                }
                else
                {
                    v.Add("ValidateCloudDriveId", new Tuple<bool, string>(false, "Не выполнено!"));
                    v.Add("ValidateCloudDriveCanConnect", new Tuple<bool, string>(false, "Не выполнено!"));
                }
            }
            var allModels = new Tuple<ModelBackupTaskViewDetail, Dictionary<string, Tuple<bool, string>>>
                (ModelHelper.GetDetailModel(item), v);

            return View(allModels);
        }
        //
        //
    }
}