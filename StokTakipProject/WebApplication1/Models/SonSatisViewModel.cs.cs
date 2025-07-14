
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WebApplication1.Models
{
    public class SonSatisViewModel
    {
        public string UrunAdi { get; set; }
        public string MusteriAdi { get; set; }
        public decimal Tutar { get; set; }
        public DateTime Tarih { get; set; }
    }
}