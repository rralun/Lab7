using Labo2.Models;
using Labo2.Validators;
using Labo2.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Labo2.Services
{
    public interface IUser_UserRoleService
    {
        IQueryable<User_UserRoleGetModel> GetById(int id);
        ErrorsCollection Create(User_UserRolePostModel userUserRolePostModel);

        string GetUserRoleNameById(int id);
    }

    public class User_UserRoleService : IUser_UserRoleService
    {
        private ExpensesDbContext context;
        private IUserRoleValidator userRoleValidator;


        public User_UserRoleService(IUserRoleValidator userRoleValidator, ExpensesDbContext context)
        {
            this.context = context;
            this.userRoleValidator = userRoleValidator;
        }

        public IQueryable<User_UserRoleGetModel> GetById(int id)  //aici iau history pt rol
        {
            IQueryable<User_UserRole> user_userRole = context.User_UserRoles
                                    .Include(u => u.UserRole)
                                    .AsNoTracking()
                                    .Where(ur => ur.UserId == id)
                                    .OrderBy(u_ur => u_ur.StartTime);

            return user_userRole.Select(u_ur => User_UserRoleGetModel.FromUser_UserRole(u_ur));
        }


        public ErrorsCollection Create(User_UserRolePostModel user_userRolePostModel)
        {
            var errors = userRoleValidator.Validate(user_userRolePostModel, context);
            if (errors != null)
            {
                return errors;
            }

            User user = context.Users
                .FirstOrDefault(u => u.Id == user_userRolePostModel.UserId);

            if (user != null)
            {
                UserRole userRole = context
                               .UserRoles
                               .Include(u_ur => u_ur.User_UserRoles)
                               .FirstOrDefault(ur => ur.Name == user_userRolePostModel.UserRoleName);

                User_UserRole currentUser_UserRole = context
                    .User_UserRoles
                    .Include(ur => ur.UserRole)
                    .FirstOrDefault(u_ur => u_ur.EndTime == null && u_ur.UserId == user.Id);

                if (currentUser_UserRole == null)
                {
                //    context.User_UserRoles.Add(new User_UserRole
                //    {
                //        User = user,
                //        UserRole = userRole,
                //        StartTime = DateTime.Now,
                //        EndTime = null
                //    });

                //    context.SaveChanges();
                    return null;
                }
                // aici se inchide perioada de activare pentru un anumit rol
                if (!currentUser_UserRole.UserRole.Name.Contains(user_userRolePostModel.UserRoleName))
                {
                    currentUser_UserRole.EndTime = DateTime.Now;

                    context.User_UserRoles.Add(new User_UserRole
                    {
                        User = user,
                        UserRole = userRole,
                        StartTime = DateTime.Now,
                        EndTime = null
                    });

                    context.SaveChanges();
                    return null;
                }
                else
                {
                    return null;    
                }
            }
            return null;    
        }
        public string GetUserRoleNameById(int id)
        {
            int userRoleId = context.User_UserRoles
               .AsNoTracking()
                .FirstOrDefault(u_ur => u_ur.UserId == id && u_ur.EndTime == null)
                .UserRoleId;

            string roleName = context.UserRoles
                  .AsNoTracking()
                  .FirstOrDefault(ur => ur.Id == userRoleId)
                  .Name;

            return roleName;
        }

    }
}
