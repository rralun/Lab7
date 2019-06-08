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
                Assert.AreEqual(1, allComments.NumberOfPages);
            }
        }
    }
}
