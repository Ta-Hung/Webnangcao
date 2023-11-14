using MaverikStudio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

    }
}