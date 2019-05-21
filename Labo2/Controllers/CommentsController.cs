﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// <returns>List of comments</returns>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // GET: api/Comments
        [HttpGet]
        public IEnumerable<CommentGetModel> Get([FromQuery]String filter)
        {
            return commentService.GetAll(filter);
        }
    }


}