using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.Entity;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class MusteriController : Controller
    {
        // GET: Musteri
        StokTakipEntities2 db = new StokTakipEntities2();
        public ActionResult Index(string p)
        {
            var mstr = from m in db.Tbl_Musteriler select m;
            if (!string.IsNullOrEmpty(p))
            {
                mstr = mstr.Where(m => m.MusteriAd.Contains(p) ||m.MusteriSoyad.Contains(p));
            }
            return View(mstr.ToList());
           /* var mstr = db.Tbl_Musteriler.ToList();
            return View(mstr);*/
        }

        public ActionResult Sil(int id)
        {
            var mstr = db.Tbl_Musteriler.Find(id);
            db.Tbl_Musteriler.Remove(mstr);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult YeniEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniEkle(Tbl_Musteriler p1)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Musteriler.Add(p1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }
        public ActionResult MusteriGetir(int id)
        {
            var mstr = db.Tbl_Musteriler.Find(id);
            return View("MusteriGetir", mstr);
        }
        public ActionResult Guncelle(Tbl_Musteriler p1)
        {
            var mstr = db.Tbl_Musteriler.Find(p1.MusteriId);
            mstr.MusteriAd = p1.MusteriAd;
            mstr.MusteriSoyad = p1.MusteriAd;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}