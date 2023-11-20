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
    public class GroupController : Controller
    {
        private WebMaverikStudioEntities1 db = new WebMaverikStudioEntities1();
        [HttpGet]
        public ActionResult Index()
        {
            
            if (Session["user_id"] != null)
            {
                ViewBag.title = "List group";

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

                TempData["group_search"] = search;

                var groups = db.groups
                .Where(u => u.name.Contains(search))
                .OrderByDescending(u => u.created_at)
                .Skip((page - 1) * countPage)
                .Take(countPage)
                .ToList();

                int totalPage = (int)Math.Ceiling((double)db.groups.Where(u => u.name.Contains(search)).ToList().Count / countPage);
                if (totalPage == 0) totalPage = 1;
                TempData["group_page"] = page;
                TempData["group_count_page"] = countPage;
                TempData["group_total_page"] = totalPage;

                return View(groups);
            }

            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult SeeDetail(int id)
        {

            var group = db.groups.AsNoTracking().Select(u => new
            {
                u.id,
                u.name,
                created_at = u.created_at,
                updated_at = u.updated_at
            }).FirstOrDefault(u => u.id == id);

            var groupData = new
            {
                id = group.id,
                name = group.name,
                created_at = string.Format("{0:dd-MM-yyyy HH:mm:ss}", group.created_at),
                updated_at = string.Format("{0:dd-MM-yyyy HH:mm:ss}", group.updated_at)
            };

            return Json(groupData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Update group";

                var group = db.groups.First(u => u.id == id);

                if (group == null)
                {
                    return RedirectToAction("Index");
                }

                return View(group);
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleUpdate(int id)
        {
            if (Session["user_id"] != null)
            {
                var group = db.groups.First(u => u.id == id);
                if (group == null)
                {
                    return RedirectToAction("Index");
                }
                if (Validate())
                {
                    group.name = Request.Form["name"];
                    group.permission = "[]";
                    group.updated_at = DateTime.Now;
                    db.SaveChanges();
                    TempData["msg"] = "Sửa thành công";
                    return RedirectToAction("Update", "Group", new { id = id });
                }

                TempData["name"] = Request.Form["name"];
                return RedirectToAction("Update", "Group", new { id = id });
            }
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.title = "Create group";
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
                    group group = new group();
                    
                    group.name = Request.Form["name"];
                    group.permission = "[]";
                    group.created_at = DateTime.Now;
                    group.updated_at = DateTime.Now;
                    db.groups.Add(group);
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
                var group = db.groups.Find(id);

                int quantity = group.users.Count();

                if(quantity > 0) {
                    TempData["err"] = $"Trong nhóm còn {quantity} người dùng nên không thể xóa";
                    return RedirectToAction("Index");
                }
                db.groups.Remove(group);
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
                TempData["err_group_name"] = "Tên nhóm không được để trống";
                check = false;
            }
            else if (Request.Form["name"].Length > 50)
            {
                TempData["err_group_name"] = "Tên nhóm tối đa 50 ký tự";
                check = false;
            }
            return check;
        }
    }
}