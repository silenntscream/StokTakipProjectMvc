using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models.Entity;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")] // 👈 Buraya ekledik
    public class RaporlamaController : Controller
    {
        private StokTakipEntities2 db = new StokTakipEntities2();

        public ActionResult Index(DateTime? baslangicTarihi, DateTime? bitisTarihi)
        {
            

            List<Tbl_Satislar> satislar;

            if (baslangicTarihi.HasValue && bitisTarihi.HasValue)
            {
                DateTime basTarih = baslangicTarihi.Value.Date;
                DateTime bitTarih = bitisTarihi.Value.Date.AddDays(1).AddSeconds(-1);

                satislar = db.Tbl_Satislar
                    .Where(s => s.Tarih >= basTarih && s.Tarih <= bitTarih)
                    .OrderByDescending(s => s.Tarih)
                    .ToList();
            }
            else
            {
                satislar = db.Tbl_Satislar
                    .OrderByDescending(s => s.Tarih)
                    .Take(10)
                    .ToList();
            }

            return View(satislar);
        }
    }
}
