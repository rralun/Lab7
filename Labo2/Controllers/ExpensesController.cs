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
        ///<remarks>
        ///  {
        /// "id": 4,
        ///  "description": "Descrip3",
        ///  "type": 4,
        ///  "location": "Oradea",
        ///  "date": "2019-07-05T11:11:11",
        ///  "currency": "USD",
        /// "sum": 654.77,
        /// "comments": [
        ///   {
        ///     "id": 2,
        ///     "text": "Expensive?!",
        ///     "important": true
        /// },
        ///   {
        ///   "id": 3,
        ///   "text": "Not so expensive!",
        ///    "important": false
        /// }
        ///  ]
        ///  }
        /// </remarks>
        /// <summary>
        /// Get all the expenses
        /// </summary>
        /// <param name="from">Optional, filtered by minimum date</param>
        /// <param name="to">Optional, filtered by maximu date</param>
        /// <param name="type">Optional, filtered by type</param>
        /// <returns>A list of expenses</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/Expenses
        [HttpGet]
        public IEnumerable<Expense> Get([FromQuery]DateTime? from, [FromQuery]DateTime? to, [FromQuery]Models.Type? type)
        {
            return expenseService.GetAll(from, to, type);
        }

        ///<remarks>
        ///{
        ///"id": 2,
        ///"description": "Alta",
        ///"type": 7,
        ///"location": "Covasna",
        ///"date": "2019-05-07T10:10:10",
        ///"currency": "EUR",
        ///"sum": 617.55,
        ///"comments": []
        ///    }
        /// </remarks>
        /// <summary>
        /// Get an expense by a given id
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>One expense with specified id or not foud</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/Expenses/2
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
        ///<remarks>
        ///{
        ///"description": "Alta",
        ///"type": 7,
        ///"location": "Covasna",
        ///"date": "2019-05-07T10:10:10",
        ///"currency": "EUR",
        ///"sum": 617.55,
        ///"comments": []
        /// }
        /// </remarks>
        /// <summary>
        /// Add an expense to the db
        /// </summary>
        /// <param name="expense"></param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // POST: api/Expenses
        [HttpPost]
        public void Post([FromBody] Expense expense)
        {
            expenseService.Create(expense);
        }
        ///<remarks>
        ///{
        ///"id": 2,
        ///"description": "Alta",
        ///"type": 7,
        ///"location": "Covasna",
        ///"date": "2019-05-07T10:10:10",
        ///"currency": "EUR",
        ///"sum": 617.55,
        ///"comments": []
        ///    }
        /// </remarks>
        /// <summary>
        /// Add or update an expense to the db
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="expense">An expense to add or update</param>
        /// <returns>The added expense with all fields</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // PUT: api/Expenses/2
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Expense expense)
        {

            var result = expenseService.Upsert(id, expense);
            return Ok(result);
        }
        ///<remarks>
        ///{
        ///{
        ///"id": 2,
        ///"description": "Alta",
        ///"type": 7,
        ///"location": "Covasna",
        ///"date": "2019-05-07T10:10:10",
        ///"currency": "EUR",
        ///"sum": 617.55,
        ///"comments": []
        /// }
        /// </remarks>
        /// <summary>
        /// Delete an expense from the db
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>The deleted item or not found</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // DELETE: api/ApiWithActions/2
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