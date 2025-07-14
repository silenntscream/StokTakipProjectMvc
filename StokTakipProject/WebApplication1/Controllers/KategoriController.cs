using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.Entity;
using PagedList;
using PagedList.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class KategoriController : Controller
    {
        // GET: Kategori
        StokTakipEntities2 db = new StokTakipEntities2();
        public ActionResult Index(string p, int sayfa = 1)
        {
            var ktgr = from m in db.Tbl_Kategoriler select m;

            if (!string.IsNullOrEmpty(p))
            {
                ktgr = ktgr.Where(m => m.KategoriAd.Contains(p));
            }

            var pagedList = ktgr.OrderBy(m => m.KategoriId).ToPagedList(sayfa, 10); // Sayfalama uygulanıyor

            return View(pagedList);
        }

        public ActionResult Sil(int id) 
        {
            var kategori = db.Tbl_Kategoriler.Find(id);
            db.Tbl_Kategoriler.Remove(kategori);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult YeniEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniEkle(Tbl_Kategoriler p1)
        {
            if (ModelState.IsValid)
            {
                // Aynı isimde kategori var mı kontrol et
                bool kategoriVarMi = db.Tbl_Kategoriler.Any(k => k.KategoriAd == p1.KategoriAd);

                if (kategoriVarMi)
                {
                    ModelState.AddModelError("KategoriAd", "Aynı isimde bir kategori zaten var.");
                    return View(p1); // Formu aynı veriyle geri döndür, hata mesajı gözüksün
                }

                db.Tbl_Kategoriler.Add(p1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(p1); // Hatalıysa formu geri döndür
            }

        }
        public ActionResult KategoriGetir(int id) 
        {
            var ktgr = db.Tbl_Kategoriler.Find(id);
            return View("KategoriGetir",ktgr);
        }
        public ActionResult Guncelle(Tbl_Kategoriler p1)
        {
            var ktgr =db.Tbl_Kategoriler.Find(p1.KategoriId);
            ktgr.KategoriAd = p1.KategoriAd;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}