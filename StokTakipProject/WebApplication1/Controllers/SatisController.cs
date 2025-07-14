using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.Entity;

namespace WebApplication1.Controllers
{
    public class SatisController : Controller
    {
        StokTakipEntities2 db = new StokTakipEntities2();
        /*
        public ActionResult Index()
        {
            //var sts = db.Tbl_Satislar.ToList();
            //return View(sts);
        }*/
        [HttpGet]
        public ActionResult YeniSatis()
        {
            UrunListesi();
            MusteriListesi();
            return View();
        }
        [HttpPost]
        public ActionResult YeniSatis(Tbl_Satislar p)
        {
            UrunListesi();
            MusteriListesi();
            db.Tbl_Satislar.Add(p);
            db.SaveChanges();
            return View();
        }
        private void UrunListesi()
        {
            var urunler = db.Tbl_Urunler.ToList();
            ViewBag.Urunler = new SelectList(urunler, "UrunId", "UrunAd");
        }
        private void MusteriListesi()
        {
            var musteriler = db.Tbl_Musteriler.ToList();
            ViewBag.Musteriler = new SelectList(musteriler, "MusteriId", "MusteriAd","MusteriSoyad");
        }
    }
}