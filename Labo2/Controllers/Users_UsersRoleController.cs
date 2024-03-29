﻿using Labo2.Models;
using Labo2.Services;
using Labo2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Labo2.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class User_UserRolesController : ControllerBase
    {
        private IUser_UserRoleService user_userRoleService;

        public User_UserRolesController(IUser_UserRoleService user_userRoleService)
        {
            this.user_userRoleService = user_userRoleService;
        }


        /// <summary>
        /// Find an userUserRole by the given id.
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /user_userRoles
        ///     [
        ///     {  
        ///        id: 1,
        ///        userId = 1,
        ///        UserRoleId = 1,
        ///        UserRole = "Regular",
        ///        StartTime = 2019-06-20,
        ///        EndTime = null
        ///     }
        ///     ]
        /// </remarks>
        /// <param name="id">The id given as parameter</param>
        /// <returns>A list of userUserRole with the given id</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/user_userRoles/1
        [HttpGet("{id}", Name = "GetUser_UserRole")]
        public IActionResult Get(int id)
        {
            var found = user_userRoleService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }


        /// <summary>
        /// Add an new UserUserRole
        /// </summary>
        ///   /// <remarks>
        /// Sample response:
        ///
        ///     Post /user_userRoles
        ///     {
        ///        userId = 1,
        ///        userRoleName = "UserManager"        
        ///     }
        /// </remarks>
        /// <param name="user_userRolePostModel"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public void Post([FromBody] User_UserRolePostModel user_userRolePostModel)
        {
            user_userRoleService.Create(user_userRolePostModel);
        }

    }
}