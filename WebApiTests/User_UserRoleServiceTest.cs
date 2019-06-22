using Labo2.Models;
using Labo2.Services;
using Labo2.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Labo2.ViewModels;

namespace WebApiTests
{
    class User_UserRoleServiceTest
    {
        private IOptions<AppSettings> config;

        [SetUp]
        public void Setup()
        {
            config = Options.Create(new AppSettings
            {
                Secret = "My secret string for tests"
            });
        }
        [Test]
        public void CreateShouldAddTheUserUserRole()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateShouldAddTheUserUserRole))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var validator = new UserRoleValidator();
                var userUserRolesService = new User_UserRoleService(validator, context);

                User userToAdd = new User
                {
                    Email = "user@yahoo.com",
                    LastName = "Ion",
                    FirstName = "POpescu",
                    Password = "secret",
                    DataRegistered = DateTime.Now,
                    User_UserRoles = new List<User_UserRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRoleRegular = new UserRole
                {
                    Name = "Regular",
                    Description = "Creat pentru testare"
                };
                UserRole addUserRoleAdmin = new UserRole
                {
                    Name = "AdminDeTest",
                    Description = "Creat pentru testare"
                };
                context.UserRoles.Add(addUserRoleRegular);
                context.UserRoles.Add(addUserRoleAdmin);
                context.SaveChanges();

                context.User_UserRoles.Add(new User_UserRole
                {
                    User = userToAdd,
                    UserRole = addUserRoleRegular,
                    StartTime = DateTime.Parse("2019-06-13T00:00:00"),
                    EndTime = null
                });
                context.SaveChanges();

                
                var u_urpm = new User_UserRolePostModel
                {
                    UserId = userToAdd.Id,
                    UserRoleName = "Admin"
                };
                var result1 = userUserRolesService.Create(u_urpm);
                Assert.IsNotNull(result1);   //User role nu exista in baza de date dupa validare, ==> exista erori la validare

                
                var u_urpm1 = new User_UserRolePostModel
                {
                    UserId = userToAdd.Id,
                    UserRoleName = "AdminDeTest"
                };
                var result2 = userUserRolesService.Create(u_urpm1);
                Assert.IsNull(result2);   //User role exista si se face upsert
            }
        }


        [Test]
        public void GetByIdShouldReturnAValidUser_UserRole()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
           .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnAValidUser_UserRole))
           .Options;

            using (var context = new ExpensesDbContext(options))
            {

                var validator = new UserRoleValidator();
                User_UserRoleService user_userRoleService = new User_UserRoleService(validator, context);
                var added = new Labo2.ViewModels.User_UserRolePostModel
                {
                    UserId = 1,
                    UserRoleName = "Regular",
                    StartTime = DateTime.Now,
                    EndTime = null
                };

                var userById = user_userRoleService.GetById(added.UserId);
                Assert.IsNotNull(userById);
          
            }
        }
        [Test]
        public void GetUserRoleNameByIdShouldReturnTheUserName()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetUserRoleNameByIdShouldReturnTheUserName))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var validator = new UserRoleValidator();
                var user_userRoleService = new User_UserRoleService(validator, context);

                User added = new User
                {
                    Email = "userTest@test.com",
                    FirstName = "User",
                    LastName = "Test",
                    Password = "raluca1234",
                    Username = "test_user"
                };
                context.Users.Add(added);

                UserRole userRoleAdded = new UserRole
                {
                    Name = "NewUser",
                    Description = "New user for test"
                };
                context.UserRoles.Add(userRoleAdded);
                context.SaveChanges();

                context.User_UserRoles.Add(new User_UserRole
                {
                    User = added,
                    UserRole = userRoleAdded,
                    StartTime = DateTime.Now,
                    EndTime = null
                });
                context.SaveChanges();

                string userRoleName = user_userRoleService.GetUserRoleNameById(added.Id);
                Assert.AreEqual("NewUser", userRoleName);
                Assert.AreEqual("User", added.FirstName);

            }
        }
        
    }
}
