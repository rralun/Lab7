using Labo2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labo2.ViewModels
{
    public class User_UserRolePostModel
    {
        public int UserId { get; set; }
        public string UserRoleName { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

    }
}
