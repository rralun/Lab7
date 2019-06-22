using Labo2.Models;
using Labo2.Services;
using Labo2.Validators;
using Labo2.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace WebApiTests
{
    class ExpenseServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }


        /// <summary>
        /// Test if an expense is added corectly.
        /// </summary>
        [Test]
        public void ValidExpenseShouldCreateANewExpenseAndGetById()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidExpenseShouldCreateANewExpenseAndGetById))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var expenseService = new ExpenseService(context);

                var expected = expenseService.Create(new Labo2.ViewModels.ExpensePostModel
                {
                    Description = "Expense for test",
                    Sum = 34,
                    Location = "Cluj",
                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                    Currency = "RON",
                    Type = "Other",
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            Text = "Comment test for expense",
                            Important = false,
                            AddedBy = null
                        }
                    }
                },null);

                var actual = expenseService.GetById(expected.Id);

                Assert.IsNotNull(actual);
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Test if an expense with invalid Type field is added.
        /// </summary>
        [Test]
        public void InvalidTypeOfExpenseShouldNotCreatANewExpense()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(InvalidTypeOfExpenseShouldNotCreatANewExpense))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var expenseService = new ExpenseService(context);

                var expected = expenseService.Create(new Labo2.ViewModels.ExpensePostModel
                {
                    Description = "Expense for test",
                    Sum = 34,
                    Location = "Cluj",
                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                    Currency = "RON",
                    Type = "Invalid Type",
                    Comments = null
                },null);

                var actual = expenseService.GetById(expected.Id);

                Assert.IsNull(actual);
            }
        }

        [Test]
        public void GetAllShouldReturnCorrectNumberOfPages()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPages))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var expenseService = new ExpenseService(context);
                var addedFlower = expenseService.Create(new Labo2.ViewModels.ExpensePostModel
                {
                    Description = "test",
                    Sum = 1.23,
                    Location = "cluj",
                    Date = new DateTime(),
                    Currency = "euro",
                   
                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "asd",
                            AddedBy = null
                        }
                    },

                }, null);

                var allComments = expenseService.GetAll(1);
                Assert.NotNull(allComments);
            }
        }

        [Test]
        public void DeleteAValidExpense()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(DeleteAValidExpense))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var expenseService = new ExpenseService(context);
                var expected = expenseService.Create(new ExpensePostModel
                {
                    Description = "test",
                    Sum = 1.23,
                    Location = "cluj",
                    Date = new DateTime(),
                    Currency = "euro",
                   
                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "asd",
                            AddedBy = null
                        }
                    },

                }, null);
                             

                var expenseDeleted = expenseService.Delete(expected.Id);
                var expenseDeletedNull = expenseService.Delete(33);
                Assert.IsNotNull(expenseDeleted);
                Assert.IsNull(expenseDeletedNull);
            }
        }
        [Test]
        public void UpsertShouldModifyExpenseWithTheGivenId()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(UpsertShouldModifyExpenseWithTheGivenId))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                var expenseService = new ExpenseService(context);
                var added = new ExpensePostModel()

                {
                    Description = "test",
                    Sum = 1.23,
                    Location = "cluj",
                    Date = new DateTime(),
                    Currency = "euro",

                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "asd",
                            AddedBy = null
                        }
                    },

                };

                var toAdd = expenseService.Create(added, null);
                var update = new ExpensePostModel()
                {
                    Description = "Updated"
                };

                var toUp = expenseService.Create(update, null);
                var updateResult = expenseService.Upsert(toUp.Id, toUp);


                Assert.IsNotNull(updateResult);
                Assert.AreEqual(toUp.Description, updateResult.Description);

            }
        }

        [Test]
        public void Update()
        {

            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(Update))
              .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
              .Options;
            using (var context = new ExpensesDbContext(options))
            {
                var expenseService = new ExpenseService(context);
                var expected = new ExpensePostModel() //expenseService.Create
                {
                    Description = "test",
                    Sum = 1.23,
                    Location = "cluj",
                    Date = DateTime.ParseExact("05/07/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                    Currency = "EURO",
                    Comments = new List<Comment>()
                        {
                            new Comment
                            {
                                Important = true,
                                Text = "asd",
                                AddedBy = null
                            }
                        },

                };//, null);
                var expectedExpenseForUpdate = new Expense
                {
                    Description = "update test",
                    Sum = 1.23,
                    Location = "brasov update",
                    Date = DateTime.ParseExact("05/09/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                    Currency = "EURO",
                    Comments = new List<Comment>()
                        {
                            new Comment
                            {
                                Important = true,
                                Text = "asd",
                                AddedBy = null
                            }
                        },
                    AddedBy = null
                };

                var updateResult = expenseService.Upsert(expected.Id, expectedExpenseForUpdate);
                Assert.IsNotNull(updateResult);
            }
        }

        [Test]
        public void CheckPaginatedListShouldReturnAllExpenseOnAPage()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CheckPaginatedListShouldReturnAllExpenseOnAPage))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {
                // declare service
                var expenseService = new ExpenseService(context);

                // add 6 expenses, for 2 pages with 5 expenses on a PaginatedList
                expenseService.Create(new Labo2.ViewModels.ExpensePostModel()
                {
                    Description = "Expense for test",
                    Sum = 34,
                    Location = "Cluj",
                    Date = DateTime.ParseExact("07/11/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                    Currency = "RON",
                    Type = "Utilities",
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            Text = "Comment test for expense",
                            Important = false
                        }
                    }
                }, null);
                expenseService.Create(new Labo2.ViewModels.ExpensePostModel()
                {
                    Description = "Expense2 for test",
                    Sum = 34,
                    Location = "Cluj",
                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                    Currency = "RON",
                    Type = "Utilities",
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            Text = "Comment2 test for expense",
                            Important = false
                        }
                    }
                },null);
                expenseService.Create(new Labo2.ViewModels.ExpensePostModel()
                {
                    Description = "Expense3 for test",
                    Sum = 34,
                    Location = "Cluj",
                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                    Currency = "USD",
                    Type = "Utilities",
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            Text = "Comment3 test for expense",
                            Important = false
                        }
                    }
                },null);
                expenseService.Create(new Labo2.ViewModels.ExpensePostModel()
                {
                    Description = "Expense4 for test",
                    Sum = 34,
                    Location = "Cluj",
                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                    Currency = "EURO",
                    Type = "Utilities",
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            Text = "Comment4 test for expense",
                            Important = false
                        }
                    }
                },null);
                expenseService.Create(new Labo2.ViewModels.ExpensePostModel()
                {
                    Description = "Expense5 for test",
                    Sum = 34,
                    Location = "Cluj",
                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                    Currency = "EURO",
                    Type = "Utilities",
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            Text = "Comment5 test for expense",
                            Important = false
                        }
                    }
                }, null);
                expenseService.Create(new Labo2.ViewModels.ExpensePostModel()
                {
                    Description = "Expense6 for test",
                    Sum = 34,
                    Location = "Oradea",
                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                    Currency = "EURO",
                    Type = "Utilities",
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            Text = "Comment6 test for expense",
                            Important = false
                        }
                    }
                }, null);


                // setez nr paginii
                int Page = 1;
                PaginatedList<ExpenseGetModel> actual =
                    expenseService.GetAll(Page,
                                                  DateTime.ParseExact("07/09/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                                                  DateTime.ParseExact("09/11/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                                                  Labo2.Models.Type.Utilities);

                // verific daca e ceva returnat si numarul de expenses pe pagina
                Assert.IsNotNull(actual);
                Assert.AreEqual(5, actual.Entries.Count);

                //  setez nr paginii
                Page = 2;
                PaginatedList<ExpenseGetModel> actual2 =
                    expenseService.GetAll(Page,
                                                  DateTime.ParseExact("07/09/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                                                  DateTime.ParseExact("09/11/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
                                                  Labo2.Models.Type.Utilities);

                // verific daca e ceva returnat si numarul de expenses pe pagina 2
                Assert.IsNotNull(actual2);
                Assert.AreEqual(1, actual2.Entries.Count);
            }
        }
    }
}
