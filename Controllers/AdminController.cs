using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IFNMUSiteCore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Web;

namespace IFNMUSiteCore.Controllers
{
    public class AdminController : Controller
    {
        private LessonContext db; // контекст БД
        IHostingEnvironment _appEnvironment;

        public AdminController(LessonContext context, IHostingEnvironment appEnvironment) // конструктор
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        // Адмін-панель
        public ActionResult Index(string s)
        {
            if (s == null || s != "qwerty")
            {
                return NotFound();
            }
            return View();
        }

        // Сторінка "Додати день до розкладу"
        [HttpGet]
        public ActionResult AddWeek(string password)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddWeek(Week w)
        {
            try
            {
                await db.Weeks.AddAsync(w);
                await db.ScheduleDays.AddRangeAsync(w.ScheduleDays);
                List<Lesson>[] les = new List<Lesson>[5];
                Lesson l = new Lesson();
                for (byte a = 0; a < 5; a++)
                {
                    les[a] = new List<Lesson>();
                    int u = HttpContext.Request.Form["ScheduleDays[" + a + "].Lessons[].LessonNumber"].Count;
                    for (int i = 0; i < HttpContext.Request.Form["ScheduleDays[" + a + "].Lessons[].LessonNumber"].Count; i++)
                    {
                        les[a].Add(new Lesson(HttpContext.Request.Form["ScheduleDays[" + a + "].Lessons[].Name"][i], Convert.ToInt32(HttpContext.Request.Form["ScheduleDays[" + a + "].Lessons[].LessonNumber"][i]), Convert.ToInt32(HttpContext.Request.Form["ScheduleDays[" + a + "].Lessons[].ThematicPlanId"][i]), Convert.ToInt32(HttpContext.Request.Form["ScheduleDays[" + a + "].Lessons[].MethodicalRecomendationId"][i])) { ScheduleDayId = w.ScheduleDays[a].Id });

                    }
                    await db.Lessons.AddRangeAsync(les[a]);
                }
                await db.SaveChangesAsync();
                return View();
            }
            catch(Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }
        // Додати урок
        public async Task<IActionResult> AddLesson(byte dayNumber, byte index)
        {
            ViewBag.ThematicPlan = await db.ThematicPlans.ToListAsync();
            ViewBag.MethodicRecomentadion = await db.MethodicalRecomendations.ToListAsync();
            return PartialView(dayNumber);
        }

        // Змінити запитання
        [HttpGet]
        public ActionResult Questions()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Questions(int from, int to, int year)
        {
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Question[]));

            Question[] questions;
            using (FileStream fs = new FileStream(_appEnvironment.WebRootPath + "/Tests/Tests_" + year + ".xml", FileMode.OpenOrCreate))
            {
                questions = (Question[])formatter.Deserialize(fs);
            }
            var q = db.Questions;
            db.Questions.RemoveRange(q);
            db.SaveChanges();

            for (int i = from; i < to; i++)
            {
                db.Questions.Add(questions[i]);
            }
            db.SaveChanges();
            return View();
        }
            catch(Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
    }
}

        [HttpGet]
        public IActionResult AddThematicPlan()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddThematicPlan(IFormFile uploadedFile)
        {
            try
            {
                if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/ThematicPlans/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                ThematicPlan file = new ThematicPlan { Name = uploadedFile.FileName, Path = path };
                db.ThematicPlans.Add(file);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
}
            catch(Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }

        [HttpGet]
        public IActionResult AddMethodic()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMethodic(IFormFile uploadedFile)
        {
            try
            {
                if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/MethodicalRecomentadion/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                MethodicalRecomendation file = new MethodicalRecomendation { Name = uploadedFile.FileName, Path = path };
                db.MethodicalRecomendations.Add(file);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
}
            catch(Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }

        [HttpGet]
        public IActionResult AddChair()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult AddChair(Chair chair)
        {
            try
            {
                db.Chairs.Add(chair);
            db.SaveChanges();
            return View();
}
            catch(Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }

        [HttpGet]
        public ActionResult Chair(int id)
        {
            ViewBag.Host = HttpContext.Request.Host.ToString();
            return View(db.Chairs.Include(a => a.Folders).Include(b => b.Files).Include(c => c.Advertisements).Include(d=>d.Graphics).Single(c => c.Id == id));
        }
        [ActionName("FolderAdd")]
        [HttpPost]
        public ActionResult Folder(Folder folder)
        {
            try
            {
                if (folder == null) return BadRequest();
            if (folder.Name.Trim(' ') == "") return BadRequest();
            ViewBag.Host = HttpContext.Request.Host.ToString();
            db.Folders.Add(folder);
            db.SaveChanges();
            if (folder.ChairId != null)
            {
                return View("ChairPartial", db.Chairs.Include(f => f.Folders).Include(fl => fl.Files).Single(a => a.Id == folder.ChairId));
            }
            else
            {
                return View("Folder", db.Folders.Include(f => f.Folders).Include(fl => fl.Files).Single(a => a.Id == folder.FolderId));
            }
}
            catch(Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }

        [HttpPost]
        public ActionResult Folder(string btn)
        {
            try
            {
                ViewBag.Host = HttpContext.Request.Host.ToString();
            if (btn.Contains("c".ToLower()))
            {
                return View("ChairPartial", db.Chairs.Include(f => f.Folders).Include(fl => fl.Files).Single(a => a.Id == Convert.ToInt32(btn.TrimStart('c'))));
            }
            else
            {
                int i = Convert.ToInt32(btn);
                return View(db.Folders.Include(a => a.Folders).Include(b => b.Files).Single(c => c.Id == i));
            }
}
            catch(Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }

        [HttpPost]
        public async Task<IActionResult> FileAdd(IFormFileCollection uploadFile, int? ChairIdd, int? FolderIdd)
        {
            try
            {
                if (uploadFile == null) return BadRequest();
            ViewBag.Host = HttpContext.Request.Host.ToString();
            foreach (var file in uploadFile)
            {
                // путь к папке Files
                string path = "/Files/" + file.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                path = "/Files/" + HttpUtility.UrlEncode(file.FileName);
                Models.File f = new Models.File { Path = path, ChairId = ChairIdd, FolderId = FolderIdd };
                if (file.FileName.Contains(".doc") || file.FileName.Contains(".docx")) { f.TypeFile = TypeFile.Word; f.Name = file.FileName.Replace(".docx", "").Replace(".doc", ""); }
                else if (file.FileName.Contains(".ppt") || file.FileName.Contains(".pptx")) { f.TypeFile = TypeFile.PowerPoint; f.Name = file.FileName.Replace(".pptx", "").Replace(".ppt", ""); }
                else if (file.FileName.Contains(".xls") || file.FileName.Contains(".xlsx")) { f.TypeFile = TypeFile.Excel; f.Name = file.FileName.Replace(".xlsx", "").Replace(".xls", ""); }
                else if (file.FileName.Contains(".pdf")) { f.TypeFile = TypeFile.PDF; f.Name = file.FileName.Replace(".pdf", ""); }
                else if (file.FileName.Contains(".djvu")) { f.TypeFile = TypeFile.DJVU; f.Name = file.FileName.Replace(".djvu", ""); }
                else if (file.FileName.Contains(".jpg") || file.FileName.Contains(".png") || file.FileName.Contains(".jpeg")) { f.TypeFile = TypeFile.Image; f.Name = file.FileName.Replace(".jpg", "").Replace(".png", "").Replace(".jpeg", ""); }
                else { f.TypeFile = TypeFile.None; f.Name = file.FileName; }
                db.Files.Add(f);
            }
            db.SaveChanges();
            if (ChairIdd != null)
            {
                return View("ChairPartial", db.Chairs.Include(f => f.Folders).Include(fl => fl.Files).Include(c => c.Advertisements).Single(a => a.Id == ChairIdd));
            }
            else
            {
                return View("Folder", db.Folders.Include(f => f.Folders).Include(fl => fl.Files).Single(a => a.Id == FolderIdd));
            }
}
            catch(Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }
        [HttpPost]
        public ActionResult AddAdvertisement(Advertisement advertisement)
        {
            try
            {
                if (advertisement == null) return BadRequest();
                if (advertisement.Text.Trim(' ') == "") return BadRequest();
                db.Advertisements.Add(advertisement);
                db.SaveChanges();
                return Redirect("~/Admin/Chair/" + advertisement.ChairId);
            }
            catch(Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }

        [HttpGet]
        public ActionResult UpdateFolderAjax(int id)
        {
            return PartialView(db.Folders.Find(id));
        }

        [HttpPost]
        public ActionResult UpdateFolder(Folder folder)
        {
            try
            {
                if (folder != null)
            {
                db.Folders.Update(folder);
                db.SaveChanges();
                if (folder.ChairId != null)
                {
                    return View("ChairPartial", db.Chairs.Include(l => l.Folders).Include(fl => fl.Files).Include(c => c.Advertisements).Single(a => a.Id == folder.ChairId));
                }
                else
                {
                    return View("Folder", db.Folders.Include(l => l.Folders).Include(fl => fl.Files).Single(a => a.Id == folder.FolderId));
                }
            }
            else return BadRequest();
}
            catch(Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }
        [HttpPost]
        public ActionResult DeleteFolder(Folder folder)
        {
            try
            {
                if (folder != null)
            {
                db.Folders.Remove(folder);
                db.SaveChanges();
                if (folder.ChairId != null)
                {
                    return View("ChairPartial", db.Chairs.Include(l => l.Folders).Include(fl => fl.Files).Include(c => c.Advertisements).Single(a => a.Id == folder.ChairId));
                }
                else
                {
                    return View("Folder", db.Folders.Include(l => l.Folders).Include(fl => fl.Files).Single(a => a.Id == folder.FolderId));
                }
            }
            else return BadRequest();
}
            catch(Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }

        [HttpGet]
        public ActionResult UpdateFolderAjaxFile(int id)
        {
            return PartialView(db.Files.Find(id));
        }

        [HttpPost]
        public ActionResult UpdateFile(Models.File file)
        {
            try
            {
                if (file != null)
            {
                db.Files.Update(file);
                db.SaveChanges();
                if (file.ChairId != null)
                {
                    return View("ChairPartial", db.Chairs.Include(l => l.Folders).Include(fl => fl.Files).Include(c => c.Advertisements).Single(a => a.Id == file.ChairId));
                }
                else
                {
                    return View("Folder", db.Folders.Include(l => l.Folders).Include(fl => fl.Files).Single(a => a.Id == file.FolderId));
                }
            }
            else return BadRequest();
}
            catch(Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }
        [HttpPost]
        public IActionResult DeleteFile(Models.File file)
        {
            try
            {
                if (file != null)
            {
                db.Files.Remove(file);
                db.SaveChanges();
                string path = _appEnvironment.WebRootPath + HttpUtility.UrlDecode(file.Path);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                if (file.ChairId != null)
                {
                    return View("ChairPartial", db.Chairs.Include(l => l.Folders).Include(fl => fl.Files).Include(c => c.Advertisements).Single(a => a.Id == file.ChairId));
                }
                else
                {
                    return View("Folder", db.Folders.Include(l => l.Folders).Include(fl => fl.Files).Single(a => a.Id == file.FolderId));
                }
            }
            else return BadRequest();
}
            catch(Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }
        [HttpGet]
        public ActionResult UpdateAjaxAdvertisement(int id)
        {
            return PartialView(db.Advertisements.Find(id));
        }
        [HttpPost]
        public ActionResult UpdateAdvertisement(Advertisement advertisement)
        {
            try
            {
                if (advertisement != null)
            {
                db.Advertisements.Update(advertisement);
                db.SaveChanges();
                return Redirect("~/Admin/Chair/" + advertisement.ChairId);
            }
            else return BadRequest();
}
            catch(Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }
        [HttpPost]
        public ActionResult DeleteAdvertisement(Advertisement advertisement)
        {
            try {
            if (advertisement != null)
            {
                db.Advertisements.Remove(advertisement);
                db.SaveChanges();
                return Redirect("~/Admin/Chair/"+advertisement.ChairId);
            }
            else return BadRequest();
}
            catch(Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }

        [HttpPost]
        public ActionResult AddGraphic(Graphic graphic)
        {
            try
            {
                if (graphic == null) return BadRequest();
                if (graphic.Text.Trim(' ') == "") return BadRequest();
                db.Graphics.Add(graphic);
                db.SaveChanges();
                return Redirect("~/Admin/Chair/" + graphic.ChairId);
            }
            catch (Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }
        [HttpGet]
        public ActionResult UpdateAjaxGraphic(int id)
        {
            return PartialView(db.Graphics.Find(id));
        }
        [HttpPost]
        public ActionResult UpdateGraphic(Graphic graphic)
        {
            try
            {
                if (graphic != null)
                {
                    db.Graphics.Update(graphic);
                    db.SaveChanges();
                    return Redirect("~/Admin/Chair/" + graphic.ChairId);
                }
                else return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }
        [HttpPost]
        public ActionResult DeleteGraphic(Graphic graphic)
        {
            try
            {
                if (graphic != null)
                {
                    db.Graphics.Remove(graphic);
                    db.SaveChanges();
                    return Redirect("~/Admin/Chair/" + graphic.ChairId);
                }
                else return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }
    }
}