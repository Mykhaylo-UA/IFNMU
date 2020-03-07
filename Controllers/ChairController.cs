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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace IFNMUSiteCore.Controllers
{
    [Authorize(Roles = "moderator")]
    public class ChairController: Controller
    {
        private LessonContext db; // контекст БД
        IWebHostEnvironment _appEnvironment;
        UserManager<User> _users;

        public ChairController(LessonContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager) // конструктор
        {
            db = context;
            _appEnvironment = appEnvironment;
            _users = userManager;
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult AddChair()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult AddChair(Chair chair)
        {
            try
            {
                db.Chairs.Add(chair);
                db.SaveChanges();
                return View();
            }
            catch (Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }
        [HttpGet]
        public ActionResult Chair(int id = 0)
        {
            /*var user = await GetCurrentUserAsync();
            var userRoles = await _users.GetRolesAsync(user);
            if (userRoles.Contains("admin"))
            {
                ViewBag.Host = HttpContext.Request.Host.ToString();
                return View(db.Chairs.Include(a => a.Folders).Include(b => b.Files).Include(c => c.Advertisements).Include(d => d.Graphics).Single(c => c.Id == id));
            }
            else if (id != user.ChairId)
            {
                return RedirectToAction("Chair", new { id = user.ChairId });
            }*/
            ViewBag.Host = HttpContext.Request.Host.ToString();
            return View(db.Chairs.Include(a => a.Folders).Include(b => b.Files).Include(c => c.Advertisements).Include(d => d.Graphics).Single(c => c.Id == id));
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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
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
            catch (Exception e)
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
                    return Redirect("~/Admin/Chair/" + advertisement.ChairId);
                }
                else return BadRequest();
            }
            catch (Exception e)
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
        [HttpGet]
        public async Task<ActionResult> LoadChairDoc(int? id)
        {
            Chair chair = db.Chairs.FirstOrDefault(c => c.Id == id);
            await db.Folders.LoadAsync();
            await db.Files.LoadAsync();

            List<Folder> folders1 = new List<Folder>();
            string htmlString = "<option value='null' selected='selected'>Не вибрано</option>";

            foreach(var a in chair.Files)
            {
                htmlString += $"<option value='{a.Id}'> {a.Name} </option>";
            }     

            ReturnString(chair.Folders, ref htmlString, folders1);

            return PartialView("LoadChairDoc", htmlString);
        }
        [NonAction]
        public static void ReturnString(List<Folder> folders, ref string str, List<Folder> folderUp)
        {
            if (folders != null)
            {
                foreach (var folder in folders)
                {
                    foreach (Models.File file in folder.Files)
                    {
                        string s = String.Empty;
                        foreach (var fl in folderUp)
                        {
                            s += $"{fl.Name} -> ";
                        }
                        str += $"<option value='{file.Id}'>{s} {folder.Name} ->  {file.Name}</option>";

                        if (folder.Folders != null)
                        {
                            folderUp.Add(folder);
                            ReturnString(folder.Folders, ref str, folderUp);
                        }
                    }
                }
            }
        }
        [NonAction]
        private Task<User> GetCurrentUserAsync() => _users.GetUserAsync(HttpContext.User);
    }
}