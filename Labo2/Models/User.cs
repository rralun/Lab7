﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Labo2.Models
{
   
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Removed { get; set; }
        public DateTime DataRegistered { get; set; }
        public IEnumerable<User_UserRole> User_UserRoles { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
