using MaverikStudio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaverikStudio.Controllers
{
    public class DashBoardController : Controller
    {
        private WebMaverikStudioEntities1 db = new WebMaverikStudioEntities1();
        // GET: DashBoard
        public ActionResult Index()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Dashboard";
                ViewBag.quantityOrderNew = db.orders.Where(o => o.status_id == 1).ToList().Count;
                ViewBag.quantityOrderTransformer = db.orders.Where(o => o.status_id == 2).ToList().Count;
                ViewBag.quantityClients = db.clients.ToList().Count;
                ViewBag.quantityInventory = db.product_details.Sum(pd => pd.quantity);

                return View();
            }
            return RedirectToAction("Login", "Auth");
        }
    }
}