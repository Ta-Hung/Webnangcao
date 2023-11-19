using MaverikStudio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security.AntiXss;

namespace MaverikStudio.Controllers
{
    public class BrandController : Controller
    {
        private WebMaverikStudioEntities1 db = new WebMaverikStudioEntities1();
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "List brand";

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

                TempData["brand_search"] = search;

                var brands = db.brands
                .Where(u => u.name.Contains(search))
                .OrderByDescending(u => u.created_at)
                .Skip((page - 1) * countPage)
                .Take(countPage)
                .ToList();

                int totalPage = (int)Math.Ceiling((double)db.brands.Where(u => u.name.Contains(search)).ToList().Count / countPage);

                TempData["brand_page"] = page;
                TempData["brand_count_page"] = countPage;
                TempData["brand_total_page"] = totalPage;

                return View(brands);
            }

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult SeeDetail(int id)
        {

            var brand = db.brands.AsNoTracking().Select(u => new
            {
                u.id,
                u.name,
                u.address,
                u.description,
                u.email,
                u.phone_number,
                created_at = u.created_at,
                updated_at = u.updated_at
            }).FirstOrDefault(u => u.id == id);

            var brandData = new
            {
                id = brand.id,
                name = brand.name,
                address = brand.address,
                description = brand.description,
                email = brand.email,
                phone_number = brand.phone_number,
                created_at = string.Format("{0:dd-MM-yyyy HH:mm:ss}", brand.created_at),
                updated_at = string.Format("{0:dd-MM-yyyy HH:mm:ss}", brand.updated_at)
            };

            return Json(brandData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Update brand";

                var brand = db.brands.First(u => u.id == id);

                if (brand == null)
                {
                    return RedirectToAction("Index");
                }

                return View(brand);
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
                var brand = db.brands.First(u => u.id == id);
                if (brand == null)
                {
                    return RedirectToAction("Index");
                }
                if (Validate(id))
                {
                    brand.name = Request.Form["name"];
                    brand.address = Request.Form["address"];
                    brand.description = Request.Form["description"];
                    brand.email = Request.Form["email"];
                    brand.phone_number = Request.Form["phone_number"];
                    db.SaveChanges();
                    TempData["msg"] = "Sửa thành công";
                    return RedirectToAction("Update", "Brand", new { id = id });
                }

                TempData["name"] = Request.Form["name"];
                TempData["address"] = Request.Form["address"];
                TempData["description"] = Request.Form["description"];
                TempData["email"] = Request.Form["email"];
                TempData["phone_number"] = Request.Form["phone_number"];
                return RedirectToAction("Update", "Brand", new { id = id });
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Create brand";
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
                    brand brand = new brand();
                    
                    brand.name = Request.Form["name"];
                    brand.address = Request.Form["address"];
                    brand.email = Request.Form["email"];
                    brand.phone_number = Request.Form["phone_number"];
                    brand.description = Request.Form["description"];
                    brand.created_at = DateTime.Now;
                    brand.updated_at = DateTime.Now;
                    db.brands.Add(brand);
                    db.SaveChanges();

                    TempData["msg"] = "Thêm thành công";
                    return RedirectToAction("Index");
                }

                TempData["name"] = Request.Form["name"];
                TempData["address"] = Request.Form["address"];
                TempData["email"] = Request.Form["email"];
                TempData["phone_number"] = Request.Form["phone_number"];
                TempData["description"] = Request.Form["description"];

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
                var brand = db.brands.Find(id);
                int quantity = brand.products.Count();

                if (quantity > 0)
                {
                    TempData["err"] = $"Thương hiệu này có {quantity} sản phẩm nên không thể xóa";
                    return RedirectToAction("Index");
                }
                db.brands.Remove(brand);
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
            string test = Request.Form["name"];
            if (Request.Form["name"] == "")
            {
                TempData["err_brand_name"] = "Tên thương hiệu không được để trống";
                check = false;
            }
            if (Request.Form["address"] == "")
            {
                TempData["err_brand_address"] = "Địa chỉ thương hiệu không được để trống";
                check = false;
            }
            string email = Request.Form["email"];
            if (Request.Form["email"] == "")
            {
                TempData["err_brand_email"] = "Email không được để trống";
                check = false;
            }
            else if (!Regex.IsMatch(Request.Form["email"], @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$"))
            {
                TempData["err_brand_email"] = "Email không đúng định dạng";
                check = false;
            }
            else if (db.brands.Where(m => m.email == email && m.id != id).ToList().Count > 0)
            {
                TempData["err_brand_email"] = "Email đã tồn tại";
                check = false;
            }
            if (Request.Form["phone_number"] == "")
            {
                TempData["err_brand_phone_number"] = "Số điện thoại không được để trống";
                check = false;
            }
            else if (!Regex.IsMatch(Request.Form["phone_number"], @"^0[1345789][0-9]{8}$"))
            {
                TempData["err_brand_phone_number"] = "Số điện thoại đúng định dạng";
                check = false;
            }
            // Kiểm tra mô tả có rỗng hay không
            if (Request.Form["description"] == "")
            {
                TempData["err_brand_description"] = "Mô tả thương hiệu không được để trống";
                check = false;
            }
            return check;
        }
    }
}