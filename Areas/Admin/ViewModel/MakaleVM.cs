using KuvarBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KuvarBlog.Areas.Admin.ViewModel
{
    public class MakaleVM
    {
        public IEnumerable<Kategoriler> Kategorilers { get; set; }
        public Makaleler Makale { get; set; }

    }
}