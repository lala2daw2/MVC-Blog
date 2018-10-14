using KuvarBlog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using KuvarBlog.Models.ViewModel;

namespace KuvarBlog.Controllers
{
    [RoutePrefix("Kullanicilar")]
    public class SecurityController : Controller
    {
        // GET: Security
        [HttpGet, Route("Register")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, Route("Register")]
        public ActionResult Create(Kullanicilar model, HttpPostedFileBase txtFoto)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                if (ModelState.IsValid)
                {
                    if (txtFoto != null)
                    {
                        WebImage img = new WebImage(txtFoto.InputStream);
                        FileInfo info = new FileInfo(txtFoto.FileName);
                        string newFoto = Guid.NewGuid().ToString() + info.Extension;
                        img.Resize(300, 150);
                        img.Save("~/KullanıcıPP/" + newFoto);
                        model.Foto = "/KullanıcıPP/" + newFoto;
                    }
                    db.Kullanicilars.Add(model);
                    model.YetkiID = 1;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(model);
                }
            }


        }
        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost, Route("Login")]
        public ActionResult Login(Kullanicilar model)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var login = db.Kullanicilars.Where(x => x.KullaniciAdi == model.KullaniciAdi && x.Sifre == model.Sifre).FirstOrDefault();
                if (ModelState.IsValid && login != null)
                {
                    FormsAuthentication.SetAuthCookie(login.KullaniciAdi, false);
                    Session["UserID"] = login.ID;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(model);
                }
            }
        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Create");
        }
        [HttpGet]
        [Route("Edit/{name}/{ID}")]
        public ActionResult Update(int ID, string name)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var model = db.Kullanicilars.Where(x => x.ID == ID).SingleOrDefault();
                if (model == null)
                    return HttpNotFound();
                return View(model);
            }
        }
        [HttpPost]
        [Route("Edit/{name}/{ID}")]
        public ActionResult Update(int ID, Kullanicilar model, HttpPostedFileBase txtFoto, string name)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                var user = db.Kullanicilars.Where(x => x.ID == ID).SingleOrDefault();
                if (user == null)
                    return HttpNotFound();
                if (txtFoto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(user.Foto)))
                        System.IO.File.Delete(Server.MapPath(user.Foto));
                    WebImage img = new WebImage(txtFoto.InputStream);
                    FileInfo info = new FileInfo(txtFoto.FileName);
                    string newFoto = Guid.NewGuid().ToString() + info.Extension;
                    img.Resize(300, 150);
                    img.Save("~/KullanıcıPP/" + newFoto);
                    user.Foto = "/KullanıcıPP/" + newFoto;
                }
                user.Adsoyad = model.Adsoyad;
                user.KullaniciAdi = model.KullaniciAdi;
                user.Mail = model.Mail;
                user.Sifre = model.Sifre;
                db.SaveChanges();
                return RedirectToAction("Profil", "Home", new { id = user.KullaniciAdi });
            }
        }
        [Route("{name}")]
        public ActionResult Detail(int ID, string name)
        {
            using (sbayDBEntities db = new sbayDBEntities())
            {
                KullaniciMakaleVM kullanici = new KullaniciMakaleVM()
                {
                    Kullanici = db.Kullanicilars.Where(x => x.ID == ID).SingleOrDefault(),
                    Makale = db.Makalelers.Where(x => x.KullaniciID == ID).ToList()
                };
                //var model = db.Kullanicilars.Where(x=>x.ID==ID).SingleOrDefault();
                if (kullanici.Kullanici == null)
                    return HttpNotFound();
                return View(kullanici);
            }
        }

    }
}