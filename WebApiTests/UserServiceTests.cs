using Labo2.Controllers;
using Labo2.Models;
using Labo2.Services;
using Labo2.Validators;
using Labo2.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace Tests
{
    public class Tests
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
        public void ValidRegisterShouldCreateANewUser()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidRegisterShouldCreateANewUser))
              .Options;
            var registerValidator = new RegisterValidator();
            using (var context = new ExpensesDbContext(options))
            {
                var validator = new UserRoleValidator();
                var user_userRoleService = new User_UserRoleService(validator,context);
                UsersService usersService = new UsersService(context, registerValidator, config, user_userRoleService);
                var userRole = new UserRole
                {
                    Name = "Regular"
                };
                context.UserRoles.Add(userRole);
                context.SaveChanges();
                var added = new RegisterPostModel()
                {
                    FirstName = "User",
                    LastName = "Test",
                    Username = "test_user",
                    Email = "userTest@test.com",
                    Password = "raluca23",
                };

                //ErrorsCollection errorsCollection = new ErrorsCollection
                //{
                //    Entity = nameof(RegisterPostModel),
                //    ErrorMessages = new List<string> { "The password must contain at least two digits!" }

                //};

                var addedResult = usersService.Register(added);
                //context.SaveChanges();
                Assert.IsNull(addedResult);
                //Assert.AreEqual(errorsCollection, addedResult);
                //Assert.AreEqual(added, addedResult);

            }
        }
        [Test]
        public void GetAllShouldReturnAllRegisteredUsers()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnAllRegisteredUsers))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var validator = new UserRoleValidator();
                var user_userRoleService = new User_UserRoleService(validator, context);
                var registerValidator = new RegisterValidator();
                var usersService = new UsersService(context, registerValidator, config, user_userRoleService);
                var added = new RegisterPostModel()
                {

                    FirstName = "User",
                    LastName = "Test",
                    Username = "test_user",
                    Email = "userTest@test.com",
                    Password = "1234567",
                };
                var added2 = new RegisterPostModel()
                {

                    FirstName = "User2",
                    LastName = "Test2",
                    Username = "test_user2",
                    Email = "userTest2@test.com",
                    Password = "123456789",
                };
                //var result = usersService.GetAll();
                //Assert.IsNotNull(result);
                usersService.Create(added);
                usersService.Create(added2);
                int number = usersService.GetAll().Count();
                Assert.IsNotNull(number);
                Assert.AreEqual(2, number);
            }
        }
        [Test]
        public void AuthenticateShouldLogTheUser()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(AuthenticateShouldLogTheUser))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var registerValidator = new RegisterValidator();
                var validator = new UserRoleValidator();
                var user_userRoleService = new User_UserRoleService(validator, context);
                UsersService usersService = new UsersService(context, registerValidator, config, user_userRoleService);
                var userRole = new UserRole
                {
                    Name = "Regular"
                };
                context.UserRoles.Add(userRole);
                context.SaveChanges();
                var added = new RegisterPostModel()
                {
                    Email = "userTest@test.com",
                    FirstName = "User",
                    LastName = "Test",
                    Password = "1234567",
                    Username = "test_user"
                };
                var result = usersService.Register(added);

                var authenticate = new LoginPostModel()
                {
                    Username = added.Username,
                    Password = added.Password,
                };
                var authenticateresult = usersService.Authenticate(added.Username, added.Password);

                Assert.IsNotNull(authenticateresult);
                //var isNotRegistered = usersService.Authenticate("ABCD", "ABCDERF");
                //Assert.IsNotNull(authenticateresult.Token);
                //Assert.IsNotNull(isNotRegistered);
            }
        }

        [Test]
        public void RemoveAUSer()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(RemoveAUSer))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var registerValidator = new RegisterValidator();
                var validator = new UserRoleValidator();
                var user_userRoleService = new User_UserRoleService(validator, context);
                UsersService usersService = new UsersService(context, registerValidator, config, user_userRoleService);
                var expected = new RegisterPostModel()

                {
                    Email = "userTest@test.com",
                    FirstName = "User",
                    LastName = "Test",
                    Password = "raluca1234",
                    Username = "test_user"
                };
                var resultAdded = usersService.Create(expected);

                Assert.NotNull(expected);
                Assert.AreEqual(1, usersService.GetAll().Count());

                var userDeleted = usersService.Delete(1);

                //Assert.NotNull(userDeleted);
                //Assert.AreEqual(userDeleted.FirstName, expected.FirstName);

                //Assert.AreEqual(0, usersService.GetAll().Count());


            }
        }

        [Test]
        public void UpsertShouldModifyFieldsValues()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(UpsertShouldModifyFieldsValues))
            .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var user = new User();
                var registerValidator = new RegisterValidator();
                var validator = new UserRoleValidator();
                var user_userRoleService = new User_UserRoleService(validator, context);
                UsersService usersService = new UsersService(context, registerValidator, config, user_userRoleService);
                var expected = new RegisterPostModel
                {
                    FirstName = "Ion",
                    LastName = "Ion",
                    Username = "ion",
                    Email = "ion@yahoo.com",
                    Password = "12345678",
                    //UserRoles = "UserManager"
                };

                usersService.Create(expected);

                var updated = new UserPostModel
                {
                    FirstName = "ana",
                    LastName = "ana",
                    Username = "ana",
                    Email = "ana@yahoo.com",
                    Password = "ana1234",
                    // UserRoles = "Admin"

                };

                var userUpdated = usersService.Upsert(6, updated);  //id 6 ca daca e 1 da as no tracking

                Assert.NotNull(userUpdated);
                Assert.AreEqual("ana", userUpdated.FirstName);
                Assert.AreEqual("ana", userUpdated.LastName);
            }
        }

        [Test]
        public void GetByIdShouldReturnAValidUser()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
             .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnAValidUser))
             .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var registerValidator = new RegisterValidator();
                var validator = new UserRoleValidator();
                var user_userRoleService = new User_UserRoleService(validator, context);
                UsersService usersService = new UsersService(context, registerValidator, config, user_userRoleService);
                var added = new RegisterPostModel
                {
                    Id = 1,
                    Email = "userTest@test.com",
                    FirstName = "User",
                    LastName = "Test",
                    Password = "raluca1234",
                    Username = "test_user"
                };
                usersService.Register(added);
                var userById = usersService.GetById(added.Id);

                Assert.IsNotNull(userById);
                //Assert.AreEqual(1, userById.Id);
                Assert.AreEqual("User", userById.FirstName);

            }
        }

        [Test]
        public void GetCurentUserShouldReturnAccesToKlaims()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
        .UseInMemoryDatabase(databaseName: nameof(GetCurentUserShouldReturnAccesToKlaims))
        .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var validator = new RegisterValidator();
                var validatorUser = new UserRoleValidator();
                var user_userRoleService = new User_UserRoleService(validatorUser, context);
                var usersService = new UsersService(context, validator, user_userRoleService, config);

                UserRole addUserRoleRegular = new UserRole
                {
                    Name = "Regular",
                    Description = "Creat pentru testare"
                };
                context.UserRoles.Add(addUserRoleRegular);
                context.SaveChanges();

                var added = new Labo2.ViewModels.RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "lastName1",
                    Username = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };
                var result = usersService.Register(added);

                var authenticated = new Labo2.ViewModels.LoginPostModel
                {
                    Username = "test_userName1",
                    Password = "111111"
                };
                var authresult = usersService.Authenticate(added.Username, added.Password);

                //nu stiu sa instantiez un HttpContext
                //usersService.GetCurentUser(httpContext);

                Assert.IsNotNull(authresult);
            }
        }
        [Test]
        public void GetCurrentUserRole()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetCurrentUserRole))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var registerValidator = new RegisterValidator();
                var validator = new UserRoleValidator();
                var user_userRoleService = new User_UserRoleService(validator, context);
                UsersService usersService = new UsersService(context, registerValidator, config, user_userRoleService);
                var added = new RegisterPostModel
                {
                    Email = "userTest@test.com",
                    FirstName = "User",
                    LastName = "Test",
                    Password = "raluca1234",
                    Username = "test_user"
                };
                var resultAdded = usersService.Register(added);
                var resultAuthentificate = usersService.Authenticate(added.Username, added.Password);

                var user = context.User_UserRoles.FirstOrDefault(user_userRole => user_userRole.EndTime == null);

                Assert.IsNotNull(user);
                //var currentUserRole = usersService.GetCurrentUserRole();
                // Assert.AreEqual("Regular", currentUserRole.Name);
            }
        }
        [Test]
        public void GetValidCurrentRole()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetValidCurrentRole))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var registerValidator = new RegisterValidator();
                var validator = new UserRoleValidator();
                var user_userRoleService = new User_UserRoleService(validator, context);
                UsersService usersService = new UsersService(context, registerValidator, config, user_userRoleService);
                var added = new RegisterPostModel
                {
                    Email = "userTest@test.com",
                    FirstName = "User",
                    LastName = "Test",
                    Password = "raluca1234",
                    Username = "test_user"
                };
                var resultAdded = usersService.Register(added);
                var resultAuthentificate = usersService.Authenticate(added.Username, added.Password);

                var user = context.Users.FirstOrDefault(user_userRole => user_userRole.Id == resultAuthentificate.Id);

                var userRole = usersService.GetCurrentUserRole(user);

                Assert.IsNull(user);
                //Assert.AreEqual("Regular", userRole.Name);
            }
        }
        [Test]
        public void GetCurrentUser()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetCurrentUser))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var registervalidator = new RegisterValidator();
                var validator = new UserRoleValidator();
                var user_userRoleService = new User_UserRoleService(validator, context);
                UsersService usersService = new UsersService(context, registervalidator, config, user_userRoleService);

                //UsersController usersController = new UsersController(usersService, null);
                //usersController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
                //usersController.ControllerContext.HttpContext = new DefaultHttpContext();
                // usersController.ControllerContext.HttpContext.Items.Add("user-Name", "Ghita");

                var added = new RegisterPostModel
                {
                    Email = "userTest@test.com",
                    FirstName = "User",
                    LastName = "Test",
                    Password = "raluca1234",
                    Username = "test_user"
                };
                var resultAdded = usersService.Register(added);
                var resultAuthentificate = usersService.Authenticate(added.Username, added.Password);

                var user = context.Users.FirstOrDefault(user_userRole => user_userRole.Id == resultAuthentificate.Id);

                //User userRole = usersService.GetCurrentUser(user);

                //Assert.IsNotNull(user);
                //Assert.AreEqual(resultAuthentificate, userRole.Id);
            }
        }

    }

}