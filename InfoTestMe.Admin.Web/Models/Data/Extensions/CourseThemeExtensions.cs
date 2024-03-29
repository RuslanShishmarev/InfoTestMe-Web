﻿using InfoTestMe.Common.Models;
using System.Linq;

namespace InfoTestMe.Admin.Web.Models.Data.Extensions
{
    public static class CourseThemeExtensions
    {
        public static CourseThemeDTO ToDTO(this CourseTheme courseTheme)
        {
            return new CourseThemeDTO()
            {
                Id = courseTheme.Id,
                CourseId = courseTheme.CourseId,
                Name = courseTheme.Name,
                Pages = courseTheme.Pages?.Select(c => c.ToShortDTO()).ToList(),
            };
        }
    }
}