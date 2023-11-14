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
    public class UserController : Controller
    {
        private WebMaverikStudioEntities1 db = new WebMaverikStudioEntities1();
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "List user";

                string search = Request.QueryString["search"] != null ? Request.QueryString["search"] : "";
                int filter_group = 0;

                if (Request.QueryString["filter_group"] != null)
                {
                    int.TryParse(Request.QueryString["filter_group"], out filter_group);
                }

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

                TempData["user_search"] = search;
                TempData["user_filter_group"] = filter_group;

                var users = db.users
                .Where(u => u.name.Contains(search) && (filter_group == 0 || u.group_id == filter_group))
                .OrderByDescending(u => u.created_at)
                .Skip((page - 1) * countPage)
                .Take(countPage)
                .ToList();

                int totalPage = (int)Math.Ceiling((double)db.users.Where(u => u.name.Contains(search) && (filter_group == 0 || u.group_id == filter_group)).ToList().Count / countPage);

                TempData["user_page"] = page;
                TempData["user_count_page"] = countPage;
                TempData["user_total_page"] = totalPage;

                return View(users);
            }

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult SeeDetail(int id)
        {

            var user = db.users.AsNoTracking().Select(u => new
            {
                u.id,
                u.name,
                u.thumbnail,
                u.gender,
                u.address,
                u.email,
                u.phone_number,
                u.salary,
                date_of_birth = u.date_of_birth,
                group = u.group.name, // Thêm trường tên group
                created_at = u.created_at,
                updated_at = u.updated_at
            }).FirstOrDefault(u => u.id == id);

            var userData = new
            {
                id = user.id,
                name = user.name,
                thumbnail = user.thumbnail,
                gender = user.gender,
                address = user.address,
                email = user.email,
                phone_number = user.phone_number,
                salary = user.salary,
                date_of_birth = user.date_of_birth.ToString("dd-MM-yyyy"),
                group = user.group,
                created_at = string.Format("{0:dd-MM-yyyy HH:mm:ss}", user.created_at),
                updated_at = string.Format("{0:dd-MM-yyyy HH:mm:ss}", user.updated_at)
            };

            return Json(userData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Update user";

                var user = db.users.First(u => u.id == id);

                if (user == null)
                {
                    return RedirectToAction("Index");
                }

                return View(user);
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleUpdate(int id)
        {
            if (Session["user_id"] != null)
            {
                var user = db.users.First(u => u.id == id);
                if (user == null)
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
                        user.thumbnail = "/assets/uploads/" + uniqueFileName;
                    }

                    user.name = Request.Form["name"];
                    user.gender = Request.Form["gender"];
                    user.address = Request.Form["address"];
                    user.email = Request.Form["email"];
                    user.phone_number = Request.Form["phone_number"];
                    if (Request.Form["password"] != null && !Request.Form["password"].Equals(""))
                    {
                        user.password = MaverikStudio.Helpers.HelperMD5.GetMD5(Request.Form["password"]);
                    }
                    user.salary = double.Parse(Request.Form["salary"]);
                    user.date_of_birth = DateTime.Parse(Request.Form["date_of_birth"]);
                    user.updated_at = DateTime.Now;
                    user.group_id = int.Parse(Request.Form["group_id"]);
                    db.SaveChanges();
                    TempData["msg"] = "Sửa thành công";
                    return RedirectToAction("Update", "User", new { id = id });
                }

                TempData["name"] = Request.Form["name"];
                TempData["gender"] = Request.Form["gender"];
                TempData["address"] = Request.Form["address"];
                TempData["email"] = Request.Form["email"];
                TempData["phone_number"] = Request.Form["phone_number"];
                TempData["password"] = Request.Form["password"];
                TempData["salary"] = Request.Form["salary"];
                TempData["date_of_birth"] = Request.Form["date_of_birth"];
                TempData["group_id"] = Request.Form["group_id"];
                return RedirectToAction("Update", "User", new { id = id });
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Create user";
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
                    user user = new user();
                    var fImage = Request.Files["thumbnail"];

                    if (fImage != null && fImage.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(fImage.FileName);
                        string fileExtension = Path.GetExtension(fileName);
                        string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                        string folder = Server.MapPath("~/assets/uploads/" + uniqueFileName);
                        fImage.SaveAs(folder);
                        user.thumbnail = "/assets/uploads/" + uniqueFileName;
                        user.name = Request.Form["name"];
                        user.gender = Request.Form["gender"];
                        user.address = Request.Form["address"];
                        user.email = Request.Form["email"];
                        user.phone_number = Request.Form["phone_number"];
                        user.password = MaverikStudio.Helpers.HelperMD5.GetMD5(Request.Form["password"]);
                        user.salary = double.Parse(Request.Form["salary"]);
                        user.date_of_birth = DateTime.Parse(Request.Form["date_of_birth"]);
                        user.created_at = DateTime.Now;
                        user.updated_at = DateTime.Now;
                        user.group_id = int.Parse(Request.Form["group_id"]);
                        db.users.Add(user);
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
                TempData["salary"] = Request.Form["salary"];
                TempData["date_of_birth"] = Request.Form["date_of_birth"];
                TempData["group_id"] = Request.Form["group_id"];
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
                var user = db.users.Find(id);

                db.users.Remove(user);
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
                TempData["err_user_name"] = "Tên người dùng không được để trống";
                check = false;
            }
            if (Request.Form["address"] == "")
            {
                TempData["err_user_address"] = "Địa chỉ người dùng không được để trống";
                check = false;
            }
            if (Request.Files["thumbnail"].ContentLength == 0 && id == 0)
            {
                TempData["err_user_thumbnail"] = "Ảnh người dùng không được để trống";
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
            string[] genders = { "Nam", "Nữ", "Khác" };
            if (Request.Form["gender"] == "" || Request.Form["gender"] == null)
            {
                TempData["err_user_gender"] = "Giới tính không được để trống";
                check = false;
            }
            else if (!genders.Contains(Request.Form["gender"]))
            {
                TempData["err_user_gender"] = "Giới tính phải là nam, nữ hoặc khác";
                check = false;
            }
            string email = Request.Form["email"];
            if (Request.Form["email"] == "")
            {
                TempData["err_user_email"] = "Email không được để trống";
                check = false;
            }
            else if (!Regex.IsMatch(Request.Form["email"], @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$"))
            {
                TempData["err_user_email"] = "Email không đúng định dạng";
                check = false;
            }
            else if (db.users.Where(m => m.email == email && m.id != id).ToList().Count > 0)
            {
                TempData["err_user_email"] = "Email đã tồn tại";
                check = false;
            }
            if (Request.Form["phone_number"] == "")
            {
                TempData["err_user_phone_number"] = "Số điện thoại không được để trống";
                check = false;
            }
            else if (!Regex.IsMatch(Request.Form["phone_number"], @"^0[1345789][0-9]{8}$"))
            {
                TempData["err_user_phone_number"] = "Số điện thoại đúng định dạng";
                check = false;
            }
            if (Request.Form["password"] == "" && id == 0)
            {
                TempData["err_user_password"] = "Mật khẩu không được để trống";
                check = false;
            }
            if (Request.Form["salary"] == "")
            {
                TempData["err_user_salary"] = "Lương không được để trống";
                check = false;
            }
            else if (!double.TryParse(Request.Form["salary"], out double result))
            {
                TempData["err_user_salary"] = "Lương phải là số thực";
                check = false;
            }
            if (Request.Form["date_of_birth"] == "")
            {
                TempData["err_user_date_of_birth"] = "Ngày sinh không được để trống";
                check = false;
            }
            else if (!DateTime.TryParse(Request.Form["date_of_birth"], out DateTime result))
            {
                TempData["err_user_date_of_birth"] = "Ngày sinh không đúng định dạng";
                check = false;
            }
            int groupId = 0;
            int.TryParse(Request.Form["group_id"], out groupId);
            if (Request.Form["group_id"] == "" || groupId <= 0 || db.groups.Find(groupId) == null)
            {
                TempData["err_user_group_id"] = "Nhóm không được để trống";
                check = false;
            }
            return check;
        }
    }
}