﻿using System;
using System.Collections.Generic;

namespace InfoTestMe.WebAPI.Models.Data
{
    public class CourseTheme
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<CoursePage> Pages { get; set; }

        public Guid CourseId { get; set; }
        public Course Course { get; set; }
    }
}