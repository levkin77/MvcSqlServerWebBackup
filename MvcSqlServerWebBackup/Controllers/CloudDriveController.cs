using System.Linq;
using System.Net;
using System.Web.Mvc;
using MvcSqlServerWebBackup.Models;
using System;

namespace MvcSqlServerWebBackup.Controllers
{
    public class CloudDriveController : Controller
    {
        /// <summary>
        /// Настройка соединений
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Message = "Настройка облачных провайдеров.";
            var data = DbContext.Current.GetCloudDrives();

            return View(data.Select(s => new ModelCloudDriveView() { Id = s.Id, Name = s.Name, Provider = s.Provider}));
        }
        /// <summary>
        /// Редактирование данные
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = id.Equals(Guid.Empty.ToString()) ? new CloudDrive() : DbContext.Current.GetCloudDrives().Find(s => s.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(new ModelCloudDriveViewEdit() { Id = item.Id,
                Name = item.Name,
                Memo = item.Memo,
                Provider = item.Provider, 
                Location = item.Location,
                Uid = item.Uid,
                Password = item.Password,
                Token = item.Token,
                CertificateFile = item.CertificateFile,
                CertificatePassword = item.CertificatePassword,
                ServiceAccountEmail = item.ServiceAccountEmail,
                ClientId = item.ClientId
            });
        }
        /// <summary>
        /// Создание нового хранилища по указанному провайдеру
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new ModelCloudDriveViewEdit {Id = Guid.Empty.ToString()};
            model.Provider = id;

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Memo, Provider,Location, Uid, Password, Token, CertificateFile, CertificatePassword,ServiceAccountEmail, ClientId")] ModelCloudDriveViewEdit item)
        {
            if (ModelState.IsValid)
            {

                var v = item.Id.Equals(Guid.Empty.ToString()) ? null : DbContext.Current.GetCloudDrives().Find(s => s.Id == item.Id);
                if (v != null)
                {
                    v.Name = item.Name;
                    v.Memo = item.Memo;
                    v.Provider = item.Provider;
                    v.Location = item.Location;
                    v.Uid = item.Uid;
                    v.Password = item.Password;
                    v.Token = item.Token;
                    v.CertificateFile = item.CertificateFile;
                    v.CertificatePassword = item.CertificatePassword;
                    v.ServiceAccountEmail = item.ServiceAccountEmail;
                    v.ClientId = item.ClientId;
                }
                else
                {
                    v = new CloudDrive();
                    v.Id = item.Id;
                    v.Name = item.Name;
                    v.Memo = item.Memo;
                    v.Provider = item.Provider;
                    v.Location = item.Location;
                    v.Uid = item.Uid;
                    v.Password = item.Password;
                    v.Token = item.Token;
                    v.CertificateFile = item.CertificateFile;
                    v.CertificatePassword = item.CertificatePassword;
                    v.ServiceAccountEmail = item.ServiceAccountEmail;
                    v.ClientId = item.ClientId;
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
            var item = DbContext.Current.GetCloudDrives().Find(s => s.Id == id);

            if (item == null)
            {
                return HttpNotFound();
            }
            return View(new ModelCloudDriveViewEdit() { Id = item.Id,
                Name = item.Name,
                Memo = item.Memo,
                Provider = item.Provider,
                Location = item.Location,
                Uid = item.Uid,
                Password = item.Password,
                Token = item.Token,
                CertificateFile = item.CertificateFile,
                CertificatePassword = item.CertificatePassword,
                ServiceAccountEmail = item.ServiceAccountEmail,
                ClientId = item.ClientId
            });
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = DbContext.Current.GetCloudDrives().Find(s => s.Id == id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(new ModelCloudDriveView() { Id = item.Id, Memo = item.Memo, Name = item.Name, Provider = item.Provider });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var item = DbContext.Current.GetCloudDrives().Find(s => s.Id == id);
            if (item != null)
            {
                DbContext.Current.Delete(item);
            }
            return RedirectToAction("Index");
        }
    }
}