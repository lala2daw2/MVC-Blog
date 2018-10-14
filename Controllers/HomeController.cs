using KuvarBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using KuvarBlog.Models.ViewModel;
using PagedList;
using PagedList.Mvc;



namespace KuvarBlog.Controllers
{
    [RoutePrefix("SBAYBlog")]
    public class HomeController : Controller
    {
        // GET: Home
        [Route]
        public ActionResult Index(int? sayfaNo)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                int _sayfaNo = sayfaNo ?? 1;
                var model = db.Makalelers.Include(x => x.Kullanicilar).OrderByDescending(x => x.ID).ToPagedList(_sayfaNo, 4);
                return View(model);
            }
        }
        public ActionResult GetCategory()
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                return View(db.Kategorilers.Include(x => x.Makalelers).ToList());
            }
        }
        [Route("~/Makale/{title}")]
        public ActionResult Detail(int ID,string title)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                MakaleVM makale = new MakaleVM()
                {
                    Makale = db.Makalelers.Include(x => x.Kullanicilar).SingleOrDefault(x => x.ID == ID),
                    Yorumlar = db.Yorumlars.Include(x => x.Kullanicilar).Where(x => x.MakaleID == ID).OrderByDescending(x => x.ID).ToList()
                };
                if (makale.Makale == null)
                    return HttpNotFound();
                return View(makale);
            }
        }
        public JsonResult YorumYap(int ID, string txtYorum)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var username = User.Identity.Name;
                var loginedUser = db.Kullanicilars.SingleOrDefault(x => x.KullaniciAdi == username);
                if (loginedUser == null || txtYorum == null)
                    return Json(false, JsonRequestBehavior.AllowGet);
                Yorumlar yorum = new Yorumlar()
                {
                    KullaniciID = loginedUser.ID,
                    MakaleID = ID,
                    Tarih = DateTime.Now,
                    Yorum = txtYorum
                };
                db.Yorumlars.Add(yorum);
                db.SaveChanges();
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        //[Route("Delete/{ID}")]
        public ActionResult Delete(int ID, int makaleID)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var makale = db.Makalelers.Where(x => x.ID == makaleID).SingleOrDefault();
                var model = db.Yorumlars.Where(x => x.ID == ID).SingleOrDefault();
                if (model == null)
                    return HttpNotFound();
                db.Yorumlars.Remove(model);
                db.SaveChanges();
                return RedirectToAction("Detail", new { id = makaleID,title=makale.Baslik});
            }
        }
        [Route("Profil/{id}")]
        public ActionResult Profil(string id)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                int userid = Convert.ToInt32(Session["UserID"]);
                KullaniciMakaleVM kullanici = new KullaniciMakaleVM()
                {
                    Kullanici = db.Kullanicilars.Where(x => x.KullaniciAdi == id).SingleOrDefault(),
                    Makale = db.Makalelers.Include(x=>x.Kategoriler).Where(x=>x.KullaniciID==userid).ToList()
                };
                //var model = db.Kullanicilars.Where(x => x.KullaniciAdi == id).SingleOrDefault();
                if (kullanici.Kullanici == null)
                    return HttpNotFound();
                return View(kullanici);
            }
        }
        [Route("~/Kategori/{name}")]
        public ActionResult MakaleGetir (string name)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var kategori = db.Kategorilers.Where(x=>x.KategoriAd==name).SingleOrDefault();
                var model = db.Makalelers.Include(x=>x.Kullanicilar).Where(x => x.KategoriID == kategori.ID).ToList();
                if (model == null)
                    return HttpNotFound();
                return View(model);
            }
        }
        //SBAYBLOG/Hakkimizda
        [Route("~/Hakkimda")]
        public ActionResult Hakkimizda()
        {
            return View();
        }
        [Route("~/İletisim")]
        public ActionResult İletisim()
        {
            return View();
        }
        public ActionResult GetComments()
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var model = db.Yorumlars.Include(x=>x.Kullanicilar).OrderByDescending(x => x.ID).Take(4).ToList();
                return View(model);
            }
        }
        [Route("Sonuc/")]
        public ActionResult Search(string txtSearch)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var model = db.Makalelers.Include(x=>x.Kullanicilar).Where(x => x.Baslik.Contains(txtSearch)).ToList();
                return View(model);
            }
        }
       
      
    }
}