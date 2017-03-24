using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcSqlServerWebBackup.Models;

namespace MvcSqlServerWebBackup.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        /// <summary>
        /// Настройка
        /// </summary>
        /// <returns></returns>
        public ActionResult Config()
        {
            ViewBag.Message = "Настройка заданий.";

            return View(DbContext.Current.Config);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Config([Bind(Include = "BackupLocation, ZipPassword")] AppConfig item)
        {
            if (ModelState.IsValid)
            {
                DbContext.Current.Config.BackupLocation = item.BackupLocation;
                DbContext.Current.Config.ZipPassword = item.ZipPassword;
                DbContext.Current.SaveAppConfig();
                return RedirectToAction("Index");
            }
            return View(item);
        }
    }
}