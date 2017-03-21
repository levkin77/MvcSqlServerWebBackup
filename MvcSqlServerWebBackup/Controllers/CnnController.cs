using System.Linq;
using System.Net;
using System.Web.Mvc;
using MvcSqlServerWebBackup.Models;

namespace MvcSqlServerWebBackup.Controllers
{
    public class CnnController: Controller
    {
        /// <summary>
        /// Настройка соединений
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Message = "Настройка соединений.";
            var data = DbContext.Current.GetServerConnections();
            
            return View(data.Select(s => new ModelConnectionView() { Id = s.Id, Name = s.Name, ServerName = s.ServerName }));
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = DbContext.Current.GetServerConnections().Find(s => s.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(new ModelConnectionViewEdit() { Id = item.Id, Memo = item.Memo, ServerName = item.ServerName, Name = item.Name });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ServerName,Name,Memo")] ModelConnectionViewEdit item)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(movie).State = EntityState.Modified;
                //db.SaveChanges();
                var v = DbContext.Current.GetServerConnections().Find(s => s.Id == item.Id);
                if (v != null)
                {
                    v.ServerName = item.ServerName;
                    v.Name = item.Name;
                    v.Memo = item.Memo;
                }
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
            var item = DbContext.Current.GetServerConnections().Find(s => s.Id == id);
            
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(new ModelConnectionViewEdit(){Id = item.Id, Memo = item.Memo, ServerName = item.ServerName, Name = item.Name});
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = DbContext.Current.GetServerConnections().Find(s => s.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            //ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            //ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            return View(new ModelConnectionView() { Id = item.Id, Memo = item.Memo, ServerName = item.ServerName, Name = item.Name });
         }
        // POST: /Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var item = DbContext.Current.GetServerConnections().Find(s => s.Id == id);
            if (item != null)
            {
                DbContext.Current.Delete(item);
            }
            return RedirectToAction("Index");
        }
    }
}