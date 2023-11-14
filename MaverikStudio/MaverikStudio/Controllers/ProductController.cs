using MaverikStudio.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MaverikStudio.Controllers
{
    public class ProductController : Controller
    {
        private WebMaverikStudioEntities1 db = new WebMaverikStudioEntities1();
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "List product";

                string search = Request.QueryString["search"] != null ? Request.QueryString["search"] : "";
                int filter_brand = 0;

                if (Request.QueryString["filter_brand"] != null)
                {
                    int.TryParse(Request.QueryString["filter_brand"], out filter_brand);
                }

                int filter_category = 0;

                if (Request.QueryString["filter_category"] != null)
                {
                    int.TryParse(Request.QueryString["filter_category"], out filter_category);
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

                TempData["product_search"] = search;
                TempData["product_filter_brand"] = filter_brand;
                TempData["product_filter_category"] = filter_category;

                var products = db.products
                .Where(u => u.name.Contains(search) && (filter_brand == 0 || u.brand_id == filter_brand) && (filter_category == 0 || u.category_id == filter_category))
                .OrderByDescending(u => u.created_at)
                .Skip((page - 1) * countPage)
                .Take(countPage)
                .ToList();

                int totalPage = (int)Math.Ceiling((double)db.products.Where(u => u.name.Contains(search) && (filter_brand == 0 || u.brand_id == filter_brand) && (filter_category == 0 || u.category_id == filter_category)).ToList().Count / countPage);

                TempData["product_page"] = page;
                TempData["product_count_page"] = countPage;
                TempData["product_total_page"] = totalPage;

                return View(products);
            }

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult SeeDetail(int id)
        {

            var product = db.products.AsNoTracking().Select(u => new
            {
                u.id,
                u.name,
                u.brand_id,
                u.category_id,
                u.description,
                u.price,
                u.price_origin,
                u.sale,
                brand = u.brand.name,
                category = u.category.name,
                created_at = u.created_at,
                updated_at = u.updated_at
            }).FirstOrDefault(u => u.id == id);

            var productData = new
            {
                id = product.id,
                name = product.name,
                brand_id = product.brand_id,
                category_id = product.category_id,
                description = product.description,
                price = product.price,
                price_origin = product.price_origin,
                sale = product.sale,
                brand = product.brand,
                category = product.category,
                created_at = string.Format("{0:dd-MM-yyyy HH:mm:ss}", product.created_at),
                updated_at = string.Format("{0:dd-MM-yyyy HH:mm:ss}", product.updated_at)
            };

            return Json(productData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Update product";

                var product = db.products.First(u => u.id == id);

                if (product == null)
                {
                    return RedirectToAction("Index");
                }

                return View(product);
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
                var product = db.products.First(u => u.id == id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }
                if (Validate(id))
                {
                    var fImage = Request.Files["thumbnail"];

                    if (fImage != null && fImage.ContentLength > 0)
                    {
                        var productImage = db.product_images.FirstOrDefault(m => m.product_id == id);
                        if (productImage != null)
                        {
                            db.product_images.Remove(productImage);
                            db.SaveChanges();
                        }

                        string fileName = Path.GetFileName(fImage.FileName);
                        string fileExtension = Path.GetExtension(fileName);
                        string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                        string folder = Server.MapPath("~/assets/uploads/" + uniqueFileName);
                        fImage.SaveAs(folder);

                        product_images productImg = new product_images();
                        productImg.product_id = id;
                        productImg.image = "/assets/uploads/" + uniqueFileName;
                        productImg.created_at = DateTime.Now;
                        productImg.updated_at = DateTime.Now;

                        db.product_images.Add(productImg);
                        db.SaveChanges();
                    }

                    product.name = Request.Form["name"];
                    product.brand_id = int.Parse(Request.Form["brand_id"]);
                    product.category_id = int.Parse(Request.Form["category_id"]);
                    product.description = Request.Form["description"];
                    product.price = double.Parse(Request.Form["price"]);
                    product.price_origin = double.Parse(Request.Form["price_origin"]);
                    product.sale = double.Parse(Request.Form["sale"]);
                    product.updated_at = DateTime.Now;
                    db.SaveChanges();
                    TempData["msg"] = "Sửa thành công";
                    return RedirectToAction("Update", "Product", new { id = id });
                }

                TempData["name"] = Request.Form["name"];
                TempData["brand_id"] = Request.Form["brand_id"];
                TempData["category_id"] = Request.Form["category_id"];
                TempData["description"] = Request.Form["description"];
                TempData["price"] = Request.Form["price"];
                TempData["price_origin"] = Request.Form["price_origin"];
                TempData["sale"] = Request.Form["sale"];
                return RedirectToAction("Update", "Product", new { id = id });
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Create product";
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
                    product product = new product();
                    product_images productImg = new product_images();
                    var fImage = Request.Files["thumbnail"];

                    if (fImage != null && fImage.ContentLength > 0)
                    {
                        product.name = Request.Form["name"];
                        product.brand_id = int.Parse(Request.Form["brand_id"]);
                        product.category_id = int.Parse(Request.Form["category_id"]);
                        product.description = Request.Form["description"];
                        product.price = double.Parse(Request.Form["price"]);
                        product.price_origin = double.Parse(Request.Form["price_origin"]);
                        product.sale = double.Parse(Request.Form["sale"]);
                        product.created_at = DateTime.Now;
                        product.updated_at = DateTime.Now;
                        db.products.Add(product);
                        db.SaveChanges();

                        string fileName = Path.GetFileName(fImage.FileName);
                        string fileExtension = Path.GetExtension(fileName);
                        string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                        string folder = Server.MapPath("~/assets/uploads/" + uniqueFileName);
                        fImage.SaveAs(folder);
                        productImg.image = "/assets/uploads/" + uniqueFileName;
                        productImg.product_id = product.id;
                        productImg.created_at = DateTime.Now;
                        productImg.updated_at = DateTime.Now;

                        db.product_images.Add(productImg);
                        db.SaveChanges();

                        TempData["msg"] = "Thêm thành công";
                    }

                    return RedirectToAction("Index");
                }

                TempData["name"] = Request.Form["name"];
                TempData["brand_id"] = Request.Form["brand_id"];
                TempData["category_id"] = Request.Form["category_id"];
                TempData["description"] = Request.Form["description"];
                TempData["price"] = Request.Form["price"];
                TempData["price_origin"] = Request.Form["price_origin"];
                TempData["sale"] = Request.Form["sale"];
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
                var product = db.products.Find(id);

                db.products.Remove(product);
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
            string name = Request.Form["name"];
            if (Request.Form["name"] == "")
            {
                TempData["err_product_name"] = "Tên sản phẩm không được để trống";
                check = false;
            }
            else if (db.products.Where(m => m.name == name && m.id != id).ToList().Count > 0)
            {
                TempData["err_product_name"] = "Tên sản phẩm đã tồn tại";
                check = false;
            }
            int brandId = 0;
            int.TryParse(Request.Form["brand_id"], out brandId);
            if (Request.Form["brand_id"] == "" || brandId <= 0 || db.brands.Find(brandId) == null)
            {
                TempData["err_product_brand_id"] = "Thương hiệu không được để trống";
                check = false;
            }
            int categoryId = 0;
            int.TryParse(Request.Form["category_id"], out categoryId);
            if (Request.Form["category_id"] == "" || categoryId <= 0 || db.categories.Find(categoryId) == null)
            {
                TempData["err_product_category_id"] = "Chuyên mục không được để trống";
                check = false;
            }
            if (Request.Form["description"] == "")
            {
                TempData["err_product_description"] = "Mô tả sản phẩm không được để trống";
                check = false;
            }
            if (Request.Files["thumbnail"].ContentLength == 0 && id == 0)
            {
                TempData["err_user_thumbnail"] = "Ảnh sản phẩm không được để trống";
                check = false;
            }
            else if (Request.Files["thumbnail"] != null && Request.Files["thumbnail"].ContentLength > 0)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                string fileExtension = Path.GetExtension(Request.Files["thumbnail"].FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    TempData["err_user_thumbnail"] = "Ảnh không đúng định dạng";
                    check = false;
                }
            }

            if (Request.Form["price"] == "")
            {
                TempData["err_user_price"] = "Giá bán không được để trống";
                check = false;
            }
            else if (!double.TryParse(Request.Form["price"], out double result))
            {
                TempData["err_user_price"] = "Giá bán phải là số thực";
                check = false;
            }

            if (Request.Form["price_origin"] == "")
            {
                TempData["err_user_price_origin"] = "Giá gốc không được để trống";
                check = false;
            }
            else if (!double.TryParse(Request.Form["price_origin"], out double result))
            {
                TempData["err_user_price_origin"] = "Giá gốc phải là số thực";
                check = false;
            }

            double sale = 0;
            if (Request.Form["sale"] == "")
            {
                TempData["err_user_sale"] = "Giảm giá không được để trống";
                check = false;
            }
            else if (!double.TryParse(Request.Form["sale"], out sale))
            {
                TempData["err_user_sale"] = "Giảm giá phải là số thực";
                check = false;
            }
            else if(sale < 0 || sale > 100)
            {
                TempData["err_user_sale"] = "Giảm giá phải lớn hơn hoặc bằng 0 và nhỏ hơn hoặc bằng 100";
                check = false;
            }

            return check;
        }
    }
}