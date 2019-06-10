using Labo2.Models;
using Labo2.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Labo2.Services
{
    public interface ICommentService
    {
        PaginatedList<CommentGetModel> GetAll(int page, string filter);
        Comment GetById(int id);
        Comment Upsert(int id, Comment expense);
        Comment Delete(int id);
    }
    public class CommentService : ICommentService
    {
        private ExpensesDbContext context;
        public CommentService(ExpensesDbContext context)
        {
            this.context = context;
        }

        public PaginatedList<CommentGetModel> GetAll(int page, string filter)
        {
            IQueryable<Comment> result = context
                .Comment
                .Where(c => string.IsNullOrEmpty(filter) || c.Text.Contains(filter))
                .OrderBy(c => c.Id)
                .Include(c => c.Expense);
            var paginatedResult = new PaginatedList<CommentGetModel>();
            paginatedResult.CurrentPage = page;

            paginatedResult.NumberOfPages = (result.Count() - 1) / PaginatedList<CommentGetModel>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedList<CommentGetModel>.EntriesPerPage)
                .Take(PaginatedList<CommentGetModel>.EntriesPerPage);
            paginatedResult.Entries = result.Select(c => CommentGetModel.FromComment(c)).ToList();

            return paginatedResult;
        }

        public Comment GetById(int id)
        {
            return context.Comment
                .FirstOrDefault(c => c.Id == id);
        }

        public Comment Upsert(int id, Comment expense)
        {
            var existing = context.Comment.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (existing == null)
            {
                context.Comment.Add(expense);
                context.SaveChanges();
                return expense;
            }
            expense.Id = id;
            context.Comment.Update(expense);
            context.SaveChanges();
            return expense;
        }


        public Comment Delete(int id)
        {
            var existing = context.Comment.FirstOrDefault(comment => comment.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Comment.Remove(existing);
            context.SaveChanges();
            return existing;
        }

    }
}
