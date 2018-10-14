using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KuvarBlog.Models;

namespace KuvarBlog.Models.ViewModel
{
    public class MakaleVM
    {
        public IEnumerable<Yorumlar> Yorumlar { get; set; }
        public Makaleler Makale { get; set; }
    }
}