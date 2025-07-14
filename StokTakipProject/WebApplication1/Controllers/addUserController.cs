using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.Entity;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity.Validation;


namespace WebApplication1.Controllers
{
    public class addUserController : Controller
    {
        private StokTakipEntities2 db = new StokTakipEntities2();

        public ActionResult Index()
        {
            if (Session["UserRole"]?.ToString() != "Admin")
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Roles = new SelectList(db.Tbl_Roller.ToList(), "RoleID", "RoleAdi");
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(string username, string password, string eposta, int RoleID)
        {
            if (Session["UserRole"]?.ToString() != "Admin")
            {
                return RedirectToAction("Index", "addUser");
            }

            var user = new Tbl_Kullanıcılar
            {
                KullanıcıAdı = username,
                Şifre = password,
                EPosta = eposta,
                RoleID = RoleID
            };

            try
            {
                db.Tbl_Kullanıcılar.Add(user);
                db.SaveChanges();
                ViewBag.Message = "Kullanıcı başarıyla eklendi.";
            }
            catch (DbEntityValidationException ex)
            {
                var errors = "";
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errors += $"Alan: {validationError.PropertyName} - Hata: {validationError.ErrorMessage}<br/>";
                    }
                }
                ViewBag.Message = errors; // Hataları View'da göster
            }

            ViewBag.Roles = new SelectList(db.Tbl_Roller.ToList(), "RoleID", "RoleAdi");
            return RedirectToAction("../Kategori/Index");
        }


    }
}

