using MaverikStudio.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MaverikStudio.Controllers
{
    public class AuthController : Controller
    {
        private WebMaverikStudioEntities1 _db = new WebMaverikStudioEntities1();

        // GET: Auth
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["user_id"] == null)
            {
                return View();
            }
            return RedirectToAction("Index", "DashBoard");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var hashPassword = GetMD5(password);
                var data = _db.users.Where(s => s.email.Equals(email) && s.password.Equals(hashPassword)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["user_id"] = data.FirstOrDefault().id;
                    Session["user_name"] = data.FirstOrDefault().name;
                    Session["user_email"] = data.FirstOrDefault().email;
                    Session["user_thumbnail"] = data.FirstOrDefault().thumbnail;
                    Session["user_gender"] = data.FirstOrDefault().gender;
                    Session["user_address"] = data.FirstOrDefault().address;
                    Session["user_phone_number"] = data.FirstOrDefault().phone_number;
                    Session["user_salary"] = data.FirstOrDefault().salary;
                    Session["user_date_of_birth"] = data.FirstOrDefault().date_of_birth;
                    Session["user_group_id"] = data.FirstOrDefault().group_id;
                    Session["user_group_name"] = data.FirstOrDefault().group.name;
                    return RedirectToAction("Index", "DashBoard");
                }
                else
                {
                    Session["login_err"] = "Tài khoản hoặc mật khẩu không chính xác";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult ClientLogin()
        {
            if (Session["client_id"] == null)
            {
                return View();
            }
            return RedirectToAction("HomePage", "FrontEnd");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClientLogin(string password, string email)
        {
            if (ModelState.IsValid)
            {
                var hashPassword = GetMD5(password);
                var data = _db.clients.Where(s => s.email.Equals(email) && s.password.Equals(hashPassword)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["client_id"] = data.FirstOrDefault().id;
                    Session["client_name"] = data.FirstOrDefault().name;
                    Session["client_email"] = data.FirstOrDefault().email;
                    Session["client_thumbnail"] = data.FirstOrDefault().thumbnail;
                    Session["client_gender"] = data.FirstOrDefault().gender;
                    Session["client_address"] = data.FirstOrDefault().address;
                    Session["client_phone_number"] = data.FirstOrDefault().phone_number;
                    Session["client_date_of_birth"] = data.FirstOrDefault().date_of_birth;
                    return RedirectToAction("HomePage", "FrontEnd");
                }
                else
                {
                    TempData["email"] = Request.Form["email"];
                    TempData["client_login_err"] = "Tài khoản hoặc mật khẩu không chính xác";
                    return RedirectToAction("ClientLogin");
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClientLogout()
        {
            Session.Remove("client_id");
            Session.Remove("client_name");
            Session.Remove("client_email");
            Session.Remove("client_thumbnail");
            Session.Remove("client_gender");
            Session.Remove("client_address");
            Session.Remove("client_phone_number");
            Session.Remove("client_date_of_birth");
            return RedirectToAction("HomePage", "FrontEnd");
        }

        [HttpGet]
        public ActionResult ClientRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleClientRegister()
        {
            if (Validate())
            {
                client client = new client();
                client.thumbnail = "/assets/plugins/admin-lte-3/dist/img/avatar5.png";
                client.name = Request.Form["name"];
                client.gender = Request.Form["gender"];
                client.address = Request.Form["address"];
                client.email = Request.Form["email"];
                client.phone_number = Request.Form["phone_number"];
                client.password = MaverikStudio.Helpers.HelperMD5.GetMD5(Request.Form["password"]);
                client.date_of_birth = DateTime.Parse(Request.Form["date_of_birth"]);
                client.created_at = DateTime.Now;
                client.updated_at = DateTime.Now;
                _db.clients.Add(client);
                _db.SaveChanges();

                return RedirectToAction("ClientProfile", "Auth");
            }

            TempData["name"] = Request.Form["name"];
            TempData["gender"] = Request.Form["gender"];
            TempData["address"] = Request.Form["address"];
            TempData["email"] = Request.Form["email"];
            TempData["phone_number"] = Request.Form["phone_number"];
            TempData["password"] = Request.Form["password"];
            TempData["date_of_birth"] = Request.Form["date_of_birth"];
            return RedirectToAction("ClientRegister");
        }

        //Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        public bool Validate(int id = 0)
        {
            bool check = true;
            string test = Request.Form["name"];
            if (Request.Form["name"] == "")
            {
                TempData["err_client_name"] = "Tên không được để trống";
                check = false;
            }
            if (Request.Form["address"] == "")
            {
                TempData["err_client_address"] = "Địa chỉ không được để trống";
                check = false;
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
            else if (_db.clients.Where(m => m.email == email && m.id != id).ToList().Count > 0)
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