using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KuvarBlog.Models.ViewModel
{
    public class KullaniciMakaleVM
    {
        public Kullanicilar Kullanici { get; set; }
        public IEnumerable<Makaleler> Makale { get; set; }
    }
}