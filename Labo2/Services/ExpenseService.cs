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

        IEnumerable<Expense> GetAll(DateTime? from = null, DateTime? to = null, Models.Type? type = null);

        Expense GetById(int id);

        Expense Create(Expense user);

        Expense Upsert(int id, Expense user);

        Expense Delete(int id);

    }
    public class ExpenseService : IExpenseService
    {

        private ExpensesDbContext context;

        public ExpenseService(ExpensesDbContext context)
        {
            this.context = context;
        }


        public Expense Create(Expense expense)
        {
            context.Expenses.Add(expense);
            context.SaveChanges();
            return expense;
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

        public IEnumerable<Expense> GetAll(DateTime? from = null, DateTime? to = null, Models.Type? type = null)
        {
            IQueryable<Expense> result = context.Expenses.Include(x => x.Comments);
            if ((from == null && to == null) && type == null)

            {
                return result;
            }
            if (from != null)
            {
                result = result.Where(e => e.Date >= from);
            }
            if (to != null)
            {
                result = result.Where(e => e.Date <= to);
            }
            if (type != null)
            {
                result = result.Where(e => e.Type == type);
            }
            return result;
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