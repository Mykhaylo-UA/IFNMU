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
using System.Text.Json;

namespace IFNMUSiteCore.Controllers
{
    [Authorize(Roles = "moderator")]
    public class ScheduleController : Controller
    {
        readonly private LessonContext db; // контекст БД
        readonly private IWebHostEnvironment _appEnvironment;
        readonly private UserManager<User> _users;

        public ScheduleController(LessonContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager) // конструктор
        {
            db = context;
            _appEnvironment = appEnvironment;
            _users = userManager;
        }
        public ActionResult Index()
        {
            return View();
        }
        // Сторінка "Додати день до розкладу"
        [HttpGet]
        public ActionResult AddWeek()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddWeek(Week w)
        {
            try
            {
                w.Faculty = FacultyDetermine(w.Group);
                await db.Weeks.AddAsync(w);
                await db.ScheduleDays.AddRangeAsync(w.ScheduleDays);
                await db.SaveChangesAsync();
                List<Lesson>[] les = new List<Lesson>[5];
                for (byte a = 0; a < 5; a++)
                {
                    les[a] = new List<Lesson>();
                    for (int i = 0; i < HttpContext.Request.Form["ScheduleDays[" + a + "].Lessons[].LessonNumber"].Count; i++)
                    {
                        les[a].Add(new Lesson()
                        {
                            Name = HttpContext.Request.Form["ScheduleDays[" + a + "].Lessons[].Name"][i],
                            LessonNumber = Convert.ToInt32(HttpContext.Request.Form["ScheduleDays[" + a + "].Lessons[].LessonNumber"][i]),
                            ScheduleDayId = w.ScheduleDays[a].Id
                        });

                    }
                    await db.Lessons.AddRangeAsync(les[a]);
                }
                await db.SaveChangesAsync();
                return View();
            }
            catch (Exception e)
            {
                return BadRequest("Помилка запиту (" + e.Message + ")");
            }
        }
        // Додати урок
        public IActionResult AddLesson(byte dayNumber, byte index)
        {
            return PartialView(dayNumber);
        }
        [HttpGet]
        public IActionResult EditWeek(int course, string group)
        {
            return View(db.Weeks.Where(w => w.Course == course && w.Group == group).Include(s => s.ScheduleDays).ThenInclude(sch => sch.Lessons).ToList());
        }
        [HttpGet]
        public IActionResult EditDay(int id)
        {
            return View(db.ScheduleDays.Include(a => a.Lessons).Single(i => i.Id == id));
        }
        [HttpGet]
        public async Task<IActionResult> EditLesson(int id)
        {
            ViewBag.Chairs = await db.Chairs.ToListAsync();
            Lesson lesson = await db.Lessons.Include(c => c.Chair).SingleAsync(l => l.Id == id);
            if (lesson.Chair != null)
            {
                ViewBag.Chairs.Remove(lesson.Chair);
            }
            Chair l = await db.Chairs.SingleOrDefaultAsync(a => a.Name.Contains("Тематичні"));
            Chair al = await db.Chairs.SingleOrDefaultAsync(a => a.Name.Contains("Методичні"));
            ViewBag.Chairs.Remove(al);
            ViewBag.Chairs.Remove(l);
            return View(lesson);
        }
        [HttpPost]
        public IActionResult EditLesson(Lesson lesson)
        {
            if (lesson != null)
            {
                if (HttpContext.Request.Form["ChairId"] != "null")
                {
                    lesson.ChairId = Convert.ToInt32(HttpContext.Request.Form["ChairId"]);
                }
                if (HttpContext.Request.Form["ThematicPlanId"] != "null")
                {
                    lesson.ThematicPlanId = Convert.ToInt32(HttpContext.Request.Form["ThematicPlanId"]);
                }
                if (HttpContext.Request.Form["MethodicalRecomendationId"] != "null")
                {
                    lesson.MethodicalRecomendationId = Convert.ToInt32(HttpContext.Request.Form["MethodicalRecomendationId"]);
                }

                db.Update(lesson);
                db.SaveChanges();
            }
            return RedirectToAction("EditDay", new { id = lesson.ScheduleDayId });
        }

        [HttpGet]
        public IActionResult DeleteLesson(int id)
        {
            return View(db.Lessons.Find(id));
        }
        [HttpPost]
        public IActionResult DeleteLesson(Lesson lesson)
        {
            int Id = lesson.Id;
            Lesson les = db.Lessons.FirstOrDefault(l => l.Id == lesson.Id);
            db.Remove(les);
            db.SaveChanges();
            return RedirectToAction("EditDay", new { id = Id });
        }
        [HttpGet]
        public IActionResult AddLessonOne(int id)
        {
            return View(id);
        }
        [HttpPost]
        public IActionResult AddLessonOne(Lesson lesson)
        {
            db.Add(lesson);
            db.SaveChanges();
            return RedirectToAction("EditDay", new { id = lesson.ScheduleDayId });
        }

        [HttpGet]
        public IActionResult EditMRTP(int? count = null)
        {
            ViewBag.CountUpdate = count;
            return View(db.Chairs.ToList());
        }
        [HttpPost]
        public async Task<IActionResult> EditMRTP(string nameLesson, int numberCourse, Faculty faculty, int? MethodicalRecomendationId, int? ThematicPlanId, string group, int regexIndex, int? idChair)
        {
            // @"патоморфологія Л\d*" - лекція
            // @"патоморфологія$" - "Патоморфологія"
            // @"патоморфологія \(\w*" - (підгрупа, клінічка)
            List<Week> week;
            if (group == null || group.Trim() == "" || group == "")
            {
                week = await db.Weeks.Where(p => p.Course == numberCourse && p.Faculty == faculty).Include(s => s.ScheduleDays).ThenInclude(les => les.Lessons).ToListAsync();
            }
            else
            {
                week = await db.Weeks.Where(p => p.Course == numberCourse && p.Group.ToLower() == group.ToLower()).Include(s => s.ScheduleDays).ThenInclude(les => les.Lessons).ToListAsync();
            }
            Regex regex = null;
            Regex regex2 = null;
            if (regexIndex == 1)
            {
                if (nameLesson.Contains("підгрупа") || nameLesson.Contains("клінічка") || nameLesson.Contains("(") || nameLesson.Contains(")"))
                {
                    nameLesson = nameLesson.Replace("підгрупа ", @"підгрупа\s").Replace("клінічка", @"клінічка\s").Replace("(", @"\(").Replace(")", @"\)");
                    regex = new Regex(nameLesson, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                }
                else
                {
                    regex = new Regex(@$"{nameLesson}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    regex2 = new Regex(@$"{nameLesson}\(\w*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                }
            }
            else regex = new Regex(@$"{nameLesson} Л\d*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
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
                        matches = regex2.Matches(lesson.Name);
                        if (matches.Count > 0) lessons.Add(lesson);
                    }
                }
            }
            for (int i = 0; i < lessons.Count; i++)
            {
                lessons[i].MethodicalRecomendationId = MethodicalRecomendationId;
                lessons[i].ThematicPlanId = ThematicPlanId;
                lessons[i].ChairId = idChair;
            }
            db.Lessons.UpdateRange(lessons);
            await db.SaveChangesAsync();
            return RedirectToAction("EditMRTP", new { count = lessons.Count() });
        }

        [HttpGet]
        public IActionResult AddSchedule()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSchedule(string[][] item, string[] potikName, string[] potikGroups, byte course)
        {
            bool subGroup = true;
            List<Week> weeks = new List<Week>();

            for (byte i = 0; i < item.Length; i++)
            {
                for (byte j = 1; j < item[i].Length; j++)
                {
                    if (item[i][j] == null) continue;
                    if (item[i][j].Contains("*"))
                    {
                        subGroup = false;
                        break;
                    }
                }

                for (byte j = 1; j < item[i].Length; j++)
                {
                    if (item[i][j] == null) continue;
                    string[] splitItems = item[i][j].Split(" ");

                    byte lessonNumber = 0;
                    byte dayNumber = 0;
                    byte weekNumber = 0;

                    switch ((double)j / 4 - Math.Truncate((double)j / 4)) // вираховуємо номер заняття
                    {
                        case (double)0.25:
                            lessonNumber = 1;
                            break;
                        case (double)0.5:
                            lessonNumber = 2;
                            break;
                        case (double)0.75:
                            lessonNumber = 3;
                            break;
                        case (double)0:
                            lessonNumber = 4;
                            break;
                    }

                    if (j < 5) dayNumber = 1; // вираховуємо день тижня
                    else if (j < 9) dayNumber = 2;
                    else if (j < 13) dayNumber = 3;
                    else if (j < 17) dayNumber = 4;
                    else if (j < 21) dayNumber = 5;
                    else if (j < 25) dayNumber = 1;
                    else if (j < 29) dayNumber = 2;
                    else if (j < 33) dayNumber = 3;
                    else if (j < 37) dayNumber = 4;
                    else if (j < 41) dayNumber = 5;

                    if (j <= item[i].Length / 2) weekNumber = 1;
                    else weekNumber = 2;

                    foreach (string splitItem in splitItems)
                    {
                        Regex regex;
                        MatchCollection matches;
                        regex = new Regex(@"[а-я][0-9]{1,2}");
                        matches = regex.Matches(splitItem.ToLower());

                        string lessonTitle = String.Empty;

                        if (matches.Count > 0)
                        {
                            lessonTitle = item[i][0] + " " + splitItem.ToUpper();
                            for (byte g = 0; g < potikName.Length; g++)
                            {
                                if (potikName[g] == null) continue;
                                if (splitItem.ToLower().Contains(potikName[g].ToLower()))
                                {
                                    foreach (string ptGroups in potikGroups[g].Split(" "))
                                    {
                                        bool weekFound = false;
                                        string groupNumber = ptGroups;

                                        foreach (Week week in weeks)
                                        {
                                            if (week.Group.Contains(groupNumber))
                                            {
                                                weekFound = true;
                                            }
                                        }
                                        if (weekFound == false)
                                        {
                                            weeks.Add(new Week()
                                            {
                                                Group = groupNumber,
                                                Faculty = FacultyDetermine(groupNumber),
                                                Course = course,
                                                NumberWeek = 1,
                                                ScheduleDays = {
                                                    new ScheduleDay(){ DayNumber = 1},
                                                    new ScheduleDay(){ DayNumber = 2},
                                                    new ScheduleDay(){ DayNumber = 3},
                                                    new ScheduleDay(){ DayNumber = 4},
                                                    new ScheduleDay(){ DayNumber = 5}
                                                }
                                            });
                                            weeks.Add(new Week()
                                            {
                                                Group = groupNumber,
                                                Faculty = FacultyDetermine(groupNumber),
                                                Course = course,
                                                NumberWeek = 2,
                                                ScheduleDays = {
                                                    new ScheduleDay(){ DayNumber = 1},
                                                    new ScheduleDay(){ DayNumber = 2},
                                                    new ScheduleDay(){ DayNumber = 3},
                                                    new ScheduleDay(){ DayNumber = 4},
                                                    new ScheduleDay(){ DayNumber = 5}
                                                }
                                            });
                                        }
                                        weeks.First(week => week.Group == ptGroups && week.Course == course && week.NumberWeek == weekNumber)
                                           .ScheduleDays.First(day => day.DayNumber == dayNumber)
                                           .Lessons.Add(
                                               new Lesson()
                                               {
                                                   Name = lessonTitle,
                                                   LessonNumber = (int)lessonNumber
                                               }
                                        );
                                    }
                                }
                            }
                        } //провіряємо чи підходить перший шаблон
                        else // переходимо до другого шаблона
                        {
                            regex = new Regex(@"^[а-я]$");
                            matches = regex.Matches(splitItem.ToLower());
                            if (matches.Count > 0) // провіряємо чи підходить другий шаблон
                            {
                                lessonTitle = item[i][0] + " " + splitItem.ToUpper();
                                for (byte g = 0; g < potikName.Length; g++)
                                {
                                    if (potikName[g] == null) continue;
                                    if (splitItem.ToLower().Contains(potikName[g].ToLower()))
                                    {
                                        foreach (string ptGroups in potikGroups[g].Split(" "))
                                        {
                                            bool weekFound = false;
                                            string groupNumber = ptGroups;

                                            foreach (Week week in weeks)
                                            {
                                                if (week.Group.Contains(groupNumber))
                                                {
                                                    weekFound = true;
                                                }
                                            }
                                            if (weekFound == false)
                                            {
                                                weeks.Add(new Week()
                                                {
                                                    Group = groupNumber,
                                                    Faculty = FacultyDetermine(groupNumber),
                                                    Course = course,
                                                    NumberWeek = 1,
                                                    ScheduleDays = {
                                                    new ScheduleDay(){ DayNumber = 1},
                                                    new ScheduleDay(){ DayNumber = 2},
                                                    new ScheduleDay(){ DayNumber = 3},
                                                    new ScheduleDay(){ DayNumber = 4},
                                                    new ScheduleDay(){ DayNumber = 5}
                                                }
                                                });
                                                weeks.Add(new Week()
                                                {
                                                    Group = groupNumber,
                                                    Faculty = FacultyDetermine(groupNumber),
                                                    Course = course,
                                                    NumberWeek = 2,
                                                    ScheduleDays = {
                                                    new ScheduleDay(){ DayNumber = 1},
                                                    new ScheduleDay(){ DayNumber = 2},
                                                    new ScheduleDay(){ DayNumber = 3},
                                                    new ScheduleDay(){ DayNumber = 4},
                                                    new ScheduleDay(){ DayNumber = 5}
                                                }
                                                });
                                            }
                                            weeks.First(week => week.Group == ptGroups && week.Course == course && week.NumberWeek == weekNumber)
                                               .ScheduleDays.First(day => day.DayNumber == dayNumber)
                                               .Lessons.Add(
                                                   new Lesson()
                                                   {
                                                       Name = lessonTitle,
                                                       LessonNumber = (int)lessonNumber
                                                   }
                                            );
                                        }
                                    }
                                }
                            }
                            else // якщо шаблон не підходить додаємо в розклад одне заняття для певної групи
                            {
                                bool weekFound = false;
                                string groupNumber = splitItem.Replace("+", "").Replace("-", "").Replace("*", "").ToLower();

                                foreach (Week week in weeks)
                                {
                                    if (week.Group.Contains(groupNumber))
                                    {
                                        weekFound = true;
                                    }
                                }
                                if (weekFound == false)
                                {
                                    weeks.Add(new Week()
                                    {
                                        Group = groupNumber,
                                        Faculty = FacultyDetermine(groupNumber),
                                        Course = course,
                                        NumberWeek = 1,
                                        ScheduleDays = {
                                                    new ScheduleDay(){ DayNumber = 1},
                                                    new ScheduleDay(){ DayNumber = 2},
                                                    new ScheduleDay(){ DayNumber = 3},
                                                    new ScheduleDay(){ DayNumber = 4},
                                                    new ScheduleDay(){ DayNumber = 5}
                                                }
                                    });
                                    weeks.Add(new Week()
                                    {
                                        Group = groupNumber,
                                        Faculty = FacultyDetermine(groupNumber),
                                        Course = course,
                                        NumberWeek = 2,
                                        ScheduleDays = {
                                                    new ScheduleDay(){ DayNumber = 1},
                                                    new ScheduleDay(){ DayNumber = 2},
                                                    new ScheduleDay(){ DayNumber = 3},
                                                    new ScheduleDay(){ DayNumber = 4},
                                                    new ScheduleDay(){ DayNumber = 5}
                                                }
                                    });
                                }
                                if (splitItem.Contains("+"))
                                {
                                    lessonTitle = item[i][0] + (subGroup == true ? " (підгрупа +)" : " (клінічка +)");
                                }
                                else if (splitItem.Contains("-"))
                                {
                                    lessonTitle = item[i][0] + (subGroup == true ? " (підгрупа -)" : " (клінічка -)");
                                }
                                else if (splitItem.Contains("*"))
                                {
                                    lessonTitle = item[i][0] + " (клінічка *)";
                                }
                                else
                                {
                                    lessonTitle = item[i][0];
                                }

                                weeks.First(week => week.Group == groupNumber && week.Course == course && week.NumberWeek == weekNumber)
                                    .ScheduleDays.First(day => day.DayNumber == dayNumber)
                                    .Lessons.Add(
                                        new Lesson()
                                        {
                                            Name = lessonTitle,
                                            LessonNumber = (int)lessonNumber
                                        }
                                );
                            }
                        }
                    }
                }
            }

            await db.Weeks.AddRangeAsync(weeks);

            await db.SaveChangesAsync();

            return Content("Введені Вами дані опрацьовані.");
        }

        [HttpGet]
        public IActionResult AddRow(int index)
        {
            return View(index);
        }

        [HttpGet]
        public IActionResult AddScheduleBig()
        {
            return View(WeeksDate());
        }

        [HttpGet]
        public IActionResult AddScheduleBigTable(byte selectDate, bool newTable = false, int countRow = 1)
        {
            ViewBag.NewTable = newTable;
            ViewBag.SelectDate = selectDate;
            ViewBag.CountRow = countRow;
            return View("AddScheduleBigTable", WeeksDate()[(int)selectDate]);
        }
        [HttpPost]
        public async Task<IActionResult> AddScheduleBig(Dictionary<int, string[]> items, string[] potikName, string[] potikGroups, byte course)
        {
            List<DateTime[]> weeksDate = WeeksDate();
            List<Week> weeks = new List<Week>();
            foreach (var day in items)
            {
                if(day.Key==-1)
                {
                    continue;
                }
                string[] stringInputs = day.Value;
                for (byte numberDay = 0; numberDay < stringInputs.Length; numberDay++)
                {
                    int countDayInWeek = (weeksDate[day.Key][1] - weeksDate[day.Key][0]).Days;

                    int d = (int)(numberDay / (countDayInWeek+1));

                    if (stringInputs[numberDay].Contains("/"))
                    {
                        string[] stringSplits = stringInputs[numberDay].Trim().Split("/");

                        

                        for (byte numberLesson = 0; numberLesson < (stringSplits.Length - 1); numberLesson++)
                        {
                            

                            string[] strs = stringSplits[numberLesson].Trim().Split(" ");
                            foreach (string str in strs)
                            {
                                Regex regex;
                                MatchCollection matches;
                                regex = new Regex(@"[а-я][0-9]{1}\D");
                                matches = regex.Matches(str.ToLower());
                                string lessonTitle = string.Empty;

                                if (matches.Count > 0) // якщо підходить перший  шаблон
                                {
                                    lessonTitle = items[-1][d] + " " + str.Remove(1, 2).ToUpper();
                                    for (byte g = 0; g < potikName.Length; g++)
                                    {
                                        if (potikName[g] == null) continue;
                                        if (str.ToLower().Contains(potikName[g].ToLower()))
                                        {
                                            foreach (string ptGroups in potikGroups[g].Split(" "))
                                            {
                                                bool weekFound = false;
                                                string groupNumber = ptGroups;

                                                foreach (var week in weeks)
                                                {
                                                    if (week.Group.Contains(groupNumber) && week.From == weeksDate[day.Key][0])
                                                    {
                                                        weekFound = true;
                                                        break;
                                                    }
                                                }

                                                if (weekFound == false)
                                                {
                                                    Week w = new Week()
                                                    {
                                                        Faculty = FacultyDetermine(groupNumber),
                                                        Group = groupNumber,
                                                        From = weeksDate[day.Key][0],
                                                        To = weeksDate[day.Key][1],
                                                        NumberWeek = (byte)day.Key,
                                                        Course = course,
                                                    };
                                                    for (byte i = 1; i < (weeksDate[day.Key][1] - weeksDate[day.Key][0]).Days + 2; i++)
                                                    {
                                                        w.ScheduleDays.Add(new ScheduleDay() { DayNumber = i });
                                                    }
                                                    weeks.Add(w);
                                                }
                                                int numb = 1;
                                                foreach (char c in str)
                                                {
                                                    bool isNum = int.TryParse(c.ToString(), out numb);
                                                    if (isNum) break;

                                                }
                                                weeks.First(w => w.Group == ptGroups && w.Course == course && w.From == weeksDate[day.Key][0] && w.To == weeksDate[day.Key][1])
                                                    .ScheduleDays.First(day => day.DayNumber == (numberDay <= countDayInWeek ? numberDay + 1 : numberDay - countDayInWeek))
                                                    .Lessons.Add(
                                                            new Lesson()
                                                            {
                                                                Name = lessonTitle,
                                                                LessonNumber = numb
                                                            }
                                                    );
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bool weekFound = false;
                                    string groupNumber = str.Replace("+", "").Replace("-", "").Replace("*", "").ToLower();

                                  

                                    if (str.Contains("+"))
                                    {
                                        lessonTitle = items[-1][d] + " (клінічка +)";
                                    }
                                    else if (str.Contains("-"))
                                    {
                                        lessonTitle = items[-1][d] + " (клінічка -)";
                                    }
                                    else if (str.Contains("*"))
                                    {
                                        lessonTitle = items[-1][d] + " (клінічка *)";
                                    }
                                    else
                                    {
                                        lessonTitle = items[-1][d];
                                    }

                                    foreach (var week in weeks)
                                    {
                                        if (week.Group.Contains(groupNumber) && week.From == weeksDate[day.Key][0])
                                        {
                                            weekFound = true;
                                        }
                                    }

                                    if (weekFound == false)
                                    {
                                        Week w = new Week()
                                        {
                                            Faculty = FacultyDetermine(groupNumber),
                                            Group = groupNumber,
                                            From = weeksDate[day.Key][0],
                                            To = weeksDate[day.Key][1],
                                            NumberWeek = (byte)day.Key,
                                            Course = course,
                                        };
                                        for (byte i = 1; i < (weeksDate[day.Key][1] - weeksDate[day.Key][0]).Days + 2; i++)
                                        {
                                            w.ScheduleDays.Add(new ScheduleDay() { DayNumber = i });
                                        }
                                        weeks.Add(w);
                                    }

                                    weeks.First(w => w.Group == groupNumber && w.Course == course && w.From == weeksDate[day.Key][0] && w.To == weeksDate[day.Key][1])
                                        .ScheduleDays.First(day => day.DayNumber == (numberDay <= countDayInWeek ? numberDay+1 : numberDay - countDayInWeek))
                                        .Lessons.AddRange(new[] {
                                    new Lesson()
                                    {
                                        Name = lessonTitle,
                                        LessonNumber = numberLesson == 0 ? 1 : 3
                                    },
                                    new Lesson()
                                    {
                                        Name = lessonTitle,
                                        LessonNumber = numberLesson == 0 ? 2 : 4
                                    }
                                            }
                                        );
                                }

                            };
                        }
                    }
                    else
                    {
                        string[] strs = stringInputs[numberDay].Trim().Split(" ");
                        foreach (string str in strs)
                        {
                            Regex regex;
                            MatchCollection matches;
                            regex = new Regex(@"[а-я][0-9]{1}\D");
                            matches = regex.Matches(str.ToLower());
                            string lessonTitle = string.Empty;

                            if (matches.Count > 0) // якщо підходить перший  шаблон
                            {
                                lessonTitle = items[-1][d] + " " + str.Remove(1, 2).ToUpper();
                                for (byte g = 0; g < potikName.Length; g++)
                                {
                                    if (potikName[g] == null) continue;
                                    if (str.ToLower().Contains(potikName[g].ToLower()))
                                    {
                                        foreach (string ptGroups in potikGroups[g].Split(" "))
                                        {
                                            bool weekFound = false;
                                            string groupNumber = ptGroups;

                                            foreach (var week in weeks)
                                            {
                                                if (week.Group.Contains(groupNumber) && week.From == weeksDate[day.Key][0])
                                                {
                                                    weekFound = true;
                                                }
                                            }

                                            if (weekFound == false)
                                            {
                                                Week w = new Week()
                                                {
                                                    Faculty = FacultyDetermine(groupNumber),
                                                    Group = groupNumber,
                                                    From = weeksDate[day.Key][0],
                                                    To = weeksDate[day.Key][1],
                                                    NumberWeek = (byte)day.Key,
                                                    Course = course,
                                                };
                                                for (byte i = 1; i < (weeksDate[day.Key][1] - weeksDate[day.Key][0]).Days + 2; i++)
                                                {
                                                    w.ScheduleDays.Add(new ScheduleDay() { DayNumber = i });
                                                }
                                                weeks.Add(w);
                                            }
                                            int numb = 1;
                                            foreach (char c in str)
                                            {
                                                bool isNum = int.TryParse(c.ToString(), out numb);
                                                if (isNum) break;

                                            }
                                            weeks.First(w => w.Group == ptGroups && w.Course == course && w.From == weeksDate[day.Key][0] && w.To == weeksDate[day.Key][1])
                                                .ScheduleDays.First(day => day.DayNumber == (numberDay <= countDayInWeek ? numberDay + 1 : numberDay - countDayInWeek))
                                                .Lessons.Add(
                                                        new Lesson()
                                                        {
                                                            Name = lessonTitle,
                                                            LessonNumber = numb
                                                        }
                                                );
                                        }
                                    }
                                }
                            }
                            else
                            {
                                bool weekFound = false;
                                string groupNumber = str.Replace("+", "").Replace("-", "").Replace("*", "").ToLower();

                                if (str.Contains("+"))
                                {
                                    lessonTitle = items[-1][d] + " (клінічка +)";
                                }
                                else if (str.Contains("-"))
                                {
                                    lessonTitle = items[-1][d] + " (клінічка -)";
                                }
                                else if (str.Contains("*"))
                                {
                                    lessonTitle = items[-1][d] + " (клінічка *)";
                                }
                                else
                                {
                                    lessonTitle = items[-1][d];
                                }

                                foreach (var week in weeks)
                                {
                                    if (week.Group.Contains(groupNumber) && week.From == weeksDate[day.Key][0])
                                    {
                                        weekFound = true;
                                        break;
                                    }
                                }

                                if (weekFound == false)
                                {
                                    Week w = new Week()
                                    {
                                        Faculty = FacultyDetermine(groupNumber),
                                        Group = groupNumber,
                                        From = weeksDate[day.Key][0],
                                        To = weeksDate[day.Key][1],
                                        NumberWeek = (byte)day.Key,
                                        Course = course,
                                    };
                                    for (byte i = 1; i < (weeksDate[day.Key][1] - weeksDate[day.Key][0]).Days + 2; i++)
                                    {
                                        w.ScheduleDays.Add(new ScheduleDay() { DayNumber = i });
                                    }
                                    weeks.Add(w);
                                }

                                weeks.First(w => w.Group == groupNumber && w.Course == course && w.From == weeksDate[day.Key][0] && w.To == weeksDate[day.Key][1])
                                    .ScheduleDays.First(day => day.DayNumber == (numberDay <= countDayInWeek ? numberDay + 1 : numberDay - countDayInWeek))
                                    .Lessons.AddRange(new[] {
                                    new Lesson()
                                    {
                                        Name = lessonTitle,
                                        LessonNumber = 1
                                    },
                                    new Lesson()
                                    {
                                        Name = lessonTitle,
                                        LessonNumber = 2
                                    }
                                        }
                                    );
                            }

                        };
                    }
                }
            }

            await db.Weeks.AddRangeAsync(weeks);

            await db.SaveChangesAsync();

            return Content("Введені Вами дані опрацьовані.");
        }

        [NonAction]
        private List<DateTime[]> WeeksDate() // Тиждні
        {
            return new List<DateTime[]>()
            {
                new DateTime[2]{ new DateTime(2020,1,13), new DateTime(2020, 1, 17) },
                new DateTime[2]{ new DateTime(2020,1,20), new DateTime(2020, 1, 24) },
                new DateTime[2]{ new DateTime(2020,1,27), new DateTime(2020, 1, 31) },
                new DateTime[2]{ new DateTime(2020,2,3), new DateTime(2020, 2, 7) },
                new DateTime[2]{ new DateTime(2020,2,10), new DateTime(2020, 2, 14) },
                new DateTime[2]{ new DateTime(2020,2,17), new DateTime(2020, 2, 21) },
                new DateTime[2]{ new DateTime(2020,2,24), new DateTime(2020, 2, 28) },
                new DateTime[2]{ new DateTime(2020,3,2), new DateTime(2020, 3, 6) },
                new DateTime[2]{ new DateTime(2020,3,10), new DateTime(2020, 3, 13) },
                new DateTime[2]{ new DateTime(2020,3,16), new DateTime(2020, 3, 20) },
                new DateTime[2]{ new DateTime(2020,3,23), new DateTime(2020, 3, 27) },
                new DateTime[2]{ new DateTime(2020,3,30), new DateTime(2020, 4, 3) },
                new DateTime[2]{ new DateTime(2020,4,6), new DateTime(2020, 4, 10) },
                new DateTime[2]{ new DateTime(2020,4,13), new DateTime(2020, 4, 17) },
                new DateTime[2]{ new DateTime(2020,4,21), new DateTime(2020, 4, 24) },
                new DateTime[2]{ new DateTime(2020,4,27), new DateTime(2020, 4, 30) },
                new DateTime[2]{ new DateTime(2020,5,4), new DateTime(2020, 5, 8) },
                new DateTime[2]{ new DateTime(2020,5,12), new DateTime(2020, 5, 15) },
                new DateTime[2]{ new DateTime(2020,5,18), new DateTime(2020, 5, 22) },
                new DateTime[2]{ new DateTime(2020,5,25), new DateTime(2020, 5, 29) },
                new DateTime[2]{ new DateTime(2020,6,1), new DateTime(2020, 6, 5) },
                new DateTime[2]{ new DateTime(2020,6,9), new DateTime(2020, 6, 12) },
                new DateTime[2]{ new DateTime(2020,6,15), new DateTime(2020, 6, 19) },
                new DateTime[2]{ new DateTime(2020,6,22), new DateTime(2020, 6, 26) }
            };
        }

        [NonAction]
        private Faculty FacultyDetermine(string group)
        {
            switch (group.ToLower())
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "10":
                case "10а":
                case "10б":
                case "81":
                case "82":
                case "83":
                case "84": return Models.Faculty.Medicine;

                case "31":
                case "32": return Models.Faculty.Pediatric;

                case "91":
                case "92":
                case "93":
                case "94": return Models.Faculty.Ergoterapy;

                case "11":
                case "12":
                case "13":
                case "14":
                case "18":
                case "19": return Models.Faculty.Stomatology;

                case "41":
                case "42":
                case "43":
                case "49":
                case "61":
                case "62":
                case "63":
                case "64":
                case "65": return Models.Faculty.Pharmacology;

                case "21":
                case "21а":
                case "22": return Models.Faculty.DensTechnic;

                case "23":
                case "24":
                case "25":
                case "26": return Models.Faculty.Nurse;

                case "30":
                case "30а": return Models.Faculty.Feldsher;

                case "47":
                case "47а": return Models.Faculty.PharmacologyColedge;

                default: return Models.Faculty.PIZ;
            }
        }
    }
}