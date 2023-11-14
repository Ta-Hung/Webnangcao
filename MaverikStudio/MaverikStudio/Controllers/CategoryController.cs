using MaverikStudio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaverikStudio.Controllers
{
    public class CategoryController : Controller
    {
        private WebMaverikStudioEntities1 db = new WebMaverikStudioEntities1();
        [HttpGet]
        public ActionResult Index()
        {

            if (Session["user_id"] != null)
            {
                ViewBag.title = "List category";

                string search = Request.QueryString["search"] != null ? Request.QueryString["search"] : "";

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

                TempData["category_search"] = search;

                var categories = db.categories
                .Where(u => u.name.Contains(search))
                .OrderByDescending(u => u.created_at)
                .Skip((page - 1) * countPage)
                .Take(countPage)
                .ToList();

                int totalPage = (int)Math.Ceiling((double)db.categories.Where(u => u.name.Contains(search)).ToList().Count / countPage);

                TempData["category_page"] = page;
                TempData["category_count_page"] = countPage;
                TempData["category_total_page"] = totalPage;

                return View(categories);
            }

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult SeeDetail(int id)
        {

            var category = db.categories.AsNoTracking().Select(u => new
            {
                u.id,
                u.name,
                created_at = u.created_at,
                updated_at = u.updated_at
            }).FirstOrDefault(u => u.id == id);

            var categoryData = new
            {
                id = category.id,
                name = category.name,
                created_at = string.Format("{0:dd-MM-yyyy HH:mm:ss}", category.created_at),
                updated_at = string.Format("{0:dd-MM-yyyy HH:mm:ss}", category.updated_at)
            };

            return Json(categoryData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Update category";

                var category = db.categories.First(u => u.id == id);

                if (category == null)
                {
                    return RedirectToAction("Index");
                }

                return View(category);
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleUpdate(int id)
        {
            if (Session["user_id"] != null)
            {
                var category = db.categories.First(u => u.id == id);
                if (category == null)
                {
                    return RedirectToAction("Index");
                }
                if (Validate())
                {
                    category.name = Request.Form["name"];
                    category.updated_at = DateTime.Now;
                    db.SaveChanges();
                    TempData["msg"] = "Sửa thành công";
                    return RedirectToAction("Update", "Category", new { id = id });
                }

                TempData["name"] = Request.Form["name"];
                return RedirectToAction("Update", "Category", new { id = id });
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Create category";
                return View();
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleCreate()
        {
            if (Session["user_id"] != null)
            {
                if (Validate())
                {
                    category category = new category();

                    category.name = Request.Form["name"];
                    category.created_at = DateTime.Now;
                    category.updated_at = DateTime.Now;
                    db.categories.Add(category);
                    db.SaveChanges();

                    TempData["msg"] = "Thêm thành công";
                    return RedirectToAction("Index");
                }

                TempData["name"] = Request.Form["name"];
                return RedirectToAction("Create");
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (Session["user_id"] != null)
            {
                var category = db.categories.Find(id);

                db.categories.Remove(category);
                db.SaveChanges();

                TempData["msg"] = "Xóa thành công";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Login", "Auth");
        }


        public bool Validate()
        {
            bool check = true;

            if (Request.Form["name"] == "")
            {
                TempData["err_category_name"] = "Tên chuyên mục không được để trống";
                check = false;
            }
            return check;
        }
    }
}