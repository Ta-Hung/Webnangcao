using MaverikStudio.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaverikStudio.Controllers
{
    public class OrderController : Controller
    {
        private WebMaverikStudioEntities1 db = new WebMaverikStudioEntities1();
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "List order";

                int filter_status = 1;

                if (Request.QueryString["filter_status"] != null)
                {
                    int.TryParse(Request.QueryString["filter_status"], out filter_status);
                }

                int page = 1;

                if (Request.QueryString["page"] != null)
                {
                    int.TryParse(Request.QueryString["page"], out page);
                }

                if (page <= 0) page = 1;

                int countPage = 10;

                if (Request.QueryString["count_page"] != null)
                {
                    int.TryParse(Request.QueryString["count_page"], out countPage);
                }

                if (countPage < 0) countPage = 10;

                TempData["order_filter_status"] = filter_status;

                var orders = db.orders
                .Where(u => (filter_status == 0 || u.status_id == filter_status))
                .OrderByDescending(u => u.created_at)
                .Skip((page - 1) * countPage)
                .Take(countPage)
                .ToList();

                int totalPage = (int)Math.Ceiling((double)db.orders.Where(u => (filter_status == 0 || u.status_id == filter_status)).ToList().Count / countPage);
                if (totalPage == 0) totalPage = 1;
                TempData["order_page"] = page;
                TempData["order_count_page"] = countPage;
                TempData["order_total_page"] = totalPage;

                return View(orders);
            }

            return RedirectToAction("Login", "Auth");
        }

        public bool Validate(int id = 0)
        {
            bool check = true;

            return check;
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Order detail";

                var order = db.orders.FirstOrDefault(m => m.id == id);
                if (order == null)
                {
                    return View("Index");
                }

                ViewBag.id = order.id;
                ViewBag.name = order.client.name;
                ViewBag.total_money = order.total_money;
                ViewBag.address = order.address;
                ViewBag.phone_number = order.phone_number;
                ViewBag.status = order.status.name;
                ViewBag.status_id = order.status_id;

                int page = 1;

                if (Request.QueryString["page"] != null)
                {
                    int.TryParse(Request.QueryString["page"], out page);
                }

                if (page <= 0) page = 1;

                int countPage = 10;

                if (Request.QueryString["count_page"] != null)
                {
                    int.TryParse(Request.QueryString["count_page"], out countPage);
                }

                if (countPage < 0) countPage = 10;

                var order_details = db.order_details
                .Where(m => m.order_id == id)
                .OrderByDescending(u => u.created_at)
                .Skip((page - 1) * countPage)
                .Take(countPage)
                .ToList();

                int totalPage = (int)Math.Ceiling((double)db.order_details.Where(m => m.order_id == id).ToList().Count / countPage);

                TempData["order_page"] = page;
                TempData["order_count_page"] = countPage;
                TempData["order_total_page"] = totalPage;

                return View(order_details);
            }

            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleUpdate(int id)
        {
            if (Session["user_id"] != null)
            {
                if (int.Parse(Request.Form["status"]) == 3)
                {
                    List<order_details> order_Details = db.order_details.Where(m => m.order_id == id).ToList();
                    foreach (order_details item in order_Details)
                    {
                        product_details product_Details = db.product_details.FirstOrDefault(m => m.product_id == item.product_id && m.size_id == item.size_id);
                        product_Details.quantity -= item.quantity;
                        db.SaveChanges();
                    }
                }
                else if (int.Parse(Request.Form["status"]) == 4)
                {
                    List<order_details> order_Details = db.order_details.Where(m => m.order_id == id).ToList();
                    foreach (order_details item in order_Details)
                    {
                        product_details product_Details = db.product_details.FirstOrDefault(m => m.product_id == item.product_id && m.size_id == item.size_id);
                        product_Details.quantity_ready += item.quantity;
                        db.SaveChanges();
                    }
                }

                order order = db.orders.FirstOrDefault(m => m.id == id);
                order.status_id = int.Parse(Request.Form["status"]);
                db.SaveChanges();

                TempData["msg"] = "Cập nhật thành công";
                return RedirectToAction("Detail", "Order", new {id = id});
            }

            return RedirectToAction("Login", "Auth");
        }

    }
}