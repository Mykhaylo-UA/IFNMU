using System.Linq;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IFNMUSiteCore.Models;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;

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
                    week = await db.Weeks.Where(w => w.Course == courses && w.Group == group.Replace("+","").Replace("-", "").Replace("*", "")).Include(day => day.ScheduleDays).ThenInclude(les => les.Lessons).ToListAsync();
                    ViewBag.Course = courses;
                    ViewBag.Group = group;
                    if (week[0].From == null)
                    {
                        if(group.Contains("+"))
                        {
                            for (byte i=0;i<week.Count;i++)
                            {
                                for (byte a = 0; a < week[i].ScheduleDays.Count; a++)
                                {
                                    List<Lesson> wk = new List<Lesson>();
                                    wk.AddRange(week[i].ScheduleDays[a].Lessons.Where(les => les.Name.Contains("-")).ToList());
                                    wk.AddRange(week[i].ScheduleDays[a].Lessons.Where(les => les.Name.Contains("*")).ToList());
                                    foreach (var l in wk)
                                    {
                                        week[i].ScheduleDays[a].Lessons.Remove(l);
                                    }
                                }
                            }
                        }
                        if (group.Contains("-"))
                        {
                            for (byte i = 0; i < week.Count; i++)
                            {
                                for (byte a = 0; a < week[i].ScheduleDays.Count; a++)
                                {
                                    List<Lesson> wk = new List<Lesson>();
                                    wk.AddRange(week[i].ScheduleDays[a].Lessons.Where(les => les.Name.Contains("+")).ToList());
                                    wk.AddRange(week[i].ScheduleDays[a].Lessons.Where(les => les.Name.Contains("*")).ToList());
                                    foreach (var l in wk)
                                    {
                                        week[i].ScheduleDays[a].Lessons.Remove(l);
                                    }
                                }
                            }
                        }
                        if (group.Contains("*"))
                        {
                            for (byte i = 0; i < week.Count; i++)
                            {
                                for (byte a = 0; a < week[i].ScheduleDays.Count; a++)
                                {
                                    List<Lesson> wk = new List<Lesson>();
                                    wk.AddRange(week[i].ScheduleDays[a].Lessons.Where(les => les.Name.Contains("-")).ToList());
                                    wk.AddRange(week[i].ScheduleDays[a].Lessons.Where(les => les.Name.Contains("+")).ToList());
                                    foreach (var l in wk)
                                    {
                                        week[i].ScheduleDays[a].Lessons.Remove(l);
                                    }
                                }
                            }
                        }
                        return View(week);
                    }
                    else
                    {
                        DateTime thisDay = DateTime.Now;
                        foreach (var w in week)
                        {
                            if (thisDay >= Convert.ToDateTime(w.From).AddDays(-2) && thisDay <= Convert.ToDateTime(w.To).AddDays(1))
                            {
                                ViewBag.Weeks = week;
                                ViewBag.Group = group;
                                Week weekResult = db.Weeks.Find(w.Id);
                                if (group.Contains("+"))
                                {
                                        for (byte a = 0; a < weekResult.ScheduleDays.Count; a++)
                                        {
                                            List<Lesson> wk = new List<Lesson>();
                                            wk.AddRange(weekResult.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("-")).ToList());
                                            wk.AddRange(weekResult.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("*")).ToList());
                                            foreach (var l in wk)
                                            {
                                                weekResult.ScheduleDays[a].Lessons.Remove(l);
                                            }
                                        }
                                }
                                if (group.Contains("-"))
                                {
                                    for (byte a = 0; a < weekResult.ScheduleDays.Count; a++)
                                    {
                                        List<Lesson> wk = new List<Lesson>();
                                        wk.AddRange(weekResult.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("+")).ToList());
                                        wk.AddRange(weekResult.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("*")).ToList());
                                        foreach (var l in wk)
                                        {
                                            weekResult.ScheduleDays[a].Lessons.Remove(l);
                                        }
                                    }
                                }
                                if (group.Contains("*"))
                                {
                                    for (byte a = 0; a < weekResult.ScheduleDays.Count; a++)
                                    {
                                        List<Lesson> wk = new List<Lesson>();
                                        wk.AddRange(weekResult.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("-")).ToList());
                                        wk.AddRange(weekResult.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("+")).ToList());
                                        foreach (var l in wk)
                                        {
                                            weekResult.ScheduleDays[a].Lessons.Remove(l);
                                        }
                                    }
                                }
                                return View("ScheduleBig", weekResult);
                            }
                        }
                        return View("ScheduleBig");
                    }
                }
                else
                {
                    c = HttpContext.Request.Cookies["Course"];
                    g = HttpContext.Request.Cookies["Group"];
                    week = await db.Weeks.Where(w => w.Course == Convert.ToByte(c) && w.Group == g.Replace("+", "").Replace("-", "").Replace("*", "")).Include(day => day.ScheduleDays).ThenInclude(les => les.Lessons).ToListAsync();
                    ViewBag.Course = c;
                    ViewBag.Group = g;
                    if (week[0].From == null)
                    {
                        if (g.Contains("+"))
                        {
                            for (byte i = 0; i < week.Count; i++)
                            {
                                for (byte a = 0; a < week[i].ScheduleDays.Count; a++)
                                {
                                    List<Lesson> wk = new List<Lesson>();
                                    wk.AddRange(week[i].ScheduleDays[a].Lessons.Where(les => les.Name.Contains("-")).ToList());
                                    wk.AddRange(week[i].ScheduleDays[a].Lessons.Where(les => les.Name.Contains("*")).ToList());
                                    foreach (var l in wk)
                                    {
                                        week[i].ScheduleDays[a].Lessons.Remove(l);
                                    }
                                }
                            }
                        }
                        if (g.Contains("-"))
                        {
                            for (byte i = 0; i < week.Count; i++)
                            {
                                for (byte a = 0; a < week[i].ScheduleDays.Count; a++)
                                {
                                    List<Lesson> wk = new List<Lesson>();
                                    wk.AddRange(week[i].ScheduleDays[a].Lessons.Where(les => les.Name.Contains("+")).ToList());
                                    wk.AddRange(week[i].ScheduleDays[a].Lessons.Where(les => les.Name.Contains("*")).ToList());
                                    foreach (var l in wk)
                                    {
                                        week[i].ScheduleDays[a].Lessons.Remove(l);
                                    }
                                }
                            }
                        }
                        if (g.Contains("*"))
                        {
                            for (byte i = 0; i < week.Count; i++)
                            {
                                for (byte a = 0; a < week[i].ScheduleDays.Count; a++)
                                {
                                    List<Lesson> wk = new List<Lesson>();
                                    wk.AddRange(week[i].ScheduleDays[a].Lessons.Where(les => les.Name.Contains("-")).ToList());
                                    wk.AddRange(week[i].ScheduleDays[a].Lessons.Where(les => les.Name.Contains("+")).ToList());
                                    foreach (var l in wk)
                                    {
                                        week[i].ScheduleDays[a].Lessons.Remove(l);
                                    }
                                }
                            }
                        }
                        return View(week);
                    }
                    else
                    {
                        DateTime thisDay = DateTime.Today;
                        foreach (var w in week)
                        {
                            if (thisDay >= Convert.ToDateTime(w.From).AddDays(-2) && thisDay <= Convert.ToDateTime(w.To).AddDays(1))
                            {
                                ViewBag.Weeks = week;
                                ViewBag.Group = g;
                                Week weekResult = db.Weeks.Find(w.Id);
                                if (g.Contains("+"))
                                {
                                    for (byte a = 0; a < weekResult.ScheduleDays.Count; a++)
                                    {
                                        List<Lesson> wk = new List<Lesson>();
                                        wk.AddRange(weekResult.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("-")).ToList());
                                        wk.AddRange(weekResult.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("*")).ToList());
                                        foreach (var l in wk)
                                        {
                                            weekResult.ScheduleDays[a].Lessons.Remove(l);
                                        }
                                    }
                                }
                                if (g.Contains("-"))
                                {
                                    for (byte a = 0; a < weekResult.ScheduleDays.Count; a++)
                                    {
                                        List<Lesson> wk = new List<Lesson>();
                                        wk.AddRange(weekResult.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("+")).ToList());
                                        wk.AddRange(weekResult.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("*")).ToList());
                                        foreach (var l in wk)
                                        {
                                            weekResult.ScheduleDays[a].Lessons.Remove(l);
                                        }
                                    }
                                }
                                if (g.Contains("*"))
                                {
                                    for (byte a = 0; a < weekResult.ScheduleDays.Count; a++)
                                    {
                                        List<Lesson> wk = new List<Lesson>();
                                        wk.AddRange(weekResult.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("-")).ToList());
                                        wk.AddRange(weekResult.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("+")).ToList());
                                        foreach (var l in wk)
                                        {
                                            weekResult.ScheduleDays[a].Lessons.Remove(l);
                                        }
                                    }
                                }
                                return View("ScheduleBig", weekResult);
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
        public async Task<IActionResult> ScheduleBig(int WeekId, string group)
        {
            Week week = await db.Weeks.Include(day => day.ScheduleDays).ThenInclude(les => les.Lessons).FirstOrDefaultAsync(w => w.Id == WeekId);
            if (group.Contains("+"))
            {
                for (byte a = 0; a < week.ScheduleDays.Count; a++)
                {
                    List<Lesson> wk = new List<Lesson>();
                    wk.AddRange(week.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("-")).ToList());
                    wk.AddRange(week.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("*")).ToList());
                    foreach (var l in wk)
                    {
                        week.ScheduleDays[a].Lessons.Remove(l);
                    }
                }
            }
            if (group.Contains("-"))
            {
                for (byte a = 0; a < week.ScheduleDays.Count; a++)
                {
                    List<Lesson> wk = new List<Lesson>();
                    wk.AddRange(week.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("+")).ToList());
                    wk.AddRange(week.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("*")).ToList());
                    foreach (var l in wk)
                    {
                        week.ScheduleDays[a].Lessons.Remove(l);
                    }
                }
            }
            if (group.Contains("*"))
            {
                for (byte a = 0; a < week.ScheduleDays.Count; a++)
                {
                    List<Lesson> wk = new List<Lesson>();
                    wk.AddRange(week.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("-")).ToList());
                    wk.AddRange(week.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("+")).ToList());
                    foreach (var l in wk)
                    {
                        week.ScheduleDays[a].Lessons.Remove(l);
                    }
                }
            }
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
        [Route("instruction")]
        [HttpGet]
        public ActionResult Instruction()
        {
            return View();
        }
        public ActionResult Documents(int id)
        {
            var lesson = db.Lessons.Include(l => l.ThematicPlan).Include(l=> l.MethodicalRecomendation).SingleOrDefault(l => l.Id == id);
            if (lesson.ThematicPlan.Id == -1 && lesson.MethodicalRecomendation.Id == -1) return View("NoThematicPlan");
            return View(lesson);
        }
        public ActionResult MethodicalRecomendation(int id)
        {
            MethodicalRecomendation mr = db.MethodicalRecomendations.Find(id);
            if(mr.Name.Contains(".pdf"))
            {
                return LocalRedirect("~"+ mr.Path);
            }
            else
            {
                return Redirect("https://view.officeapps.live.com/op/embed.aspx?src=http://" + Request.Host + "" + mr.Path);
            }
        }
        public ActionResult ThematicPlan(int id)
        {
            ThematicPlan tp = db.ThematicPlans.Find(id);
            if (tp.Name.Contains(".pdf"))
            {
                return LocalRedirect("~" + tp.Path);
            }
            else
            {
                return Redirect("https://view.officeapps.live.com/op/embed.aspx?src=http://" + Request.Host + "" + tp.Path);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
