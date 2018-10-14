using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KuvarBlog.Models;
using System.Data.Entity;

namespace KuvarBlog.Areas.Admin.Controllers
{
    [Authorize(Roles = "2"),RouteArea("Admin")]
    public class YorumController : Controller
    {
        // GET: Admin/Yorum
        [Route("Yorumlar")]
        public ActionResult List()
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var model = db.Yorumlars.Include(x=>x.Kullanicilar).Include(x=>x.Makaleler).ToList();
                return View(model);
            }
        }
        public ActionResult Delete(int ID)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var model = db.Yorumlars.Where(x => x.ID == ID).SingleOrDefault();
                if (model == null)
                    return HttpNotFound();
                db.Yorumlars.Remove(model);
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }
    }
}