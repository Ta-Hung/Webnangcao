using MaverikStudio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MaverikStudio.Controllers
{
    public class SizeController : Controller
    {
        private WebMaverikStudioEntities1 db = new WebMaverikStudioEntities1();
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "List size";

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

                TempData["size_search"] = search;

                var sizes = db.sizes
                .Where(u => u.size1.Contains(search))
                .OrderByDescending(u => u.created_at)
                .Skip((page - 1) * countPage)
                .Take(countPage)
                .ToList();

                int totalPage = (int)Math.Ceiling((double)db.sizes.Where(u => u.size1.Contains(search)).ToList().Count / countPage);
                if (totalPage == 0) totalPage = 1;
                TempData["size_page"] = page;
                TempData["size_count_page"] = countPage;
                TempData["size_total_page"] = totalPage;

                return View(sizes);
            }

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult SeeDetail(int id)
        {

            var size = db.sizes.AsNoTracking().Select(u => new
            {
                u.id,
                u.size1,
                u.height,
                u.weight,
                u.width,
                u.lenght,
                created_at = u.created_at,
                updated_at = u.updated_at
            }).FirstOrDefault(u => u.id == id);

            var sizeData = new
            {
                id = size.id,
                size1 = size.size1,
                height = size.height,
                weight = size.weight,
                width = size.width,
                lenght = size.lenght,
                created_at = string.Format("{0:dd-MM-yyyy HH:mm:ss}", size.created_at),
                updated_at = string.Format("{0:dd-MM-yyyy HH:mm:ss}", size.updated_at)
            };

            return Json(sizeData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Update size";

                var size = db.sizes.First(u => u.id == id);

                if (size == null)
                {
                    return RedirectToAction("Index");
                }

                return View(size);
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult HandleUpdate(int id)
        {
            if (Session["user_id"] != null)
            {
                var size = db.sizes.First(u => u.id == id);
                if (size == null)
                {
                    return RedirectToAction("Index");
                }
                if (Validate(id))
                {
                    size.size1 = Request.Form["size1"];
                    size.height = Request.Form["height"];
                    size.weight = Request.Form["weight"];
                    size.width = Request.Form["width"];
                    size.lenght = Request.Form["lenght"];
                    db.SaveChanges();
                    TempData["msg"] = "Sửa thành công";
                    return RedirectToAction("Update", "Size", new { id = id });
                }

                TempData["size1"] = Request.Form["size1"];
                TempData["height"] = Request.Form["height"];
                TempData["weight"] = Request.Form["weight"];
                TempData["width"] = Request.Form["width"];
                TempData["lenght"] = Request.Form["lenght"];
                return RedirectToAction("Update", "Size", new { id = id });
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Create size";
                return View();
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult HandleCreate()
        {
            if (Session["user_id"] != null)
            {
                if (Validate())
                {
                    size size = new size();

                    size.size1 = Request.Form["size1"];
                    size.height = Request.Form["height"];
                    size.weight = Request.Form["weight"];
                    size.width = Request.Form["width"];
                    size.lenght = Request.Form["lenght"];
                    size.created_at = DateTime.Now;
                    size.updated_at = DateTime.Now;
                    db.sizes.Add(size);
                    db.SaveChanges();

                    TempData["msg"] = "Thêm thành công";
                    return RedirectToAction("Index");
                }

                TempData["size1"] = Request.Form["size1"];
                TempData["height"] = Request.Form["height"];
                TempData["weight"] = Request.Form["weight"];
                TempData["width"] = Request.Form["width"];
                TempData["lenght"] = Request.Form["lenght"];

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
                var size = db.sizes.Find(id);
                int quantity = size.product_details.Count();

                if(quantity > 0)
                {
                    TempData["err"] = $"Còn {quantity} chi tiết sản phẩm dùng size này";
                    return RedirectToAction("Index");
                }
                db.sizes.Remove(size);
                db.SaveChanges();

                TempData["msg"] = "Xóa thành công";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Login", "Auth");
        }

        [ValidateInput(false)]
        public bool Validate(int id = 0)
        {
            bool check = true;

            string test = Request.Form["size1"];
            string sizeName = Request.Form["size1"];
            if (Request.Form["size1"] == "")
            {
                TempData["err_size_size1"] = "Tên size không được để trống";
                check = false;
            }
            else if (db.sizes.Where(m => m.size1 == sizeName && m.id != id).ToList().Count > 0)
            {
                TempData["err_size_size1"] = "Size đã tồn tại";
                check = false;
            }
            else if(sizeName.Length > 20)
            {
                TempData["err_size_size1"] = "Size tối đa 20 ký tự";
                check = false;
            }

            return check;
        }
    }
}