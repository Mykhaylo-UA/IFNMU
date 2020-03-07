using System.Linq;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IFNMUSiteCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization; 

namespace IFNMUSiteCore.Controllers
{
    public class HomeController : Controller
    {
        private LessonContext db; // контекст БД
        private IWebHostEnvironment _appEnvironment;
        UserManager<User> _users;
        public HomeController(LessonContext context, IWebHostEnvironment appEnvironment, UserManager<User> usersManager) // конструктор
        {
            db = context;
            _appEnvironment = appEnvironment;
            _users = usersManager;
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
                        Week weekDefault = db.Weeks.Find(week[0].Id);
                        ViewBag.Weeks = week;
                        ViewBag.Group = group;
                        if (group.Contains("+"))
                        {
                            for (byte a = 0; a < weekDefault.ScheduleDays.Count; a++)
                            {
                                List<Lesson> wk = new List<Lesson>();
                                wk.AddRange(weekDefault.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("-")).ToList());
                                wk.AddRange(weekDefault.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("*")).ToList());
                                foreach (var l in wk)
                                {
                                    weekDefault.ScheduleDays[a].Lessons.Remove(l);
                                }
                            }
                        }
                        if (group.Contains("-"))
                        {
                            for (byte a = 0; a < weekDefault.ScheduleDays.Count; a++)
                            {
                                List<Lesson> wk = new List<Lesson>();
                                wk.AddRange(weekDefault.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("+")).ToList());
                                wk.AddRange(weekDefault.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("*")).ToList());
                                foreach (var l in wk)
                                {
                                    weekDefault.ScheduleDays[a].Lessons.Remove(l);
                                }
                            }
                        }
                        if (group.Contains("*"))
                        {
                            for (byte a = 0; a < weekDefault.ScheduleDays.Count; a++)
                            {
                                List<Lesson> wk = new List<Lesson>();
                                wk.AddRange(weekDefault.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("-")).ToList());
                                wk.AddRange(weekDefault.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("+")).ToList());
                                foreach (var l in wk)
                                {
                                    weekDefault.ScheduleDays[a].Lessons.Remove(l);
                                }
                            }
                        }
                        return View("ScheduleBig", weekDefault);
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
                        Week weekDefault = db.Weeks.Find(week[0].Id);
                        ViewBag.Weeks = week;
                        ViewBag.Group = g;
                        if (g.Contains("+"))
                        {
                            for (byte a = 0; a < weekDefault.ScheduleDays.Count; a++)
                            {
                                List<Lesson> wk = new List<Lesson>();
                                wk.AddRange(weekDefault.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("-")).ToList());
                                wk.AddRange(weekDefault.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("*")).ToList());
                                foreach (var l in wk)
                                {
                                    weekDefault.ScheduleDays[a].Lessons.Remove(l);
                                }
                            }
                        }
                        if (g.Contains("-"))
                        {
                            for (byte a = 0; a < weekDefault.ScheduleDays.Count; a++)
                            {
                                List<Lesson> wk = new List<Lesson>();
                                wk.AddRange(weekDefault.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("+")).ToList());
                                wk.AddRange(weekDefault.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("*")).ToList());
                                foreach (var l in wk)
                                {
                                    weekDefault.ScheduleDays[a].Lessons.Remove(l);
                                }
                            }
                        }
                        if (g.Contains("*"))
                        {
                            for (byte a = 0; a < weekDefault.ScheduleDays.Count; a++)
                            {
                                List<Lesson> wk = new List<Lesson>();
                                wk.AddRange(weekDefault.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("-")).ToList());
                                wk.AddRange(weekDefault.ScheduleDays[a].Lessons.Where(les => les.Name.Contains("+")).ToList());
                                foreach (var l in wk)
                                {
                                    weekDefault.ScheduleDays[a].Lessons.Remove(l);
                                }
                            }
                        }
                        return View("ScheduleBig", weekDefault);
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
            try
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
            catch(Exception exp)
            {
                return View("Warning", exp);
            }
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
            if (lesson.ThematicPlanId == null && lesson.MethodicalRecomendationId ==null) return View("NoThematicPlan");
            return View(lesson);
        }
        public ActionResult MethodicalRecomendation(int id)
        {
            File mr = db.Files.Find(id);
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
            File tp = db.Files.Find(id);
            if (tp.Name.Contains(".pdf"))
            {
                return LocalRedirect("~" + tp.Path);
            }
            else
            {
                return Redirect("https://view.officeapps.live.com/op/embed.aspx?src=http://" + Request.Host + "" + tp.Path);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Chair(int id)
        {
            if(User.Identity.IsAuthenticated)
            {
                var user = await GetCurrentUserAsync();
                if(user.ChairId != null)
                {
                    return RedirectToAction("Chair", "Admin", new { id = user.ChairId });
                }
            }
            ViewBag.Host = HttpContext.Request.Host.ToString();
            return View(db.Chairs.Include(a => a.Folders).Include(b => b.Files).Include(c => c.Advertisements).Include(d=> d.Graphics).Single(c => c.Id == id));
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
            catch (Exception exp)
            {
                return View("Warning", exp);
            }
        }

        [Authorize(Roles="moderator, admin")]
        [HttpGet]
        public IActionResult Panel()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [NonAction]
        private Task<User> GetCurrentUserAsync() => _users.GetUserAsync(HttpContext.User);
    }
}
