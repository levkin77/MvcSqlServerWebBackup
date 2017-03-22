using System.Linq;
using System.Net;
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
            var item = DbContext.Current.GetBackupTasks().Find(s => s.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(new ModelBackupTaskViewEdit() { Id = item.Id, Name = item.Name, Memo = item.Memo});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Memo, Provider")] ModelBackupTaskViewEdit item)
        {
            if (ModelState.IsValid)
            {

                var v = DbContext.Current.GetBackupTasks().Find(s => s.Id == item.Id);
                if (v != null)
                {
                    v.Name = item.Name;
                    v.Memo = item.Memo;
                }
                else
                {
                    v = new BackupTask();
                    v.Id = item.Id;
                    v.Name = item.Name;
                    v.Memo = item.Memo;
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
            return View(new ModelBackupTaskViewEdit() { Id = item.Id, Name = item.Name, Memo = item.Memo, DbName = item.DbName});
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
            return View(new ModelBackupTaskView() { Id = item.Id, Memo = item.Memo, Name = item.Name, DbName = item.DbName });
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
    }
}