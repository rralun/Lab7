using Labo2.Models;
using Labo2.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Linq;

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
                Secret = "afgvhsbfjwlgeubfnjmkaksjdfhsjkadjfhsaj"
            });
        }

        [Test]
        public void ValidRegisterShouldCreateANewUser()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidRegisterShouldCreateANewUser))// "ValidRegisterShouldCreateANewUser")
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var usersService = new UsersService(context, config);
                var added = new Labo2.ViewModels.RegisterPostModel
                {
                    Email = "userTest@test.com",
                    FirstName = "User",
                    LastName = "Test",
                    Password = "1234567",
                    Username = "test_user"
                };
                var result = usersService.Register(added);

                Assert.IsNotNull(result);
                Assert.AreEqual(added.Username, result.Username);
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
                var usersService = new UsersService(context, config);
                var added = new Labo2.ViewModels.RegisterPostModel
                {
                    
                    FirstName = "User",
                    LastName = "Test",
                    Username = "test_user",
                    Email = "userTest@test.com",
                    Password = "1234567",
                };
                var added2 = new Labo2.ViewModels.RegisterPostModel
                {

                    FirstName = "User2",
                    LastName = "Test2",
                    Username = "test_user2",
                    Email = "userTest2@test.com",
                    Password = "123456789",
                };
                usersService.Register(added);
                usersService.Register(added2);
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
                var usersService = new UsersService(context, config);
                var added = new Labo2.ViewModels.RegisterPostModel
                {
                    Email = "userTest@test.com",
                    FirstName = "User",
                    LastName = "Test",
                    Password = "1234567",
                    Username = "test_user"
                };
                var result = usersService.Register(added);

                var authenticate = new Labo2.ViewModels.LoginPostModel
                {
                    Username = "test_user",
                    Password = "1234567",
                };
                var authenticateresult = usersService.Authenticate(added.Username, added.Password);

                Assert.IsNotNull(authenticateresult);
                Assert.AreEqual(1, authenticateresult.Id);
                Assert.AreEqual(authenticate.Username, authenticateresult.Username);
            }
        }
    }
}