﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFNMUSiteCore.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LessonNumber { get; set; }

        public int? ThematicPlanId { get; set; }
        public ThematicPlan ThematicPlan { get; set; }

        public int? MethodicalRecomendationId { get; set; }
        public MethodicalRecomendation MethodicalRecomendation { get; set; }

        public int ScheduleDayId { get; set; }
        public ScheduleDay ScheduleDay { get; set; }

        public int? ChairId { get; set; }
        public Chair Chair { get; set; }

        public Lesson() { }
        public Lesson(string Name, int LessonNumber, int ThematicPlanId,  int MethodicalRecomendationId)
        {
            this.Name = Name;
            this.LessonNumber = LessonNumber;
            this.ThematicPlanId = ThematicPlanId;
            this.MethodicalRecomendationId = MethodicalRecomendationId;
        }
    }
}