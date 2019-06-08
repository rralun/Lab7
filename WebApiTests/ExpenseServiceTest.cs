//using Labo2.Models;
//using Labo2.Services;
//using Labo2.Validators;
//using Labo2.ViewModels;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Text;

//namespace WebApiTests
//{
//    class ExpenseServiceTest
//    {
//        [SetUp]
//        public void Setup()
//        {
//        }
        

//        /// <summary>
//        /// Test if an expense is added corectly.
//        /// </summary>
//        [Test]
//        public void ValidExpenseShouldCreateANewExpense()
//        {
//            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
//              .UseInMemoryDatabase(databaseName: nameof(ValidExpenseShouldCreateANewExpense))
//              .Options;

//            using (var context = new ExpensesDbContext(options))
//            {
//                var expenseService = new ExpenseService(context);

//                var expected = new ExpensePostModel
//                {
//                    Description = "Expense for test",
//                    Sum = 34,
//                    Location = "Cluj",
//                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                    Currency = "RON",
//                    Type = "Utilities",
//                    Comments = new List<Comment>()
//                    {
//                        new Comment()
//                        {
//                            Text = "Comment test for expense",
//                            Important = false
//                        }
//                    }
//                };

//                var actual = expenseService.Create(expected);

//                Assert.IsNotNull(actual);
//                Assert.AreEqual(expected.Description, actual.Description);
//                Assert.AreEqual(expected.Sum, actual.Sum);
//                Assert.AreEqual(expected.Location, actual.Location);
//                Assert.AreEqual(expected.Date, actual.Date);
//                Assert.AreEqual(expected.Currency, actual.Currency);
//                Assert.AreEqual(expected.Type, actual.Type.ToString());
//                Assert.AreEqual(expected.Comments, actual.Comments);
//            }
//        }

//        /// <summary>
//        /// Test if an expense with invalid Type field is added.
//        /// </summary>
//        [Test]
//        public void InvalidTypeOfExpenseShouldNotCreatANewExpense()
//        {
//            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
//              .UseInMemoryDatabase(databaseName: nameof(InvalidTypeOfExpenseShouldNotCreatANewExpense))
//              .Options;

//            using (var context = new ExpensesDbContext(options))
//            {
//                var expenseService = new ExpenseService(context);

//                var expected = new ExpensePostModel()
//                {
//                    Description = "Expense for test",
//                    Sum = 34,
//                    Location = "Cluj",
//                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                    Currency = "RON",
//                    Type = "Invalid Type",
//                    Comments = null
//                };

//                var actual = expenseService.Create(expected);

//                Assert.IsNull(actual);
//            }
//        }

       
//        [Test]
//        public void CheckPaginatedListShouldReturnAllExpenseOnAPage()
//        {
//            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
//              .UseInMemoryDatabase(databaseName: nameof(CheckPaginatedListShouldReturnAllExpenseOnAPage))
//              .Options;

//            using (var context = new ExpensesDbContext(options))
//            {
//                // declare service
//                var expenseService = new ExpenseService(context);

//                // add 4 expenses, for 2 pages with 3 expenses on a PaginatedList
//                expenseService.Create(new ExpensePostModel()
//                {
//                    Description = "Expense for test",
//                    Sum = 34,
//                    Location = "Cluj",
//                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                    Currency = "RON",
//                    Type = "Utilities",
//                    Comments = new List<Comment>()
//                    {
//                        new Comment()
//                        {
//                            Text = "Comment test for expense",
//                            Important = false
//                        }
//                    }
//                });
//                expenseService.Create(new ExpensePostModel()
//                {
//                    Description = "Expense2 for test",
//                    Sum = 34,
//                    Location = "Cluj",
//                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                    Currency = "RON",
//                    Type = "Utilities",
//                    Comments = new List<Comment>()
//                    {
//                        new Comment()
//                        {
//                            Text = "Comment2 test for expense",
//                            Important = false
//                        }
//                    }
//                });
//                expenseService.Create(new ExpensePostModel()
//                {
//                    Description = "Expense3 for test",
//                    Sum = 34,
//                    Location = "Cluj",
//                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                    Currency = "USD",
//                    Type = "Other",
//                    Comments = new List<Comment>()
//                    {
//                        new Comment()
//                        {
//                            Text = "Comment3 test for expense",
//                            Important = false
//                        }
//                    }
//                });
//                expenseService.Create(new ExpensePostModel()
//                {
//                    Description = "Expense4 for test",
//                    Sum = 34,
//                    Location = "Cluj",
//                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                    Currency = "EURO",
//                    Type = "Groceries",
//                    Comments = new List<Comment>()
//                    {
//                        new Comment()
//                        {
//                            Text = "Comment4 test for expense",
//                            Important = false
//                        }
//                    }
//                });
                

//                // set the page number
//                int Page = 1;
//                PaginatedList<ExpenseGetModel> actual =
//                    expenseService.GetAll(Page,
//                                                  DateTime.ParseExact("04/30/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                                                  DateTime.ParseExact("06/30/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                                                  Labo2.Models.Type.Food);

//                // check if something is returned and number of expenses on the first page
//                Assert.IsNotNull(actual);
//                Assert.AreEqual(3, actual.Entries.Count);

//                // set the page number
//                Page = 2;
//                PaginatedList<ExpenseGetModel> actual2 =
//                    expenseService.GetAll(Page,
//                                                  DateTime.ParseExact("04/30/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                                                  DateTime.ParseExact("06/30/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                                                  Labo2.Models.Type.Food);

//                // check if something is returned and number of expenses on on the second page
//                Assert.IsNotNull(actual2);
//                Assert.AreEqual(2, actual2.Entries.Count);
//            }
//        }

//        /// <summary>
//        /// Delete a valid expense from db.
//        /// </summary>
//        [Test]
//        public void DeleteValidExpenseShouldDeleteExpense()
//        {
//            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
//              .UseInMemoryDatabase(databaseName: nameof(DeleteValidExpenseShouldDeleteExpense))
//              .Options;

//            using (var context = new ExpensesDbContext(options))
//            {
//                var expenseService = new ExpenseService(context);

//                var expected = new ExpensePostModel()
//                {
//                    Description = "Expense for test",
//                    Sum = 34,
//                    Location = "Cluj",
//                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                    Currency = "RON",
//                    Type = "Utilities",
//                    Comments = null,
//                };
                
//                var actual = expenseService.Create(expected);
//                // delete expense and put return expense in obj 
//                var afterDelete = expenseService.Delete(actual.Id);
//                // search for the added expense to see if exists in db
//                var result = context.Expenses.Find(actual.Id);

//                Assert.IsNotNull(afterDelete);
//                Assert.IsNull(result);
//            }
//        }

//        /// <summary>
//        /// Delete a valid expense from db.
//        /// </summary>
//        [Test]
//        public void DeleteInvalidExpenseShouldNotDeleteExpense()
//        {
//            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
//              .UseInMemoryDatabase(databaseName: nameof(DeleteInvalidExpenseShouldNotDeleteExpense))
//              .Options;

//            using (var context = new ExpensesDbContext(options))
//            {
//                var expenseService = new ExpenseService(context);

//                var expected = new ExpensePostModel()
//                {
//                    Description = "Expense for test",
//                    Sum = 34,
//                    Location = "Cluj",
//                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                    Currency = "RON",
//                    Type = "Utilities",
//                    Comments = null
//                };
//                // add the expense to db
//                var actual = expenseService.Create(expected);
//                // delete a non existing expense and put return in a expense obj 
//                var afterDelete = expenseService.Delete(888);
//                // search for the added expense to see if exists in db
//                var result = context.Expenses.Find(actual.Id);

//                Assert.IsNull(afterDelete);
//                Assert.IsNotNull(result);
//                Assert.AreEqual(expected.Description, result.Description);
//                Assert.AreEqual(expected.Sum, result.Sum);
//                Assert.AreEqual(expected.Location, result.Location);
//                Assert.AreEqual(expected.Date, result.Date);
//                Assert.AreEqual(expected.Currency, result.Currency);
//                Assert.AreEqual(expected.Type, result.Type.ToString());
//                Assert.AreEqual(expected.Comments, result.Comments);

//            }
//        }

//        /// <summary>
//        /// Delete a valid expense with comments from db.
//        /// </summary>
//        [Test]
//        public void DeleteValidExpenseWithCommentsShouldDeleteExpenseAndComments()
//        {
//            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
//              .UseInMemoryDatabase(databaseName: nameof(DeleteValidExpenseWithCommentsShouldDeleteExpenseAndComments))
//              .Options;

//            using (var context = new ExpensesDbContext(options))
//            {
//                var expenseService = new ExpenseService(context);

//                var expected = new ExpensePostModel()
//                {
//                    Description = "Expense for test",
//                    Sum = 34,
//                    Location = "Cluj",
//                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                    Currency = "RON",
//                    Type = "Utilities",
//                    Comments = new List<Comment>()
//                    {
//                        new Comment()
//                        {
//                            Text = "Comment test for expense",
//                            Important = false
//                        }
//                    }
//                };
//                // add the expense to db
//                var actual = expenseService.Create(expected);
//                // delete expense and put return expense in obj 
//                var afterDelete = expenseService.Delete(actual.Id);
//                // find the number of comments that exists in db
//                int numberOfCommentsInDb = context.Comment.CountAsync().Result;
//                // search for the added expense to see if exists in db
//                var resultExpense = context.Expenses.Find(actual.Id);

//                Assert.IsNotNull(afterDelete);
//                Assert.IsNull(resultExpense);
//                Assert.AreEqual(0, numberOfCommentsInDb);
//            }
//        }


//        /// <summary>
//        /// Test if an expense with a specific given Valid Id is returned.
//        /// </summary>
//        [Test]
//        public void GetExpenseByValidIdShouldFindIdSelectedExpense()
//        {
//            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
//              .UseInMemoryDatabase(databaseName: nameof(GetExpenseByValidIdShouldFindIdSelectedExpense))
//              .Options;

//            using (var context = new ExpensesDbContext(options))
//            {
//                var expenseService = new ExpenseService(context);

//                var toAdd = new ExpensePostModel()
//                {
//                    Description = "Expense for test",
//                    Sum = 34,
//                    Location = "Cluj",
//                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                    Currency = "RON",
//                    Type = "Utilities",
//                    Comments = new List<Comment>()
//                    {
//                        new Comment()
//                        {
//                            Text = "Comment test for expense",
//                            Important = false
//                        }
//                    }
//                };

//                var actual = expenseService.Create(toAdd);

//                var expected = expenseService.GetById(actual.Id);

//                Assert.IsNotNull(expected);
//                Assert.AreEqual(expected.Description, actual.Description);
//                Assert.AreEqual(expected.Sum, actual.Sum);
//                Assert.AreEqual(expected.Location, actual.Location);
//                Assert.AreEqual(expected.Date, actual.Date);
//                Assert.AreEqual(expected.Currency, actual.Currency);
//                Assert.AreEqual(expected.Type, actual.Type);
//                Assert.AreEqual(expected.Comments, actual.Comments);
//            }
//        }


//        /// <summary>
//        /// Test if an expense with a given Invalid Id is returned.
//        /// </summary>
//        [Test]
//        public void GetExpenseByInvalidIdShouldNotFindAnyExpense()
//        {
//            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
//              .UseInMemoryDatabase(databaseName: nameof(GetExpenseByInvalidIdShouldNotFindAnyExpense))
//              .Options;

//            using (var context = new ExpensesDbContext(options))
//            {
//                var expenseService = new ExpenseService(context);

//                var toAddExpense = new ExpensePostModel()
//                {
//                    Description = "Expense for test",
//                    Sum = 34,
//                    Location = "Cluj",
//                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                    Currency = "RON",
//                    Type = "Utilities",
//                    Comments = new List<Comment>()
//                    {
//                        new Comment()
//                        {
//                            Text = "Comment test for expense",
//                            Important = false
//                        }
//                    }
//                };

//                var actual = expenseService.Create(toAddExpense);

//                var expected = expenseService.GetById(123);

//                Assert.IsNull(expected);

//            }
//        }


//        /// <summary>
//        /// Update an expense, should modify the given expense 
//        /// </summary>
//        [Test]
//        public void UpdateExpenseShouldModifyTheExpense()
//        {
//            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
//              .UseInMemoryDatabase(databaseName: nameof(UpdateExpenseShouldModifyTheExpense))
//              .Options;

//            using (var context = new ExpensesDbContext(options))
//            {
//                var expenseService = new ExpenseService(context);

//                var toExpense = new ExpensePostModel()
//                {
//                    Description = "Expense for test",
//                    Sum = 34,
//                    Location = "Cluj",
//                    Date = DateTime.ParseExact("07/10/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                    Currency = "RON",
//                    Type = "Utilities",
//                    Comments = new List<Comment>()
//                    {
//                        new Comment()
//                        {
//                            Text = "Comment test for expense",
//                            Important = false
//                        }
//                    }
//                };
//                var added = expenseService.Create(toExpense);

//                // new modified expense 
//                var toExpenseUpdated = new ExpensePostModel()
//                {
//                    Description = "Expense for test updated",
//                    Sum = 30,
//                    Location = "Timisoara",
//                    Date = DateTime.ParseExact("05/31/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture),
//                    Currency = "Euro",
//                    Type = "Food",
//                    Comments = new List<Comment>()
//                    {
//                        new Comment()
//                        {
//                            Text = "An updated test for expense with comment",
//                            Important = true
//                        }
//                    }
//                };
//                var updatedExpense = expenseService.Upsert(added.Id, toExpense);


//                Assert.IsNotNull(updatedExpense);
//                Assert.AreEqual(toExpense.Description, updatedExpense.Description);
//                Assert.AreEqual(toExpense.Sum, updatedExpense.Sum);
//                Assert.AreEqual(toExpense.Location, updatedExpense.Location);
//                Assert.AreEqual(toExpense.Date, updatedExpense.Date);
//                Assert.AreEqual(toExpense.Currency, updatedExpense.Currency);
//                Assert.AreEqual(toExpense.Type, updatedExpense.Type.ToString());
//                Assert.AreEqual(toExpense.Comments, updatedExpense.Comments);

//            }
//        }
//    }
//}
