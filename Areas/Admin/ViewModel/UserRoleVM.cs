using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KuvarBlog.Models;

namespace KuvarBlog.Areas.Admin.ViewModel
{
    public class UserRoleVM
    {
        public Kullanicilar Kullanici { get; set; }
        public IEnumerable<Yetkiler> Yetkiler { get; set; }

    }
}