using Labo2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labo2.ViewModels
{
    public class ExpenseGetModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfComments { get; set; }
       
        public static ExpenseGetModel FromExpense(Expense expense)
        {
            return new ExpenseGetModel
            {
                Id = expense.Id,
                Description = expense.Description,
                Location = expense.Location,
                Date = expense.Date,
                NumberOfComments = expense.Comments.Count
               
                
            };
        }
    }
}
