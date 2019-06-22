using Labo2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Labo2.ViewModels
{
    public class UserPostModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserRoles { get; set; }
        public DateTime DataRegistered { get; set; }
       


        private static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            

            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        public static User ToUser(UserPostModel userModel)
        {
            ////UserRoles userRoles = Labo2.UserRoles.Regular;
            //if (userModel.UserRoles.Equals("Regular")) { }
            //else if (userModel.UserRoles.Equals( "UserManager")){}
            //else if (userModel.UserRoles.Equals("Admin")){}

            return new User
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Username = userModel.Username,
                Email = userModel.Email,
                Password = ComputeSha256Hash(userModel.Password),
                DataRegistered = userModel.DataRegistered
            };
        }
    }
}

