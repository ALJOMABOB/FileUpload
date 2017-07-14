using FileDB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileDB.Controllers
{
    public class SupportController : Controller
    {
        private SupportContext db = new SupportContext();

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Support support)
        {
            if (ModelState.IsValid)
            {
                List<FileDetail> fileDetails = new List<FileDetail>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        FileDetail fileDetail = new FileDetail()
                        {
                            FileName = fileName,
                            Extension = Path.GetExtension(fileName),
                            Id = Guid.NewGuid()
                        };
                        fileDetails.Add(fileDetail);

                        var path = Path.Combine(Server.MapPath("~/App_Data/Upload/"), fileDetail.Id + fileDetail.Extension);
                        file.SaveAs(path);
                    }
                }

                support.FileDetails = fileDetails;
                db.Supports.Add(support);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(support);
        }
    }
}