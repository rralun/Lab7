using Labo2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labo2.ViewModels
{
    public class UserGetRoleModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }


        public static UserGetRoleModel FromUser(User user)
        {
            return new UserGetRoleModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,

            };
        }
    }
}
