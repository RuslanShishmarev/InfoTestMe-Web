﻿using InfoTestMe.Admin.Web.Models.Abstractions;
using InfoTestMe.Admin.Web.Models.Data;
using InfoTestMe.Admin.Web.Models.Data.Extensions;
using InfoTestMe.Common.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace InfoTestMe.Admin.Web.Services
{
    public class UserService : CommonService<UserDTO>, IUserService
    {
        public UserService(InfoTestMeDataContext db) : base(db) { }

        #region PRIVATE METHODS
        private User GetUser(string login, string password)
        {
            var user = DB.Users.FirstOrDefault(u => u.Email == login && u.Password == password);
            return user;
        }

        private User GetUser(int id)
        {
            var user = DB.Users.Find(id);
            return user;
        }

        private void CreateUser(UserDTO dto)
        {
            User newUser = new User()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
                Image = dto.Image,
                RegistrationDate = DateTime.Now
            };
            DB.Users.Add(newUser);
        }

        private void UpdateUser(UserDTO dto)
        {
            User user = GetUser(dto.Id);

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Password = dto.Password;
            user.Image = dto.Image;

            DB.Users.Update(user);
        }

        private void DeleteUser(int id)
        {
            User user = GetUser(id);
            DB.Users.Remove(user);
        }

        public void AddUserToCourse(int userId, int courseId)
        {
            User user = GetUser(userId);

            Course course = DB.Courses.Find(courseId);

            if (user != null && course != null)
            {
                course.Users.Add(user);
            }
        }

        public void AddUserToCourse(User user, int courseId)
        {
            Course course = DB.Courses.Find(courseId);

            if (user != null && course != null)
            {
                course.Users.Add(user);
            }
        }

        public void RemoveUserFromCourse(User user, int courseId)
        {
            Course course = DB.Courses.Find(courseId);

            if (user != null && course != null)
            {
                course.Users.Remove(user);
            }
        }
        #endregion

        public (string userName, string userPassword) GetUserLoginPassFromBasicAuth(HttpRequest request)
        {
            string userName = "";
            string userPass = "";
            string authHeader = request.Headers["Authorization"].ToString();
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUserNamePass = authHeader.Replace("Basic ", "");
                var encoding = Encoding.GetEncoding("iso-8859-1");

                string[] namePassArray = encoding.GetString(Convert.FromBase64String(encodedUserNamePass)).Split(':');
                userName = namePassArray[0];
                userPass = namePassArray[1];
            }
            return (userName, userPass);
        }

        public ClaimsIdentity GetIdentity(string username, string password)
        {
            User currentUser = GetUser(username, password);
            if (currentUser != null)
            {
                currentUser.LastLoginDate = DateTime.Now;
                DB.Users.Update(currentUser);
                DB.SaveChanges();

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, currentUser.Email),
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
        
        public UserDTO Get(int id)
        {
            return DB.Users.FirstOrDefault(u => u.Id == id)?.ToDTO();
        }        

        public bool Create(UserDTO dto)
        {
            return CreateOrUpdateActionData(CreateUser, dto);
        }

        public bool Update(UserDTO dto)
        {
            return CreateOrUpdateActionData(UpdateUser, dto);
        }

        public bool Delete(int id)
        {
            return DeleteActionData(DeleteUser, id);
        }

        public List<UserShortDTO> GetByRange(int startPosition, int countModels)
        {
            List<UserShortDTO> userDtos = new List<UserShortDTO>();

            int allCount = DB.Users.Count();

            if (allCount <= startPosition)
            {
                return userDtos;
            }
            else if (allCount < startPosition + countModels)
            {
                countModels = allCount - startPosition;
            }

            userDtos = DB.Users.ToList().GetRange(startPosition, countModels).Select(u => u.ToShortDTO()).ToList();
            return userDtos;
        }

        public bool EnterToCourse(int userId, int courseId)
        {
            try
            {
                AddUserToCourse(userId, courseId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EnterToCourse(User user, int courseId)
        {
            try
            {
                AddUserToCourse(user, courseId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public User GetUserByLogin(string login)
        {
            return DB.Users.FirstOrDefault(u => u.Email == login);
        }

        public bool OutCourse(User user, int courseId)
        {
            try
            {
                RemoveUserFromCourse(user, courseId);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
