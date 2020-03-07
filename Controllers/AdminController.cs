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
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private LessonContext db; // контекст БД
        IWebHostEnvironment _appEnvironment;
        UserManager<User> _users;

        public AdminController(LessonContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager) // конструктор
        {
            db = context;
            _appEnvironment = appEnvironment;
            _users = userManager;
        }

        [Authorize(Roles="admin")]
        // Адмін-панель
        public ActionResult Index()
        {
            return View();
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
            catch (Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }

        [HttpGet]
        public IActionResult EditMRTP()
        {
            return View(db.Chairs.ToList());
        }
        [HttpPost]
        public IActionResult EditMRTP(int idChair, byte course = 0)
        {
            ViewBag.Course = course;
            return View("EditMRTP2", db.Chairs.Include(c => c.Files).Include(c => c.Folders).FirstOrDefault(i => i.Id == idChair));
        }

        [HttpGet]
        public IActionResult EditMRTP2(int idChair, byte course = 0)
        {
            ViewBag.Course = course;
            return View(db.Chairs.Include(c => c.Files).Include(c => c.Folders).FirstOrDefault(i => i.Id == idChair));
        }
        [HttpPost]
        public IActionResult EditMRTP2(string nameLesson, int numberCourse, Faculty faculty, int? fileMR, int? fileTP)
        {
            IEnumerable<Week> week = db.Weeks.Where(p => p.Course == numberCourse && p.Faculty == faculty).Include(s => s.ScheduleDays).ThenInclude(les => les.Lessons);
            Regex regex = new Regex(nameLesson);
            MatchCollection matches;
            List<Lesson> lessons = new List<Lesson>();
            foreach (Week w in week)
            {
                foreach (ScheduleDay scheduleDay in w.ScheduleDays)
                {
                    foreach (Lesson lesson in scheduleDay.Lessons)
                    {
                        matches = regex.Matches(lesson.Name);
                        if (matches.Count > 0) lessons.Add(lesson);
                    }
                }
            }
            for (int i = 0; i < lessons.Count; i++)
            {
                lessons[i].MethodicalRecomendationId = fileMR;
                lessons[i].ThematicPlanId = fileTP;
            }
            db.Lessons.UpdateRange(lessons);
            db.SaveChanges();
            ViewBag.CountUpdate = lessons.Count();
            return RedirectToAction("EditMRTP");
        }

        [NonAction]
        private Task<User> GetCurrentUserAsync() => _users.GetUserAsync(HttpContext.User);
    }
}