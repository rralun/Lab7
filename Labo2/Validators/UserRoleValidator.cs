using Labo2.Models;
using Labo2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labo2.Validators
{
    public interface IUserRoleValidator
    {
        ErrorsCollection Validate(User_UserRolePostModel user_userRolePostModel, ExpensesDbContext context);
    }


    public class UserRoleValidator : IUserRoleValidator
    {
        public ErrorsCollection Validate(User_UserRolePostModel user_userRolePostModel, ExpensesDbContext context)
        {
            ErrorsCollection errorsCollection = new ErrorsCollection { Entity = nameof(UserRole) };

            var existing = context
                .UserRoles
                .FirstOrDefault(userRole1 => userRole1.Name == user_userRolePostModel.UserRoleName);
            if (existing == null)
            {
                errorsCollection.ErrorMessages.Add("The role does not exist !");
            }
            if (errorsCollection.ErrorMessages.Count > 0)
            {
                return errorsCollection;
            }
            return null;
            //List<string> userRoles = context.UserRoles
            //    .Select(userRole => userRole.Name)
            //    .ToList();

            //if (!userRoles.Contains(user_userRolePostModel.UserRoleName))
            //{
            //    errorsCollection.ErrorMessages.Add($"The userRole {user_userRolePostModel.UserRoleName} does not exists in Db!");
            //}

            //if (errorsCollection.ErrorMessages.Count > 0)
            //{
            //    return errorsCollection;
            //}
            //return null;
        }
    }
}
