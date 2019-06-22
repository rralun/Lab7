using Labo2.Models;
using Labo2.Services;
using Labo2.Validators;
using Labo2.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiTests
{
    class UserRoleServiceTest
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
        public void ValidRegisterShouldCreateANewRole()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidRegisterShouldCreateANewRole))// "ValidRegisterShouldCreateANewUser")
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                UserRoleService userRoleService = new UserRoleService(context);
                var added = new Labo2.ViewModels.UserRolePostModel
                {
                    Name = "Regular",
                    Description = "regular description"

                };
                var result = userRoleService.Create(added);
                Assert.IsNotNull(result);


            }
        }


        [Test]
        public void ValidDeleteTest()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidDeleteTest))// "ValidRegisterShouldCreateANewUser")
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                UserRoleService userRoleService = new UserRoleService(context);
                var added = new Labo2.ViewModels.UserRolePostModel
                {
                    Name = "Regular",
                    Description = "regular description"

                };
                var result = userRoleService.Create(added);
                var resultDelete = userRoleService.Delete(result.Id);
                var resultNull = userRoleService.Delete(38743);
                Assert.IsNotNull(result);
                Assert.IsNull(resultNull);


            }
        }

        [Test]
        public void ValidGetAll()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidGetAll))// "ValidRegisterShouldCreateANewUser")
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                UserRoleService userRoleService = new UserRoleService(context);
                var added = new Labo2.ViewModels.UserRolePostModel
                {
                    Name = "Regular",
                    Description = "regular description"

                };
                var result = userRoleService.GetAll();

                Assert.IsNotNull(result);


            }
        }
        [Test]
        public void GetByIdShouldReturnAValidUserRole()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnAValidUserRole))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                UserRoleService userRoleService = new UserRoleService(context);
                var addUserRole = new UserRolePostModel()
                {
                    Name = "NewUser",
                    Description = "New user added"
                };

                var current = userRoleService.Create(addUserRole);
                var expected = userRoleService.GetById(current.Id);

                Assert.IsNotNull(expected);
                Assert.AreEqual(expected.Name, current.Name);
                Assert.AreEqual(expected.Id, current.Id);
            }
        }

        //[Test]
        //public void GetByIdShouldReturnAnValidUserRole()
        //{
        //      var options = new DbContextOptionsBuilder<ExpensesDbContext>()
        //     .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnAnValidUserRole))
        //     .Options;

        //    using (var context = new ExpensesDbContext(options))
        //    {

        //        UserRoleService userRoleService = new UserRoleService(context);
        //        var added = new Labo2.ViewModels.UserRolePostModel
        //        {
        //            Name = "Regular",
        //            Description = "regular description"
        //        };

        //        userRoleService.Create(added, null);
        //        var userById = userRoleService.GetById(1);

        //        Assert.NotNull(userById);
        //        Assert.AreEqual("Regular", userById.Name);

        //    }
        //}
        [Test]
        public void UpsertShouldModifyFieldsValues()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(UpsertShouldModifyFieldsValues))
            .Options;

            using (var context = new ExpensesDbContext(options))
            {
               
                var userRoleService = new UserRoleService(context);
                var expected = new UserRolePostModel
                {
                   Name = "Regular",
                   Description = "Regular for test"
                };

                userRoleService.Create(expected);

                var updated = new UserRolePostModel
                {
                    Name = "Admin",
                    Description = "Admin for test"

                };

                var userUpdated = userRoleService.Upsert(5, updated); //id 5 pt ca altfel imi da eroare de as no tracking

                Assert.NotNull(userUpdated);
                Assert.AreEqual("Admin", userUpdated.Name);

            }
        }
        [Test]
        public void UpsertShouldChangeTheFildValuesForRole()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(UpsertShouldChangeTheFildValuesForRole))
              .EnableSensitiveDataLogging()
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var userRoleService = new UserRoleService(context);
                var addUserRole = userRoleService.Create(new Labo2.ViewModels.UserRolePostModel
                {
                    Name = "Rol testare",
                    Description = "Creat pentru testare"
                });

                var userRole = context.UserRoles.Find(1);
                Assert.AreEqual(addUserRole.Name, userRole.Name);
            }
        }
    }
}
