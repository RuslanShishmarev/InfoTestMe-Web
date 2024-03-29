﻿using System;

namespace InfoTestMe.Admin.Web.Models.Abstractions
{
    public abstract class UserCommon
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public byte[] Image { get; set; }
    }
}
