using KuvarBlog.Areas.Admin.ViewModel;
using KuvarBlog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Data.Entity;


namespace KuvarBlog.Areas.Admin.Controllers
{
    [Authorize(Roles = "2"),RouteArea("Admin")]
    public class MakaleController : Controller
    {
        // GET: Admin/Makale
        [Route("Makaleler")]
        public ActionResult List()
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var model = db.Makalelers.Include(x => x.Kullanicilar).Include(x => x.Kategoriler).ToList();
                return View(model);
            }
        }
        [HttpGet,Route("YeniMakale")]
        public ActionResult Create()
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                MakaleVM makale = new MakaleVM()
                {
                    Kategorilers = db.Kategorilers.ToList()
                };
                return View(makale);
            }

        }
        [HttpPost,Route("YeniMakale")]
        public ActionResult Create(MakaleVM model, HttpPostedFileBase txtFoto)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                if(!ModelState.IsValid)
                {
                    MakaleVM makale = new MakaleVM()
                    {
                        Kategorilers = db.Kategorilers.ToList(),
                    };
                    return View(makale);
                }
                WebImage img = new WebImage(txtFoto.InputStream);
                FileInfo info = new FileInfo(txtFoto.FileName);
                string newFoto = Guid.NewGuid().ToString() + info.Extension;
                img.Resize(750, 300);
                img.Save("~/MakaleFoto/" + newFoto);
                model.Makale.Foto = "/MakaleFoto/" + newFoto;
                model.Makale.Tarih = DateTime.Now;
                db.Makalelers.Add(model.Makale);
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }
        [HttpGet,Route("Makale/Edit/{title}/{ID}")]
        public ActionResult Update(int ID,string title)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                MakaleVM makale = new MakaleVM()
                {
                    Kategorilers = db.Kategorilers.ToList(),
                    Makale = db.Makalelers.Find(ID)
                };
                if (makale.Makale == null)
                    return HttpNotFound();
                return View(makale);
            }
        }
        [HttpPost, Route("Makale/Edit/{title}/{ID}")]
        public ActionResult Update(int ID,MakaleVM model,HttpPostedFileBase txtFoto, string title)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                if(!ModelState.IsValid)
                {
                    MakaleVM makale = new MakaleVM()
                    {
                        Kategorilers = db.Kategorilers.ToList(),
                        Makale = db.Makalelers.Find(ID)
                    };
                    return View(makale);
                }
                var result = db.Makalelers.Where(x => x.ID == ID).SingleOrDefault();
                if (result == null)
                    return HttpNotFound();
                if (System.IO.File.Exists(Server.MapPath(result.Foto)))
                    System.IO.File.Delete(Server.MapPath(result.Foto));
                if(txtFoto!=null)
                {
                    WebImage img = new WebImage(txtFoto.InputStream);
                    FileInfo info = new FileInfo(txtFoto.FileName);
                    string newFoto = Guid.NewGuid().ToString() + info.Extension;
                    img.Resize(750, 300);
                    img.Save("~/MakaleFoto/" + newFoto);
                    result.Foto = "/MakaleFoto/" + newFoto;
                }
                result.Baslik = model.Makale.Baslik;
                result.KategoriID = model.Makale.KategoriID;
                result.İcerik = model.Makale.İcerik;
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }
        [Route("MakaleDetay/{title}/{ID}")]
        public ActionResult Detail(int ID,string title)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var model = db.Makalelers.Include(x=>x.Kategoriler).Where(x => x.ID == ID).SingleOrDefault();
                return View(model);
            }
        }
        public ActionResult Delete(int ID)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var makale = db.Makalelers.Where(x => x.ID == ID).SingleOrDefault();
                var yorums = db.Yorumlars.Where(x => x.MakaleID == ID);
                if (makale == null)
                    return HttpNotFound();
                if (yorums != null)
                    db.Yorumlars.RemoveRange(yorums);
                db.Makalelers.Remove(makale);
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }
      
    }
}