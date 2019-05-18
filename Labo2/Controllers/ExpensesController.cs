using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labo2.Models;
using Labo2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Labo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private IExpenseService expenseService;
        public ExpensesController(IExpenseService expenseService)
        {
            this.expenseService = expenseService;
        }


        /// <summary>
        /// Gets all the expenses
        /// </summary>
        /// <param name="from">Optional, filter by minimum Date.</param>
        /// <param name="to">Optional, filter by maximum Date.</param>
        /// <param name="type">Optional, filter by type</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Expense> Get([FromQuery]DateTime? from, [FromQuery]DateTime? to, [FromQuery]String type)
        {
            return expenseService.GetAll(from, to,type);
        }

        // GET: api/Expenses/5
        [HttpGet("{id}", Name = "Get")]
       

        public IActionResult Get(int id)
        {
            var found = expenseService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }



        /// <summary>
        /// Add an expense.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Expenses
        ///     {
        ///          "id": 2,
        ///          "description": "Des2",
        ///          "sum": 32.5,
        ///          "location": "Loc2",
        ///          "currency": "Euro",
        ///          "type": "other",
        ///          "date": "0001-01-01T00:00:00",
        ///          "comments": [
        ///
        ///
        ///         ]
        ///     }
        ///
        /// </remarks>
        /// <param name="expense">The expense to add</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
       
       public void Post([FromBody] Expense expense)
        {
            expenseService.Create(expense);
        }


        // PUT: api/Expenses/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Expense expense)
        {
            var result = expenseService.Upsert(id, expense);
            return Ok(result);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = expenseService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}