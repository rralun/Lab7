using Labo2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Labo2.ViewModels
{
    public class ExpensePostModel
    {
        public string Description { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string Currency { get; set; }
        public double Sum { get; set; }
        public List<Comment> Comments { get; set; }

        public static Expense ToExpense(ExpensePostModel expense)
        {
            Models.Type expenseType = Models.Type.Utilities;
            if (expense.Type == "Food")
            {
                expenseType = Models.Type.Food;
            }
            else if (expense.Type == "Transportation")
            {
                expenseType = Models.Type.Transportation;
            }
            else if (expense.Type == "Outing")
            {
                expenseType = Models.Type.Outing;
            }
            else if (expense.Type == "Groceries")
            {
                expenseType = Models.Type.Groceries;
            }
            else if (expense.Type == "Clothes")
            {
                expenseType = Models.Type.Clothes;
            }
            else if (expense.Type == "Electronics")
            {
                expenseType = Models.Type.Electronics;
            }
            else if (expense.Type == "Other")
            {
                expenseType = Models.Type.Other;
            }
            //if (expense.Type == "Utilities") { }
            //else if (expense.Type == "Food") { }
            //else if (expense.Type == "Transportation") { }
            //else if (expense.Type == "Outing") { }
            //else if (expense.Type == " Groceries") { }
            //else if (expense.Type == " Clothes") { }
            //else if (expense.Type == " Electronics") { }
            //else if (expense.Type == " Other") { }


            return new Expense
            {
                
                Description = expense.Description,
                Type = expenseType,
                Location = expense.Location,
                Date = expense.Date,
                Currency = expense.Currency,
                Sum = expense.Sum,
                Comments = expense.Comments,

            };
        }
    }
}
