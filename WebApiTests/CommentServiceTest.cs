using Labo2.Models;
using Labo2.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiTests
{
    class CommentServiceTest
    {
        [Test]
        public void GetAllShouldReturnCorrectNumberOfPages()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPages))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {

                var commentService = new CommentService(context);
                var flowerService = new ExpenseService(context);
                var addedFlower = flowerService.Create(new Labo2.ViewModels.ExpensePostModel
                {
                    Description = "fdsfsd",
                    Type = "Utilities",
                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "asd",
                            AddedBy = null
                        }
                    },
                    Location = "Cluj",
                    Date = new DateTime(),
                    Currency = "USD",
                    Sum = 234.5

                }, null);

                var allComments = commentService.GetAll(1, string.Empty);
                Assert.NotNull(allComments);
                
            }
        }
        [Test]
        public void GetByIdTest()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPages))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {

                var commentService = new CommentService(context);
                var expenseService = new ExpenseService(context);
                var addedExpense = expenseService.Create(new Labo2.ViewModels.ExpensePostModel
                {
                    Description = "description test",
                    Sum = 1.23,
                    Location = "cluj",
                    Date = new DateTime(),
                    Currency = "euro",
                   
                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "ddfgfgsrt",
                            AddedBy = null
                        }
                    },

                }, null);

                var addedComment = commentService.Create(new Labo2.ViewModels.CommentPostModel
                {
                    Important = true,
                    Text = "asd",
                }, addedExpense.Id);

                var comment = commentService.GetById(addedComment.Id);
                Assert.NotNull(comment);
            }
        }
        [Test]
        public void DeleteCommentTest()
        {
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(DeleteCommentTest))
              .Options;

            using (var context = new ExpensesDbContext(options))
            {

                var commentService = new CommentService(context);
                var expenseService = new ExpenseService(context);
                var addedExpense = expenseService.Create(new Labo2.ViewModels.ExpensePostModel
                {
                    Description = "jshdkhsakjd",
                    Sum = 1.23,
                    Location = "jsfkdsf",
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

                var addedComment = commentService.Create(new Labo2.ViewModels.CommentPostModel
                {
                    Important = true,
                    Text = "fdlkflsdkm",
                }, addedExpense.Id);

                var comment = commentService.Delete(addedComment.Id);
                var commentNull = commentService.Delete(17);
                Assert.IsNull(commentNull);
                Assert.NotNull(comment);
            }
        }

    }
}
