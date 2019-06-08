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



        //    /// <summary>
        //    /// Add model, so we can add Type(ENUM) using letters
        //    /// </summary>
        //    /// <param name="expense">Model received from "input"</param>
        //    /// <returns>added Expense</returns>
        //    public static Expense ToExpense(ExpensePostModel expense)
        //    {
        //        Models.Type type = new Models.Type();

        //        if (expense.Type == "Food")
        //        {
        //            type = Models.Type.Food;
        //        }

        //        else if (expense.Type == "Utilities")
        //        {
        //            type = Models.Type.Utilities;
        //        }

        //        else if (expense.Type == "Outing")
        //        {
        //            type = Models.Type.Outing;
        //        }

        //        else if (expense.Type == "Groceries")
        //        {
        //            type = Models.Type.Groceries;
        //        }

        //        else if (expense.Type == "Clothes")
        //        {
        //            type = Models.Type.Clothes;
        //        }

        //        else if (expense.Type == "Electronics")
        //        {
        //            type = Models.Type.Electronics;
        //        }

        //        else if (expense.Type == "Other")
        //        {
        //            type = Models.Type.Other;
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //        return new Expense
        //        {
        //            Description = expense.Description,
        //            Sum = expense.Sum,
        //            Location = expense.Location,
        //            Date = expense.Date,
        //            Currency = expense.Currency,
        //            Type = type,
        //            Comments = expense.Comments
        //        };
        //    }


        //    /// <summary>
        //    /// Update model, so we can update an expense Type(ENUM) using letters
        //    /// </summary>
        //    /// <param name="expensePostModel">Model received from "input"</param>
        //    /// <param name="expense"></param>
        //    /// <returns>added Expense</returns>
        //    public static Expense ToUpdateExpense(ExpensePostModel expensePostModel, Expense expense)
        //    {
        //        Models.Type type = new Models.Type();

        //        if (expensePostModel.Type == "Food")
        //        {
        //            type = Models.Type.Food;
        //        }

        //        else if (expensePostModel.Type == "Utilities")
        //        {
        //            type = Models.Type.Utilities;
        //        }

        //        else if (expensePostModel.Type == "Outing")
        //        {
        //            type = Models.Type.Outing;
        //        }

        //        else if (expensePostModel.Type == "Groceries")
        //        {
        //            type = Models.Type.Groceries;
        //        }

        //        else if (expensePostModel.Type == "Clothes")
        //        {
        //            type = Models.Type.Clothes;
        //        }

        //        else if (expensePostModel.Type == "Electronics")
        //        {
        //            type = Models.Type.Electronics;
        //        }

        //        else if (expensePostModel.Type == "Other")
        //        {
        //            type = Models.Type.Other;
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //        expense.Description = expensePostModel.Description;
        //        expense.Sum = expensePostModel.Sum;
        //        expense.Location = expensePostModel.Location;
        //        expense.Date = expensePostModel.Date;
        //        expense.Currency = expensePostModel.Currency;
        //        expense.Type = type;
        //        expense.Comments = expensePostModel.Comments;


        //        return expense;
        //    }
        //}
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
