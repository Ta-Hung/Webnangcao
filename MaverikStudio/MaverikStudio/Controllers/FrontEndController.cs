using MaverikStudio.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MaverikStudio.Controllers
{
    public class FrontEndController : Controller
    {
        private WebMaverikStudioEntities1 db = new WebMaverikStudioEntities1();

        [HttpGet]
        public ActionResult HomePage()
        {
            ViewBag.title = "Maverik Studio";

            var products = db.products
                .OrderByDescending(u => u.created_at)
                .Skip(0)
                .Take(10)
                .ToList();

            return View(products);
        }

        [HttpGet]
        public ActionResult Collection(int id)
        {
            category currentCategory = db.categories.FirstOrDefault(m => m.id == id);
            ViewBag.title = currentCategory.name;
            ViewBag.idCategory = currentCategory.id;

            int page = 1;

            if (Request.QueryString["page"] != null)
            {
                int.TryParse(Request.QueryString["page"], out page);
            }

            if (page <= 0) page = 1;

            int countPage = 16;

            var products = db.products
                .Where(m => m.category_id == currentCategory.id)
                .OrderByDescending(u => u.created_at)
                .Skip((page - 1) * countPage)
                .Take(countPage)
                .ToList();

            int totalPage = (int)Math.Ceiling((double)db.products.Where(m => m.category_id == currentCategory.id).ToList().Count / countPage);

            TempData["product_page"] = page;
            TempData["product_count_page"] = countPage;
            TempData["product_total_page"] = totalPage;

            return View(products);
        }

        [HttpGet]
        public ActionResult NewArrivals()
        {
            ViewBag.title = "New Arrivals";

            int page = 1;

            if (Request.QueryString["page"] != null)
            {
                int.TryParse(Request.QueryString["page"], out page);
            }

            if (page <= 0) page = 1;

            int countPage = 16;

            DateTime threeMonthsAgo = DateTime.Now.AddMonths(-3);

            var products = db.products
                .Where(m => m.created_at >= threeMonthsAgo)
                .OrderByDescending(u => u.created_at)
                .Skip((page - 1) * countPage)
                .Take(countPage)
                .ToList();

            int totalPage = (int)Math.Ceiling((double)db.products.Where(m => m.created_at >= threeMonthsAgo).ToList().Count / countPage);

            TempData["product_page"] = page;
            TempData["product_count_page"] = countPage;
            TempData["product_total_page"] = totalPage;

            return View(products);
        }

        [HttpGet]
        public ActionResult Search(string search)
        {
            var products = db.products.AsNoTracking().Where(p => p.name.Contains(search))
                .OrderByDescending(p => p.created_at)
                .Skip(0)
                .Take(5)
                .Select(p => new
                {
                    p.id,
                    p.name,
                    price = p.price * (1 - p.sale / 100),
                    image = p.product_images.FirstOrDefault().image,
                }).ToList();

            List<object> data = new List<object>();

            foreach(var item in products)
            {
                data.Add(new
                {
                    id = item.id,
                    name = item.name,
                    price = string.Format("{0:#,##0}đ", item.price),
                    image = item.image
                });
            }

            data.Add(new
            {
                total = db.products.Where(p => p.name.Contains(search)).ToList().Count
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckQuantity(int id, int size_id, int quantity)
        {
            var product_detail = db.product_details.Where(p => p.product_id == id && p.size_id == size_id && p.quantity_ready >= quantity).FirstOrDefault();

            object data = new
            {
                check = "true"
            };

            if(product_detail == null)
            {
                data = new
                {
                    check = "false"
                };
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ResultAll(string search)
        {
            ViewBag.search = search;
            int page = 1;

            if (Request.QueryString["page"] != null)
            {
                int.TryParse(Request.QueryString["page"], out page);
            }

            if (page <= 0) page = 1;

            int countPage = 8;

            var products = db.products
                .Where(m => m.name.Contains(search))
                .OrderByDescending(u => u.created_at)
                .Skip((page - 1) * countPage)
                .Take(countPage)
                .ToList();

            int totalPage = (int)Math.Ceiling((double)db.products.Where(m => m.name.Contains(search)).ToList().Count / countPage);

            TempData["product_page"] = page;
            TempData["product_count_page"] = countPage;
            TempData["product_total_page"] = totalPage;

            return View(products);
        }

        [HttpGet]
        public ActionResult Product(int id)
        {
            product currentProduct = db.products.FirstOrDefault(m => m.id == id);

            ViewBag.title = currentProduct.name;
            ViewBag.currentProductImgs = currentProduct.product_images.ToList();
            ViewBag.currentProductDetails = currentProduct.product_details.ToList();
            ViewBag.relateProducts = db.products.Where(
                m => m.category_id == currentProduct.category_id
                && m.id != currentProduct.id
            )
            .OrderByDescending(u => u.created_at)
            .Skip(0)
            .Take(12)
            .ToList();

            return View(currentProduct);
        }

        [HttpGet]
        public ActionResult Pay()
        {
            if (Session["client_id"] != null)
            {
                return View();
            }

            return RedirectToAction("ClientLogin", "Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandlePay()
        {
            if (Session["client_id"] != null)
            {
                if (Validate())
                {
                    HttpCookie gioHangCookie = Request.Cookies["gioHangCookie"];

                    // Kiểm tra xem cookie có tồn tại hay không
                    if (gioHangCookie != null && !string.IsNullOrEmpty(gioHangCookie.Value))
                    {
                        // Giải mã giá trị cookie, nếu đó là một chuỗi JSON
                        string gioHangValue = HttpUtility.UrlDecode(gioHangCookie.Value);

                        // Bạn có thể sử dụng giá trị cookie (đã giải mã) theo nhu cầu của bạn
                        // Ví dụ: Deserialize JSON thành một danh sách đối tượng
                        List<MapCookieCart> gioHangItems = JsonConvert.DeserializeObject<List<MapCookieCart>>(gioHangValue);

                        if (gioHangItems.Count > 0)
                        {
                            order order = new order();
                            order.client_id = (int)Session["client_id"];
                            order.status_id = 1;
                            order.address = Request.Form["address"];
                            order.total_money = 0;
                            order.phone_number = Request.Form["phone_number"];
                            order.created_at = DateTime.Now;
                            order.updated_at = DateTime.Now;

                            db.orders.Add(order);
                            db.SaveChanges();

                            foreach (MapCookieCart itemCart in gioHangItems)
                            {
                                product product = db.products.FirstOrDefault(m => m.id == itemCart.id);
                                size size = db.sizes.FirstOrDefault(m => m.id == itemCart.size_id);

                                if (product != null && size != null)
                                {
                                    order_details order_Details = new order_details();
                                    order_Details.order_id = order.id;
                                    order_Details.product_id = product.id;
                                    order_Details.size_id = size.id;
                                    order_Details.name = product.name;
                                    order_Details.size = size.size1;
                                    order_Details.quantity = itemCart.quantity;
                                    order_Details.price = product.price;
                                    order_Details.price_origin = product.price_origin;
                                    order_Details.sale = product.sale;
                                    order_Details.total_money = (product.price - product.price * product.sale / 100) * itemCart.quantity;
                                    order_Details.created_at = DateTime.Now;
                                    order_Details.updated_at = DateTime.Now;
                                    db.order_details.Add(order_Details);
                                    db.SaveChanges();

                                    product_details product_Details = db.product_details.FirstOrDefault(m => m.product_id == itemCart.id && m.size_id == itemCart.size_id);
                                    product_Details.quantity_ready -= itemCart.quantity;
                                    db.SaveChanges();
                                }
                            }

                            TempData["success"] = "Đặt hàng thành công";
                            return RedirectToAction("Pay", "FrontEnd");
                        }
                    }

                    TempData["success"] = "Giỏ hàng trống";
                    return RedirectToAction("Pay", "FrontEnd");
                }
                return RedirectToAction("Pay", "FrontEnd");
            }
            return RedirectToAction("ClientLogin", "Auth");
        }

        public bool Validate()
        {
            bool check = true;

            if (Request.Form["address"] == "")
            {
                TempData["err_pay_address"] = "Địa chỉ giao hàng không được để trống";
                check = false;
            }
            if (Request.Form["phone_number"] == "")
            {
                TempData["err_pay_phone_number"] = "Số điện thoại người nhận không được để trống";
                check = false;
            }
            else if (!Regex.IsMatch(Request.Form["phone_number"], @"^0[1345789][0-9]{8}$"))
            {
                TempData["err_pay_phone_number"] = "Số điện thoại không đúng định dạng";
                check = false;
            }

            return check;
        }

        class MapCookieCart
        {
            public int id;
            public int size_id;
            public string size;
            public string name;
            public double price;
            public int quantity;
            public string image;
        }
    }
}