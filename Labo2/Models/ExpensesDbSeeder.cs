using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labo2.Models
{
    public class ExpensesDbSeeder
    {

        public static void Initialize(ExpensesDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any expenses.
            if (context.Expenses.Any())
            {
                return;   // DB has been seeded
            }

            context.Expenses.AddRange(
                new Expense
                {
                    Description = "Des1",
                    Sum = 22.4,
                    Location = "Loc1",
                    Currency = "Euro",
                    Type = "Food"
                },
                new Expense
                {
                    Description = "Des2",
                    Sum = 32.5,
                    Location = "Loc2",
                    Currency = "Euro",
                    Type = "other"
                }
            );
            context.SaveChanges();
        }
    }
}

