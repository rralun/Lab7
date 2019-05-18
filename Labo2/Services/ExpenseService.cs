using Labo2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Labo2.Services
{
    public interface IExpenseService
    {
        IEnumerable<Expense> GetAll (DateTime? from = null, DateTime? to = null, String type=null);
        Expense GetById(int id);
        Expense Create(Expense expense);
        Expense Upsert(int id, Expense expense);
        Expense Delete(int id);
    }

    public class ExpenseService : IExpenseService   
    {
        //aici tb sa declar un DB context
        private ExpensesDbContext context;
        //tb constructor
        public ExpenseService(ExpensesDbContext context)
        {
            this.context = context;
        }

        //acum mutam logica din Controller pe Service. Nu il eliminam dar Controller-ul va apela Service si  nu va mai apela UI-ul Service-ul

        public Expense Create(Expense expense)
        {
            context.Expenses.Add(expense);
            context.SaveChanges();
            return expense;
        }

        public Expense Delete(int id)
        {
            var existing = context.Expenses.FirstOrDefault(expense => expense.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Expenses.Remove(existing);
            context.SaveChanges();

            return existing;
        }
        public IEnumerable<Expense> GetAll(DateTime? from = null, DateTime? to = null, string type=null)
        {
            IQueryable<Expense> result = context.Expenses.Include(f => f.Comments);
            if (from == null && to == null && type == null)
            {
                return result;
            }
            if (from != null)
            {
                result = result.Where(f => f.Date >= from);
            }
            if (to != null)
            {
                result = result.Where(f => f.Date <= to);
            }
            if (type != null)
            {
                result = result.Where(f => f.Type.Equals(type));
            }
            return result;
        }

       

        public Expense GetById(int id)
        {
            // sau context.Expenses.Find()
            return context.Expenses
                .Include(f => f.Comments)
                .FirstOrDefault(f => f.Id == id);
        }


        public Expense Upsert(int id, Expense expense)
        {
            var existing = context.Expenses.AsNoTracking().FirstOrDefault(f => f.Id == id);
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


