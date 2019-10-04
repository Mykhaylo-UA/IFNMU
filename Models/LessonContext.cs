using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace IFNMUSiteCore.Models
{
    public class LessonContext : DbContext
    {
        public DbSet<Week> Weeks { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<ScheduleDay> ScheduleDays { get; set; }
        public DbSet<ThematicPlan> ThematicPlans { get; set; }
        public DbSet<MethodicalRecomendation> MethodicalRecomendations { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
             
        public LessonContext(DbContextOptions<LessonContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Lesson>()
                .HasOne(p => p.ThematicPlan)
                .WithMany(t => t.Lessons)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Answer>()
                .HasOne(p => p.Question)
                .WithMany(t => t.Answers)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}