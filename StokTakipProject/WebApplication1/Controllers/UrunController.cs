using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models.Entity;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class UrunController : Controller
    {
        StokTakipEntities2 db = new StokTakipEntities2();

        public ActionResult Index()
        {
            var urnler = db.Tbl_Urunler.ToList();
            return View(urnler);
        }

        public ActionResult Sil(int id)
        {
            var urnler = db.Tbl_Urunler.Find(id);
            db.Tbl_Urunler.Remove(urnler);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult YeniEkle()
        {
            KategoriListesi(); // ViewBag set ediliyor
            return View();
        }

        [HttpPost]
        public ActionResult YeniEkle(Tbl_Urunler p1)
        {
            var urunvarmi = db.Tbl_Urunler.FirstOrDefault(x => x.UrunAd == p1.UrunAd);
            if (urunvarmi != null) 
            {
                urunvarmi.Stok += p1.Stok;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else 
            {
                db.Tbl_Urunler.Add(p1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            KategoriListesi(); // hata varsa tekrar kategori listesi gönderilmeli
            return View(p1);
        }

        public ActionResult UrunGetir(int id)
        {
            var urnler = db.Tbl_Urunler.Find(id);
            KategoriListesi(); // güncelleme ekranında da kategori dropdown gerekiyor
            return View("UrunGetir", urnler);
        }

        public ActionResult Guncelle(Tbl_Urunler p1)
        {
            var urnler = db.Tbl_Urunler.Find(p1.UrunId);
            urnler.UrunAd = p1.UrunAd;
            urnler.UrunKategori = p1.UrunKategori;
            urnler.Fiyat = p1.Fiyat;
            urnler.Stok = p1.Stok;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Yardımcı metod: Kategori listesini ViewBag'e at
        private void KategoriListesi()
        {
            var kategoriler = db.Tbl_Kategoriler.ToList();
            ViewBag.Kategoriler = new SelectList(kategoriler, "KategoriId", "KategoriAd");
        }
    }
}
