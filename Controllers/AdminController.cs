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
            if(s == null || s != "qwerty")
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
                    les[a].Add(new Lesson(HttpContext.Request.Form["ScheduleDays[" + a + "].Lessons[].Name"][i], Convert.ToInt32(HttpContext.Request.Form["ScheduleDays[" + a + "].Lessons[].LessonNumber"][i]), Convert.ToInt32(HttpContext.Request.Form["ScheduleDays[" + a + "].Lessons[].ThematicPlanId"][i]), Convert.ToInt32(HttpContext.Request.Form["ScheduleDays[" + a + "].Lessons[].MethodicalRecomendationId"][i])) { ScheduleDayId=w.ScheduleDays[a].Id});
                   
                }
                await db.Lessons.AddRangeAsync(les[a]);
            }
            await db.SaveChangesAsync();
            return View();
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
        public IActionResult Questions(int from,int to, int year)
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

        [HttpGet]
        public IActionResult AddThematicPlan()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddThematicPlan(IFormFile uploadedFile)
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
                ThematicPlan file = new ThematicPlan {Name=uploadedFile.FileName, Path = path };
                db.ThematicPlans.Add(file);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddMethodic()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMethodic(IFormFile uploadedFile)
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
    }
}