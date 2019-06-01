using Labo2.Models;
using Labo2.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Labo2.Services
{
    public interface IExpenseService
    {

        PaginatedList<ExpenseGetModel> GetAll(int page, DateTime? from = null, DateTime? to = null, Models.Type? type = null);

        Expense GetById(int id);

        Expense Create(ExpensePostModel expense, User addedBy);

        Expense Upsert(int id, Expense expense);

        Expense Delete(int id);

    }
    public class ExpenseService : IExpenseService
    {

        private ExpensesDbContext context;

        public ExpenseService(ExpensesDbContext context)
        {
            this.context = context;
        }


        public Expense Create(ExpensePostModel expense, User addedBy)
        {
            Expense toAdd = ExpensePostModel.ToExpense(expense);
            toAdd.AddedBy = addedBy;
            context.Expenses.Add(toAdd);
            context.SaveChanges();
            return toAdd;
        }

        public Expense Delete(int id)
        {
            var existing = context.Expenses.Include(x => x.Comments).FirstOrDefault(expense => expense.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Expenses.Remove(existing);
            context.SaveChanges();
            return existing;
        }

        public PaginatedList<ExpenseGetModel> GetAll(int page, DateTime? from = null, DateTime? to = null, Models.Type? type = null)
        {
            IQueryable<Expense> result = context
               .Expenses
               .OrderBy(t => t.Id)
               .Include(t => t.Comments);
            PaginatedList<ExpenseGetModel> paginatedResult = new PaginatedList<ExpenseGetModel>();
            paginatedResult.CurrentPage = page;

            if (from != null)
            {
                result = result.Where(d => d.Date >= from);
            }
            if (to != null)
            {
                result = result.Where(d => d.Date <= to);
            }
            if (type != null)
            {
                result = result.Where(e => e.Type == type);
            }
            paginatedResult.NumberOfPages = (result.Count() - 1) / PaginatedList<ExpenseGetModel>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedList<ExpenseGetModel>.EntriesPerPage)
                .Take(PaginatedList<ExpenseGetModel>.EntriesPerPage);
            paginatedResult.Entries = result.Select(e => ExpenseGetModel.FromExpense(e)).ToList();

            return paginatedResult;
            
        }

        public Expense GetById(int id)
        {
            return context.Expenses.Include(x => x.Comments).FirstOrDefault(e => e.Id == id);
        }

        public Expense Upsert(int id, Expense expense)
        {
            var existing = context.Expenses.AsNoTracking().FirstOrDefault(e => e.Id == id);
            if (existing == null)
            {
                context.Expenses.Add(expense);
                context.SaveChanges();
                return expense;

            }

            expense.Id = id;
            context.Expenses.Update(expense);
            context.SaveChanges();
            return expense;
        }

    }

}