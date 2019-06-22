using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labo2.Models;
using Labo2.Services;
using Labo2.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Labo2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }
        ///<remarks>
        ///{
        ///"id": 1,
        ///"text": "Expensive?!",
        ///"important": true
        ///}
        ///</remarks>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter">Optional, filtered by text</param>
        /// <param name="page"></param>
        /// <returns>List of comments</returns>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // GET: api/Comments
        [HttpGet]
        public PaginatedList<CommentGetModel> Get([FromQuery]string filter, int page = 1)
        {
            page = Math.Max(page, 1);
            return commentService.GetAll(page, filter);
        }


        // PUT: api/Comments/5

        /// <summary>
        /// update the comment with the specified id
        /// </summary>
        /// <param name="id">the id of the comment we want to update</param>
        /// <param name="comment">a comment that contains the new data</param>
        /// <returns>a comment object</returns>
        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody] Comment comment)
        //{
        //    var result = commentService.Upsert(id, comment);
        //    return Ok(result);
        //}

        // DELETE: api/ApiWithActions/5
        /// <summary>
        /// Delete the comment with the specified id
        /// </summary>
        /// <param name="id">The id of the comment we want to delete</param>
        /// <returns>a comment object</returns>
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var existing = commentService.Delete(id);
        //    if (existing == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(existing);
        //}
    }

}
    