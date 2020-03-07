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
    [Authorize(Roles="moderator, admin")]
    public class ModeratorController:Controller
    {
        private LessonContext db; // контекст БД
        IWebHostEnvironment _appEnvironment;
        UserManager<User> _users;

        public ModeratorController(LessonContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager) // конструктор
        {
            db = context;
            _appEnvironment = appEnvironment;
            _users = userManager;
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<Chair> c = await db.Chairs.ToListAsync();
            Chair l = await db.Chairs.SingleOrDefaultAsync(a => a.Name.Contains("Тематичні"));
            Chair al = await db.Chairs.SingleOrDefaultAsync(a => a.Name.Contains("Методичні"));
            c.Remove(al);
            c.Remove(l);
            return View(c);
        }

        [HttpPost]
        public async Task<ActionResult> EditChair(int chairId)
        {
            var week = await db.Chairs.FindAsync(chairId);
            if(week != null)
            {
                return RedirectToAction("Chair", "Chair", new { id = week.Id });
            }
             return RedirectToAction("Moderator", "Index");
        }

        [HttpPost]
        public IActionResult EditWeek(int Course, string Group)
        {
            return RedirectToAction("EditWeek", "Schedule", new { course = Course, group = Group  });
        }

    }
}