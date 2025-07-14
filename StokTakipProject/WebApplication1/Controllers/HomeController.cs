using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Entity;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        StokTakipEntities2 db = new StokTakipEntities2();
        public ActionResult Index()
        {
            ViewBag.UrunSayisi = db.Tbl_Urunler.Count();
            ViewBag.MusteriSayisi = db.Tbl_Musteriler.Count();
            ViewBag.BugunkuSatis = db.Tbl_Satislar
                .Where(s => DbFunctions.TruncateTime(s.Tarih) == DbFunctions.TruncateTime(DateTime.Now))
                .Sum(s => (decimal?)s.Fiyat) ?? 0;
            ViewBag.EnCokKategori = db.Tbl_Kategoriler
                .OrderByDescending(k => k.Tbl_Urunler.Count)
                .Select(k => k.KategoriAd)
                .FirstOrDefault();

            var sonSatislar = db.Tbl_Satislar
                .OrderByDescending(s => s.Tarih)
                .Take(5)
                .Select(s => new SonSatisViewModel
                {
                    UrunAdi = s.Tbl_Urunler.UrunAd,
                    MusteriAdi = s.Tbl_Musteriler.MusteriAd + " " + s.Tbl_Musteriler.MusteriSoyad,
                    Tutar = (decimal)s.Fiyat,
                    Tarih = (DateTime)s.Tarih
                }).ToList();

            return View(sonSatislar);
        }


    }
}