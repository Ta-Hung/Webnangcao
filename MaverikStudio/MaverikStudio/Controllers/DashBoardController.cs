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
        public ActionResult Index(string year)
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Dashboard";
                ViewBag.quantityOrderNew = db.orders.Where(o => o.status_id == 1).ToList().Count;
                ViewBag.quantityOrderTransformer = db.orders.Where(o => o.status_id == 2).ToList().Count;
                ViewBag.quantityClients = db.clients.ToList().Count;
                ViewBag.quantityInventory = db.product_details.Sum(pd => pd.quantity);
                int yearValue = DateTime.Now.Year;

                if (year != null)
                {
                    int.TryParse(year, out yearValue);
                }
                ViewBag.month1 = db.orders
                .Where(o => o.status_id == 3 && o.created_at.HasValue && o.created_at.Value.Year == yearValue && o.created_at.Value.Month == 1)
                .Sum(o => o.total_money);

                ViewBag.month2 = db.orders
                .Where(o => o.status_id == 3 && o.created_at.HasValue && o.created_at.Value.Year == yearValue && o.created_at.Value.Month == 2)
                .Sum(o => o.total_money);

                ViewBag.month3 = db.orders
                .Where(o => o.status_id == 3 && o.created_at.HasValue && o.created_at.Value.Year == yearValue && o.created_at.Value.Month == 3)
                .Sum(o => o.total_money);

                ViewBag.month4 = db.orders
                .Where(o => o.status_id == 3 && o.created_at.HasValue && o.created_at.Value.Year == yearValue && o.created_at.Value.Month == 4)
                .Sum(o => o.total_money);

                ViewBag.month5 = db.orders
                .Where(o => o.status_id == 3 && o.created_at.HasValue && o.created_at.Value.Year == yearValue && o.created_at.Value.Month == 5)
                .Sum(o => o.total_money);

                ViewBag.month6 = db.orders
                .Where(o => o.status_id == 3 && o.created_at.HasValue && o.created_at.Value.Year == yearValue && o.created_at.Value.Month == 6)
                .Sum(o => o.total_money);

                ViewBag.month7 = db.orders
                .Where(o => o.status_id == 3 && o.created_at.HasValue && o.created_at.Value.Year == yearValue && o.created_at.Value.Month == 7)
                .Sum(o => o.total_money);

                ViewBag.month8 = db.orders
                .Where(o => o.status_id == 3 && o.created_at.HasValue && o.created_at.Value.Year == yearValue && o.created_at.Value.Month == 8)
                .Sum(o => o.total_money);

                ViewBag.month9 = db.orders
                .Where(o => o.status_id == 3 && o.created_at.HasValue && o.created_at.Value.Year == yearValue && o.created_at.Value.Month == 9)
                .Sum(o => o.total_money);

                ViewBag.month10 = db.orders
                .Where(o => o.status_id == 3 && o.created_at.HasValue && o.created_at.Value.Year == yearValue && o.created_at.Value.Month == 10)
                .Sum(o => o.total_money);

                ViewBag.month11 = db.orders
                .Where(o => o.status_id == 3 && o.created_at.HasValue && o.created_at.Value.Year == yearValue && o.created_at.Value.Month == 11)
                .Sum(o => o.total_money);

                ViewBag.month12 = db.orders
                .Where(o => o.status_id == 3 && o.created_at.HasValue && o.created_at.Value.Year == yearValue && o.created_at.Value.Month == 12)
                .Sum(o => o.total_money);

                TempData["year"] = yearValue;

                return View();
            }
            return RedirectToAction("Login", "Auth");
        }
    }
}