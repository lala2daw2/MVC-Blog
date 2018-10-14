using KuvarBlog.Areas.Admin.ViewModel;
using KuvarBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KuvarBlog.Areas.Admin.Controllers
{
    [Authorize(Roles ="2"),RouteArea("Admin")]
    public class AnasayfaController : Controller
    {
        // GET: Admin/Home
        [Route("Kullanicilar")]
        public ActionResult Index()
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var model = db.Kullanicilars.ToList();
                return View(model);
            }
        }
        [HttpGet]
        [Route("Yetkiver/{name}/{ID}")]
        public ActionResult Yetki(int ID,string name)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                UserRoleVM model = new UserRoleVM()
                {
                    Kullanici = db.Kullanicilars.Where(x => x.ID == ID).SingleOrDefault(),
                    Yetkiler = db.Yetkilers.ToList()
                };
                //var model = db.Kullanicilars.Where(x => x.ID == ID).SingleOrDefault();
                if (model.Kullanici == null)
                    return HttpNotFound();
                return View(model);
            }
        }
        [HttpPost]
        [Route("~/Yetkiver/{name}/{ID}")]
        public ActionResult Yetki(int ID,UserRoleVM model,string name)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var kullanici = db.Kullanicilars.Where(x => x.ID == ID).SingleOrDefault();
                if (kullanici == null)
                    return HttpNotFound();
                kullanici.YetkiID = model.Kullanici.YetkiID;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}