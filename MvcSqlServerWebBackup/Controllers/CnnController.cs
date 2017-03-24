using System;
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
            
            var item = id.Equals(Guid.Empty.ToString()) ? new ServerConnection() : DbContext.Current.GetServerConnections().Find(s => s.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(new ModelConnectionViewEdit() { Id = item.Id,
                Name = item.Name,
                Memo = item.Memo,
                ServerName = item.ServerName,
                ConnectionString = item.ConnectionString,
                Password = item.Password,
                Uid = item.Uid,
                IntegratedSecurity = item.IntegratedSecurity
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Memo,ServerName,ConnectionString,Password,Uid,IntegratedSecurity")] ModelConnectionViewEdit item)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(movie).State = EntityState.Modified;
                //db.SaveChanges();
                var v = item.Id.Equals(Guid.Empty.ToString()) ? null : DbContext.Current.GetServerConnections().Find(s => s.Id == item.Id);
                if (v != null)
                {
                    v.Name = item.Name;
                    v.Memo = item.Memo;
                    v.ServerName = item.ServerName;
                    v.ConnectionString = item.ConnectionString;
                    v.Password = item.Password;
                    v.Uid = item.Uid;
                    v.IntegratedSecurity = item.IntegratedSecurity;
                }
                else
                {
                    v= new ServerConnection();
                    v.Id = item.Id;
                    v.Name = item.Name;
                    v.Memo = item.Memo;
                    v.ServerName = item.ServerName;
                    v.ConnectionString = item.ConnectionString;
                    v.Password = item.Password;
                    v.Uid = item.Uid;
                    v.IntegratedSecurity = item.IntegratedSecurity;
                    if (item.Id.Equals(Guid.Empty.ToString()))
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