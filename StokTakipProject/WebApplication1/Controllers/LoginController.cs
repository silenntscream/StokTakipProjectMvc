using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models.Entity;
using BCrypt.Net; // BCrypt hash için

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        StokTakipEntities2 db = new StokTakipEntities2();

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(string UserName, string Password)
        {
            // Kullanıcıyı veritabanından bul
            var user = db.Tbl_Kullanıcılar.FirstOrDefault(x => x.KullanıcıAdı == UserName);

            if (user != null)
            {
                // BCrypt ile şifre kontrolü
                bool isValid = false;
                if (user.Şifre.StartsWith("$2a$") || user.Şifre.StartsWith("$2b$"))
                {
                    isValid = BCrypt.Net.BCrypt.Verify(Password, user.Şifre);
                }

                if (isValid)
                {
                    // Rol bilgisini çek
                    string userRole = db.Tbl_Roller.Where(r => r.RoleID == user.RoleID)
                                                   .Select(r => r.RoleAdi)
                                                   .FirstOrDefault();

                    if (string.IsNullOrEmpty(userRole))
                    {
                        ViewBag.Eror = "Kullanıcı rolü bulunamadı!";
                        return View();
                    }

                    // 👇 Role bilgisini Cookie’ye yaz
                    var ticket = new FormsAuthenticationTicket(
                        1,
                        user.KullanıcıAdı,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30),
                        false,
                        userRole, // 👈 Rolü UserData’ya yaz
                        FormsAuthentication.FormsCookiePath
                    );

                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                    {
                        HttpOnly = true,
                        Secure = Request.IsSecureConnection
                    };

                    Response.Cookies.Add(authCookie);

                    // 👇 Ayrıca Role bilgisini Session’a da yaz (Navbar için)
                    Session["UserRole"] = userRole;

                    return RedirectToAction("Index", "Kategori");
                }
            }

            ViewBag.Eror = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear(); // 👈 Rol bilgisini temizle
            return RedirectToAction("Index", "Login");
        }
    }
}

