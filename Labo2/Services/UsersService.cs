using Labo2.Models;
using Labo2.Validators;
using Labo2.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Labo2.Services
{
    public interface IUsersService
    {
        LoginGetModel Authenticate(string username, string password);
        ErrorsCollection Register(RegisterPostModel registerinfo);
        User GetCurrentUser(HttpContext httpContext);

        IEnumerable<UserGetModel> GetAll();
        UserGetModel GetById(int id);
        UserGetModel Create(UserPostModel userModel);
        User Upsert(int id, User user, User userCurrentLogin);
        //UserGetModel Upsert(int id, UserPostModel userPostModel);
        User Delete(int id);
        UserGetRoleModel RoleChanges(int id, string Role, User currentUser);
        IEnumerable<User_UserRoleGetModel> GetHistoryOfARole(int id);
    }

    public class UsersService : IUsersService
    {
        private ExpensesDbContext context;
        private readonly AppSettings appSettings;
        private IRegisterValidator registerValidator;

        public UsersService(ExpensesDbContext context, IRegisterValidator registerValidator, IOptions<AppSettings> appSettings)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
            this.registerValidator = registerValidator;
        }


        public LoginGetModel Authenticate(string username, string password)
        {
            var user = context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == ComputeSha256Hash(password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    //new Claim(ClaimTypes.Role, user.UserRole.ToString()),        //rolul vine ca string
                    new Claim(ClaimTypes.UserData, user.DataRegistered.ToString())        //DataRegistered vine ca string
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var result = new LoginGetModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Token = tokenHandler.WriteToken(token)
            };
            //first remove pw 
            return result;
        }


        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            //TODO: Also use salt

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


        public ErrorsCollection Register(RegisterPostModel registerinfo)
        {

            var errors = registerValidator.Validate(registerinfo, context);
            if (errors != null)
            {
                return errors;
            }

            User toAdd = new User
            {
                Email = registerinfo.Email,
                LastName = registerinfo.LastName,
                FirstName = registerinfo.FirstName,
                Password = ComputeSha256Hash(registerinfo.Password),
                Username = registerinfo.Username,
                DataRegistered = DateTime.Now,
                User_UserRoles = new List<User_UserRole>()
            };

            var regularRole = context
                .UserRoles
                .FirstOrDefault(ur => ur.Name == UserRoles.Regular);

            context.Users.Add(toAdd);
            context.User_UserRoles.Add(new User_UserRole
            {
                User = toAdd,
                UserRole = regularRole,
                StartTime = DateTime.Now,
                EndTime = null
            });

            context.SaveChanges();
            return null;
        }


        public UserRole GetCurrentUserRole(User user)
        {
            return user
                .User_UserRoles
                .FirstOrDefault(user_userRole => user_userRole.EndTime == null)
                .UserRole;
        }

        public User GetCurrentUser(HttpContext httpContext)
        {
            string username = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            //string accountType = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.AuthenticationMethod).Value;
            //return _context.Users.FirstOrDefault(u => u.Username == username && u.AccountType.ToString() == accountType);

            return context
                .Users
                .Include(u => u.User_UserRoles)
                .FirstOrDefault(u => u.Username == username);
        }



        //IMPLEMENTARE CRUD PENTRU USER

        public IEnumerable<UserGetModel> GetAll()
        {
            //return context.Users.Select(user => new UserGetModelWithRole
            //{
            //    Id = user.Id,
            //    Email = user.Email,
            //    Username = user.Username,

            //});
            return context.Users.Select(user => UserGetModel.FromUser(user));
        }

        public UserGetModel GetById(int id)
        {
            User user = context.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == id);

            return UserGetModel.FromUser(user);
        }

        public UserGetModel Create(UserPostModel userModel)
        {
            User toAdd = UserPostModel.ToUser(userModel);

            context.Users.Add(toAdd);
            context.SaveChanges();
            return UserGetModel.FromUser(toAdd);

        }

        public UserGetRoleModel RoleChanges(int id, string Role, User userCurrentLogin)
        {
            DateTime dateCurrent = DateTime.Now;
            TimeSpan diferenta = dateCurrent.Subtract(userCurrentLogin.DataRegistered);
            var currentUserRole = GetCurrentUserRole(userCurrentLogin);
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            var userRoleFromUserToChange = GetCurrentUserRole(user);
            if ((currentUserRole.Name == "Admin" || diferenta.Days > 190) && userRoleFromUserToChange.Name != "Admin")
            {

                var userActiveRole = user.User_UserRoles.FirstOrDefault(role => role.EndTime == null);
                userActiveRole.EndTime = DateTime.Now;
                var regularRole = context
                   .UserRoles
                   .FirstOrDefault(ur => ur.Name == Role);
                if (regularRole != null)
                {
                    context.User_UserRoles.Add(new User_UserRole
                    {
                        User = user,
                        UserRole = regularRole,
                        StartTime = DateTime.Now,
                        EndTime = null,
                    });

                }
            }

            return UserGetRoleModel.FromUser(user);

        }

        public IEnumerable<User_UserRoleGetModel> GetHistoryOfARole(int id)
        {
            
            var result = context
                .User_UserRoles
                .Select(user_userRole => new User_UserRoleGetModel
            {
                User = user_userRole.User,
                UserRole = user_userRole.UserRole,
                StartTime = user_userRole.StartTime,
                EndTime = user_userRole.EndTime,

            }).Where(u => u.User.Id == id).OrderBy(us => us.StartTime).ToList();

            return result;
        }



        public User Upsert(int id, User user, User userCurrentLogin)//UserGetModel Upsert(int id, UserPostModel userPostModel)
        {
            var existing = context
                .Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == id);
            if (existing == null)
            {
                context.
                    Users.Add(user);
                context.SaveChanges();
                return user;
            }
            DateTime currentDate = DateTime.Now;
            TimeSpan diferenta = currentDate.Subtract(userCurrentLogin.DataRegistered);

            user.Id = id;
            var userCurentRole = GetCurrentUserRole(userCurrentLogin);
            var curentRoleForExisting = GetCurrentUserRole(existing);
            if ((userCurentRole.Name == "Admin" || diferenta.Days > 190) && curentRoleForExisting.Name != "Admin")
            {
                var getUserRole = GetCurrentUserRole(user);
                RoleChanges(user.Id, getUserRole.Name, existing);
                context.Users.Update(user);

                context.SaveChanges();
                return user;
            }
            var getUserRoleNotChangeRole = GetCurrentUserRole(user);
            var getExistingRole = GetCurrentUserRole(existing);
            RoleChanges(user.Id, getExistingRole.Name, existing);

            context.Users.Update(user);
            context.SaveChanges();
            return user;
            //var existing = context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
            //if (existing == null)
            //{
            //    User toAdd = UserPostModel.ToUser(userPostModel);
            //    context.Users.Add(toAdd);
            //    context.SaveChanges();
            //    return UserGetModel.FromUser(toAdd);
            //}

            //User toUpdate = UserPostModel.ToUser(userPostModel);
            //toUpdate.Id = id;
            //context.Users.Update(toUpdate);
            //context.SaveChanges();
            //return UserGetModel.FromUser(toUpdate);
        }


        public User Delete(int id)
        {
            var existing = context.Users
            .FirstOrDefault(user => user.Id == id);
            if (existing == null)
            {
                return null;
            }
            existing.Removed = true;
            context.Update(existing);
            context.SaveChanges();
            return existing;
        }

    }
}

