//using Labo2.Models;
//using Labo2.Validators;
//using Labo2.ViewModels;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace Labo2.Services
//{
//    public interface IUser_UserRolesService
//    {
//        IQueryable<User_UserRoleGetModel> GetById(int id);
//        ErrorsCollection Create(User_UserRolePostModel userUserRolePostModel);
//    }

//    public class User_UserRolesService : IUser_UserRolesService
//    {
//        private ExpensesDbContext context;
//        private IUserRoleValidator userRoleValidator;


//        public User_UserRolesService(IUserRoleValidator userRoleValidator, ExpensesDbContext context)
//        {
//            this.context = context;
//            this.userRoleValidator = userRoleValidator;
//        }

//        public IQueryable<User_UserRoleGetModel> GetById(int id)
//        {
//            IQueryable<User_UserRole> user_userRole = context.User_UserRoles
//                                    .Include(u => u.UserRole)
//                                    .AsNoTracking()
//                                    .Where(ur => ur.UserId == id)
//                                    .OrderBy(u_ur => u_ur.StartTime);

//            return user_userRole.Select(u_ur => User_UserRoleGetModel.FromUser_UserRole(u_ur));
//        }


//        public ErrorsCollection Create(User_UserRolePostModel user_userRolePostModel)
//        {
//            var errors = userRoleValidator.Validate(user_userRolePostModel, context);
//            if (errors != null)
//            {
//                return errors;
//            }

//            User user = context.Users
//                .FirstOrDefault(u => u.Id == user_userRolePostModel.UserId);

//            if (user != null)
//            {
//                UserRole userRole = context
//                               .UserRoles
//                               .Include(u_ur => u_ur.User_UserRoles)
//                               .FirstOrDefault(ur => ur.Name == user_userRolePostModel.UserRoleName);

//                User_UserRole currentUserUserRole = context
//                    .User_UserRoles
//                    .Include(ur => ur.UserRole)
//                    .FirstOrDefault(u_ur => u_ur.EndTime == null && u_ur.UserId == user.Id);

//                if (currentUserUserRole == null)
//                {
//                    context.User_UserRoles.Add(new User_UserRole
//                    {
//                        User = user,
//                        UserRole = userRole,
//                        StartTime = DateTime.Now,
//                        EndTime = null
//                    });

//                    context.SaveChanges();
//                    return null;
//                }
//                //inchiderea perioadel de activare pentru un anumit rol
//                if (!currentUserUserRole.UserRole.Name.Contains(user_userRolePostModel.UserRoleName))
//                {
//                    currentUserUserRole.EndTime = DateTime.Now;

//                    context.User_UserRoles.Add(new User_UserRole
//                    {
//                        User = user,
//                        UserRole = userRole,
//                        StartTime = DateTime.Now,
//                        EndTime = null
//                    });

//                    context.SaveChanges();
//                    return null;
//                }
//                else
//                {
//                    return null;    //trebuie sa trimit eroare ca modificarea nu poate avea loc, rol nou = rol vechi
//                }
//            }
//            return null;    //eroare Nu exista User cu Id-ul specificat

//        }


//    }
//}
