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
    public class ClientController : Controller
    {
        private WebMaverikStudioEntities1 db = new WebMaverikStudioEntities1();
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "List client";

                string search = Request.QueryString["search"] != null ? Request.QueryString["search"] : "";

                int page = 1;

                if (Request.QueryString["page"] != null)
                {
                    int.TryParse(Request.QueryString["page"], out page);
                }

                if (page <= 0) page = 1;

                int countPage = 1;

                if (Request.QueryString["count_page"] != null)
                {
                    int.TryParse(Request.QueryString["count_page"], out countPage);
                }

                if (countPage < 0) countPage = 1;

                TempData["client_search"] = search;

                var clients = db.clients
                .Where(u => u.name.Contains(search))
                .OrderByDescending(u => u.created_at)
                .Skip((page - 1) * countPage)
                .Take(countPage)
                .ToList();

                int totalPage = (int)Math.Ceiling((double)db.clients.Where(u => u.name.Contains(search)).ToList().Count / countPage);

                TempData["client_page"] = page;
                TempData["client_count_page"] = countPage;
                TempData["client_total_page"] = totalPage;

                return View(clients);
            }

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult SeeDetail(int id)
        {

            var client = db.clients.AsNoTracking().Select(u => new
            {
                u.id,
                u.name,
                u.thumbnail,
                u.gender,
                u.address,
                u.email,
                u.phone_number,
                date_of_birth = u.date_of_birth,
                created_at = u.created_at,
                updated_at = u.updated_at
            }).FirstOrDefault(u => u.id == id);

            var clientData = new
            {
                id = client.id,
                name = client.name,
                thumbnail = client.thumbnail,
                gender = client.gender,
                address = client.address,
                email = client.email,
                phone_number = client.phone_number,
                date_of_birth = client.date_of_birth.ToString("dd-MM-yyyy"),
                created_at = string.Format("{0:dd-MM-yyyy HH:mm:ss}", client.created_at),
                updated_at = string.Format("{0:dd-MM-yyyy HH:mm:ss}", client.updated_at)
            };

            return Json(clientData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Update client";

                var client = db.clients.First(u => u.id == id);

                if (client == null)
                {
                    return RedirectToAction("Index");
                }

                return View(client);
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleUpdate(int id)
        {
            if (Session["user_id"] != null)
            {
                var client = db.clients.First(u => u.id == id);
                if (client == null)
                {
                    return RedirectToAction("Index");
                }
                if (Validate(id))
                {
                    var fImage = Request.Files["thumbnail"];

                    if (fImage != null && fImage.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(fImage.FileName);
                        string fileExtension = Path.GetExtension(fileName);
                        string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                        string folder = Server.MapPath("~/assets/uploads/" + uniqueFileName);
                        fImage.SaveAs(folder);
                        client.thumbnail = "/assets/uploads/" + uniqueFileName;
                    }

                    client.name = Request.Form["name"];
                    client.gender = Request.Form["gender"];
                    client.address = Request.Form["address"];
                    client.email = Request.Form["email"];
                    client.phone_number = Request.Form["phone_number"];
                    if (Request.Form["password"] != null && !Request.Form["password"].Equals(""))
                    {
                        client.password = MaverikStudio.Helpers.HelperMD5.GetMD5(Request.Form["password"]);
                    }
                    client.date_of_birth = DateTime.Parse(Request.Form["date_of_birth"]);
                    client.updated_at = DateTime.Now;
                    db.SaveChanges();
                    TempData["msg"] = "Sửa thành công";
                    return RedirectToAction("Update", "Client", new { id = id });
                }

                TempData["name"] = Request.Form["name"];
                TempData["gender"] = Request.Form["gender"];
                TempData["address"] = Request.Form["address"];
                TempData["email"] = Request.Form["email"];
                TempData["phone_number"] = Request.Form["phone_number"];
                TempData["password"] = Request.Form["password"];
                TempData["date_of_birth"] = Request.Form["date_of_birth"];
                return RedirectToAction("Update", "Client", new { id = id });
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Create client";
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
                    client client = new client();
                    var fImage = Request.Files["thumbnail"];

                    if (fImage != null && fImage.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(fImage.FileName);
                        string fileExtension = Path.GetExtension(fileName);
                        string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                        string folder = Server.MapPath("~/assets/uploads/" + uniqueFileName);
                        fImage.SaveAs(folder);
                        client.thumbnail = "/assets/uploads/" + uniqueFileName;
                        client.name = Request.Form["name"];
                        client.gender = Request.Form["gender"];
                        client.address = Request.Form["address"];
                        client.email = Request.Form["email"];
                        client.phone_number = Request.Form["phone_number"];
                        client.password = MaverikStudio.Helpers.HelperMD5.GetMD5(Request.Form["password"]);
                        client.date_of_birth = DateTime.Parse(Request.Form["date_of_birth"]);
                        client.created_at = DateTime.Now;
                        client.updated_at = DateTime.Now;
                        db.clients.Add(client);
                        db.SaveChanges();

                        TempData["msg"] = "Thêm thành công";
                    }

                    return RedirectToAction("Index");
                }

                TempData["name"] = Request.Form["name"];
                TempData["gender"] = Request.Form["gender"];
                TempData["address"] = Request.Form["address"];
                TempData["email"] = Request.Form["email"];
                TempData["phone_number"] = Request.Form["phone_number"];
                TempData["password"] = Request.Form["password"];
                TempData["date_of_birth"] = Request.Form["date_of_birth"];
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
                var client = db.clients.Find(id);

                db.clients.Remove(client);
                db.SaveChanges();

                TempData["msg"] = "Xóa thành công";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Login", "Auth");
        }


        public bool Validate(int id = 0)
        {
            bool check = true;
            string test = Request.Form["name"];
            if (Request.Form["name"] == "")
            {
                TempData["err_client_name"] = "Tên khách hàng không được để trống";
                check = false;
            }
            if (Request.Form["address"] == "")
            {
                TempData["err_client_address"] = "Địa chỉ khách hàng không được để trống";
                check = false;
            }
            if (Request.Files["thumbnail"].ContentLength == 0 && id == 0)
            {
                TempData["err_client_thumbnail"] = "Ảnh khách hàng không được để trống";
                check = false;
            }
            else if (Request.Files["thumbnail"] != null && Request.Files["thumbnail"].ContentLength > 0)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
                string fileExtension = Path.GetExtension(Request.Files["thumbnail"].FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    TempData["err_client_thumbnail"] = "Ảnh không đúng định dạng";
                    check = false;
                }
            }
            string[] genders = { "Nam", "Nữ", "Khác" };
            if (Request.Form["gender"] == "" || Request.Form["gender"] == null)
            {
                TempData["err_client_gender"] = "Giới tính không được để trống";
                check = false;
            }
            else if (!genders.Contains(Request.Form["gender"]))
            {
                TempData["err_client_gender"] = "Giới tính phải là nam, nữ hoặc khác";
                check = false;
            }
            string email = Request.Form["email"];
            if (Request.Form["email"] == "")
            {
                TempData["err_client_email"] = "Email không được để trống";
                check = false;
            }
            else if (!Regex.IsMatch(Request.Form["email"], @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$"))
            {
                TempData["err_client_email"] = "Email không đúng định dạng";
                check = false;
            }
            else if (db.clients.Where(m => m.email == email && m.id != id).ToList().Count > 0)
            {
                TempData["err_client_email"] = "Email đã tồn tại";
                check = false;
            }
            if (Request.Form["phone_number"] == "")
            {
                TempData["err_client_phone_number"] = "Số điện thoại không được để trống";
                check = false;
            }
            else if (!Regex.IsMatch(Request.Form["phone_number"], @"^0[1345789][0-9]{8}$"))
            {
                TempData["err_client_phone_number"] = "Số điện thoại đúng định dạng";
                check = false;
            }
            if (Request.Form["password"] == "" && id == 0)
            {
                TempData["err_client_password"] = "Mật khẩu không được để trống";
                check = false;
            }
            if (Request.Form["date_of_birth"] == "")
            {
                TempData["err_client_date_of_birth"] = "Ngày sinh không được để trống";
                check = false;
            }
            else if (!DateTime.TryParse(Request.Form["date_of_birth"], out DateTime result))
            {
                TempData["err_client_date_of_birth"] = "Ngày sinh không đúng định dạng";
                check = false;
            }
            
            return check;
        }
    }
}