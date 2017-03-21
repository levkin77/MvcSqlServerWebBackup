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
        /// Настройка провайдеров
        /// </summary>
        /// <returns></returns>
        public ActionResult ListDrive()
        {
            ViewBag.Message = "Настройка провайдеров.";

            return View();
        }
        /// <summary>
        /// Настройка заданий
        /// </summary>
        /// <returns></returns>
        public ActionResult ListTask()
        {
            ViewBag.Message = "Настройка заданий.";

            return View();
        }
    }
}