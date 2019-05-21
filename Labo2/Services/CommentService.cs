using Labo2.Models;
using Labo2.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Labo2.Services
{
    public interface ICommentService
    {

        IEnumerable<CommentGetModel> GetAll(String filter);

    }

    public class CommentService : ICommentService
    {

        private ExpensesDbContext context;

        public CommentService(ExpensesDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<CommentGetModel> GetAll(String filter)
        {
            IQueryable<Expense> result = context.Expenses.Include(c => c.Comments);

            List<CommentGetModel> resultComments = new List<CommentGetModel>();
            List<CommentGetModel> resultCommentsAll = new List<CommentGetModel>();

            foreach (Expense expense in result)
            {
                expense.Comments.ForEach(c =>
                {
                    if (c.Text == null || filter == null)
                    {
                        CommentGetModel comment = new CommentGetModel
                        {
                            Id = c.Id,
                            Important = c.Important,
                            Text = c.Text,
                            ExpenseId = expense.Id

                        };
                        resultCommentsAll.Add(comment);
                    }
                    else if (c.Text.Contains(filter))
                    {
                        CommentGetModel comment = new CommentGetModel
                        {
                            Id = c.Id,
                            Important = c.Important,
                            Text = c.Text,
                            ExpenseId = expense.Id

                        };
                        resultComments.Add(comment);

                    }
                });
            }
            if (filter == null)
            {
                return resultCommentsAll;
            }
            return resultComments;
        }
    }
}