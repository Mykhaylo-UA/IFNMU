using System.Linq;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IFNMUSiteCore.Models;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace IFNMUSiteCore.Controllers
{
    public class HomeController : Controller
    {
        private LessonContext db; // контекст БД

        public HomeController(LessonContext context) // конструктор
        {
            db = context;
        }

        // Головна сторінка
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var questions = await db.Questions.Include(p => p.Answers).ToListAsync();
            if (questions != null)
            {
                foreach (Question question in questions)
                {
                    Random rand = new Random();
                    for (int i = question.Answers.Count - 1; i >= 1; i--)
                    {
                        int j = rand.Next(i + 1);

                        Answer tmp = question.Answers[j];
                        question.Answers[j] = question.Answers[i];
                        question.Answers[i] = tmp;
                    }
                }
            }
            ViewBag.Questions = questions;

            return View();
        }

        // Пошук розкладу
        [HttpPost]
        public async Task<IActionResult> SearchSchedule(byte courses, string group)
        {
            try
            {
                List<Week> week = new List<Week>();
                string c, g;
                if (courses != 0 || group != null)
                {
                    HttpContext.Response.Cookies.Append("Course", courses.ToString(), new Microsoft.AspNetCore.Http.CookieOptions() { IsEssential = true, Expires = DateTime.Now.AddMonths(1) });
                    HttpContext.Response.Cookies.Append("Group", group, new Microsoft.AspNetCore.Http.CookieOptions() { IsEssential = true, Expires = DateTime.Now.AddMonths(1) });
                    week = await db.Weeks.Where(w => w.Course == courses && w.Group == group).Include(day => day.ScheduleDays).ThenInclude(les => les.Lessons).ToListAsync();
                    ViewBag.Course = courses;
                    ViewBag.Group = group;
                    if (week[0].From == null) return View(week);
                    else
                    {
                        DateTime thisDay = DateTime.Now;
                        foreach (var w in week)
                        {
                            if (thisDay >= Convert.ToDateTime(w.From).AddDays(-2) && thisDay <= Convert.ToDateTime(w.To).AddDays(1))
                            {
                                ViewBag.Weeks = week;
                                return View("ScheduleBig", await db.Weeks.FindAsync(w.Id));
                            }
                        }
                        return View("ScheduleBig");
                    }
                }
                else
                {
                    c = HttpContext.Request.Cookies["Course"];
                    g = HttpContext.Request.Cookies["Group"];
                    week = await db.Weeks.Where(w => w.Course == Convert.ToByte(c) && w.Group == g).Include(day => day.ScheduleDays).ThenInclude(les => les.Lessons).ToListAsync();
                    ViewBag.Course = c;
                    ViewBag.Group = g;
                    if (week[0].From == null) return View(week);
                    else
                    {
                        DateTime thisDay = DateTime.Today;
                        foreach (var w in week)
                        {
                            if (thisDay >= Convert.ToDateTime(w.From).AddDays(-2) && thisDay <= Convert.ToDateTime(w.To).AddDays(1))
                            {
                                ViewBag.Weeks = week;
                                return View("ScheduleBig", await db.Weeks.FindAsync(w.Id));
                            }
                        }
                        return View("ScheduleBig");
                    }
                }
            }
            catch(Exception exp)
            {
                return View("Warning",exp);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> ScheduleBig(int WeekId)
        {
            Week week = await db.Weeks.Include(day => day.ScheduleDays).ThenInclude(les => les.Lessons).FirstOrDefaultAsync(w => w.Id == WeekId);
            return View("ScheduleBigResult", week);
        }

        // Налаштування
        [Route("properties")]
        public ActionResult Properties()
        {
            return View();
        }

        // Сторінка "Про нас"
        [Route("about")]
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Documents(int id)
        {
            var lesson = db.Lessons.Include(l => l.ThematicPlan).Include(l=> l.MethodicalRecomendation).SingleOrDefault(l => l.Id == id);
            if (lesson.ThematicPlan.Id == -1 && lesson.MethodicalRecomendation.Id == -1) return View("NoThematicPlan");
            ViewBag.Domen = Request.Host;
            return View(lesson);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
