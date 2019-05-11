using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Labo2.Models
{
    public enum Type
    {
        food,
        utilities,
        transportation,
        outing,
        groceries,
        clothes,
        electronics,
        other
    }
   
    public class Expense
    {

        //[Key()]
        public int Id { get; set; }
        public string Description { get; set; }
        public double Sum { get; set; }
        public string Location { get; set; }
        public string Currency { get; set; }
        [EnumDataType(typeof(Type))]
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public List<Comment>Comments { get; set; }
    }
}

