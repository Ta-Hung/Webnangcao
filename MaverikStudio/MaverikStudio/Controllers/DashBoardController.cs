using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaverikStudio.Controllers
{
    public class DashBoardController : Controller
    {
        // GET: DashBoard
        public ActionResult Index()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Dashboard";
                return View();
            }
            return RedirectToAction("Login", "Auth");
        }
    }
}