using Calcom.Domain;
using Calcom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalCom.Web.Controllers
{
    [Authorize]
    public class GenelController : Controller
    {
        //
        // GET: /Genel/
        public ActionResult LabPanel()
        {
            return View();
        }
        public ActionResult Haberler()
        {
            return View();
        }
        public ActionResult Protokoller()
        {
            return View();
        }
        public ActionResult Standardlar()
        {
            return View();
        }
        public ActionResult Bildirimler()
        {
            return View();
        }


        public ActionResult Anasayfa()
        {
            return View();
        }
        public ActionResult GelismisArama()
        {
            return View();
        }

        public ActionResult AdminPanel()
        {
            return View();
        }

        public ActionResult DosyaYukle()
        {
            //dosyalar tablosundan isimler alınmalı ve gönderilmeli liste şeklinde. Aynı zamanda path de gönderilmeli, her isim path'a link olmalı basınca indirmeli
            Repository<Dosyalar> RepDos = new Repository<Dosyalar>(new CalComEntities());
            return View(RepDos.GetAll().ToList());
        }
        [HttpPost]
        public ActionResult UploadRehber(HttpPostedFileBase file)
        {

            string directory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\RehberDokumanlar";
            Repository<Dosyalar> RepDos = new Repository<Dosyalar>(new CalComEntities());
            if (file != null && file.ContentLength > 0)
            {

                var fileName = Path.GetFileName(file.FileName);
                string fullPath = Path.Combine(directory, fileName);
                file.SaveAs(fullPath);
                Dosyalar dos = new Dosyalar { İsim = fileName, path = fullPath, Tur = "R", };
                RepDos.Add(dos);
            }
            return RedirectToAction("DosyaYukle");
        }
        [HttpPost]
        public ActionResult UploadProtokol(HttpPostedFileBase file)
        {
            //string directory = @"~/App_Data/Protokoller/";
            string directory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\Protokoller";
            Repository<Dosyalar> RepDos = new Repository<Dosyalar>(new CalComEntities());
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                string fullPath = Path.Combine(directory, fileName);
                file.SaveAs(fullPath);
                Dosyalar dos = new Dosyalar { İsim = fileName, path = fullPath, Tur = "P", };
                RepDos.Insert("Id",  new object[] {
                 new SqlParameter() { ParameterName = "@İsim", Value = dos.İsim },
                  new SqlParameter() { ParameterName = "@Tur", Value = dos.Tur },
                new SqlParameter() { ParameterName = "@path", Value = dos.path } });
            }
            return RedirectToAction("DosyaYukle");
        }
        public FileResult getDocument(string path)
        {
            return File(path, "application/text");
        }
        public ActionResult Dosyalar()
        {
            return View();
        }
    }
}
