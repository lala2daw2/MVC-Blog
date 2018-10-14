using KuvarBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KuvarBlog.Areas.Admin.Controllers
{
    [Authorize(Roles ="2"),RouteArea("Admin")]
    public class KategoriController : Controller
    {
        // GET: Admin/Kategori
        [Route("Kategoriler")]
        public ActionResult List()
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var model = db.Kategorilers.ToList();
                return View(model);
            }
        }
        [HttpGet,Route("YeniKategori")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost,Route("YeniKategori")]
        public ActionResult Create(Kategoriler model)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                if (!ModelState.IsValid)
                    return View(model);
                else
                {
                    db.Kategorilers.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("List");
                }

            }
        }
        [HttpGet,Route("Edit/{name}/{ID}")]
        public ActionResult Edit(int ID,string name)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var model = db.Kategorilers.Find(ID);
                if (model == null)
                    return HttpNotFound();
                return View(model);
            }
        }
        [HttpPost, Route("Edit/{name}/{ID}")]
        public ActionResult Edit(int ID, Kategoriler model,string name)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                if (!ModelState.IsValid)
                    return View(model);
                else
                {
                    var result = db.Kategorilers.Find(ID);
                    if (result == null)
                        return HttpNotFound();
                    result.KategoriAd = model.KategoriAd;
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
            }
        }
        public ActionResult Delete(int ID)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var model = db.Kategorilers.Find(ID);
                if (model == null)
                    return HttpNotFound();
                db.Kategorilers.Remove(model);
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }
    }
}