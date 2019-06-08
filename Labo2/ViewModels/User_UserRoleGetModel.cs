using Labo2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labo2.ViewModels
{
    public class User_UserRoleGetModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserRoleId { get; set; }

        public string UserRoleName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }


        public static User_UserRoleGetModel FromUser_UserRole(User_UserRole user_userRole)
        {
            return new User_UserRoleGetModel
            {
                Id = user_userRole.Id,
                UserId = user_userRole.UserId,
                UserRoleId = user_userRole.UserRoleId,
                UserRoleName = user_userRole.UserRole.Name,
                StartTime = user_userRole.StartTime,
                EndTime = user_userRole.EndTime
            };
        }
    }
}
