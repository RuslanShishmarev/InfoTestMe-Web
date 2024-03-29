﻿using InfoTestMe.Common.Models;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace InfoTestMe.Admin.Web.Models.Data.Extensions
{
    public static class AuthorExtensions
    {
        public static AuthorDTO ToDTO(this Author author)
        {
            return new AuthorDTO()
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Email = author.Email,
                Password = author.Password,
                Description = author.Description,
                KeyWords = JsonConvert.DeserializeObject<string[]>(author.KeyWords),
                RegistrationDate = (DateTime)(author?.RegistrationDate),
                Image = author.Image,
                Courses = author.Courses?.Select(c => c.ToShortDTO()).ToList(),
                Tests = author.Tests?.Select(t => t.ToShortDTO()).ToList(),
            };
        }
        public static AuthorShortDTO ToShortDTO(this Author author)
        {
            return new AuthorShortDTO()
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Image = author.Image,
            };
        }
    }
}
