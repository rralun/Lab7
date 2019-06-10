using Labo2.Models;
using Labo2.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labo2.Services
{
    public interface IUserRoleService
    {
        IEnumerable<UserRoleGetModel> GetAll();
        UserRoleGetModel GetById(int id);
        UserRoleGetModel Create(UserRolePostModel userRolePostModel, User currentUser);
        UserRoleGetModel Upsert(int id, UserRolePostModel userRolePostModel);
        UserRole Delete(int id);
    }

    public class UserRoleService : IUserRoleService
    {
        private ExpensesDbContext context;

        public UserRoleService(ExpensesDbContext context)
        {
            this.context = context;
        }


        public IEnumerable<UserRoleGetModel> GetAll()
        {
            return context
                .UserRoles
                .Select(userRol => UserRoleGetModel.FromUserRole(userRol));
        }

        public UserRoleGetModel GetById(int id)
        {
            UserRole userRole = context.UserRoles
                                    .AsNoTracking()
                                    .FirstOrDefault(ur => ur.Id == id);

            return UserRoleGetModel.FromUserRole(userRole);
        }


        public UserRoleGetModel Create(UserRolePostModel userRolePostModel, User currentUser)
        {
            UserRole toAdd = UserRolePostModel.ToUserRole(userRolePostModel);

            toAdd.AddedBy = currentUser;
            context.UserRoles.Add(toAdd);
            context.SaveChanges();
            return UserRoleGetModel.FromUserRole(toAdd);
        }


        public UserRoleGetModel Upsert(int id, UserRolePostModel userRolePostModel)
        {
            var existing = context.UserRoles.AsNoTracking().FirstOrDefault(ur => ur.Id == id);
            if (existing == null)
            {
                UserRole toAdd = UserRolePostModel.ToUserRole(userRolePostModel);
                context.UserRoles.Add(toAdd);
                context.SaveChanges();
                return UserRoleGetModel.FromUserRole(toAdd);
            }

            UserRole toUpdate = UserRolePostModel.ToUserRole(userRolePostModel);
            toUpdate.Id = id;
            context.UserRoles.Update(toUpdate);
            context.SaveChanges();
            return UserRoleGetModel.FromUserRole(toUpdate);
        }


        public UserRole Delete(int id)
        {
            var existing = context.UserRoles
                           .FirstOrDefault(ur => ur.Id == id);
            if (existing == null)
            {
                return null;
            }

            context.UserRoles.Remove(existing);
            context.SaveChanges();

            return (existing);
        }

    }
}
