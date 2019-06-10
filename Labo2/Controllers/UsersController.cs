using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Labo2.Models;
using Labo2.Services;
using Labo2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Labo2.Controllers
{
    [Authorize(Roles = "Admin, UserManager")]
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersService userService;
        //private IUser_UserRolesService user_userRolesService;

        public UsersController(IUsersService userService)//, IUser_UserRolesService user_userRolesService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginPostModel login)
        {
            var user = userService.Authenticate(login.Username, login.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        //[HttpPost]
        public IActionResult Register([FromBody]RegisterPostModel registerModel)
        {
            var errors = userService.Register(registerModel);//, out User user);
            if (errors != null)
            {
                return BadRequest(errors);
            }
            return Ok();//user);
        }
        [Authorize(Roles = "Admin,UserManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[AllowAnonymous]
        [HttpGet]
        public IEnumerable<UserGetModel> GetAll()
        {
            return userService.GetAll();
        }

        /// <summary>
        /// Find an user by the given id.
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /users
        ///     {  
        ///        id: 3,
        ///        firstName = "Bodea",
        ///        lastName = "Raluca",
        ///        userName = "raluca",
        ///        email = "raluca@yahoo.com",
        ///        userRole = "regular"
        ///     }
        /// </remarks>
        /// <param name="id">The id given as parameter</param>
        /// <returns>The user with the given id</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/Users/5
        [Authorize(Roles = "Admin,UserManager")]
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get(int id)
        {
            var found = userService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }

        /// <summary>
        /// Add an new User
        /// </summary>
        ///   /// <remarks>
        /// Sample response:
        ///
        ///     Post /users
        ///     {
        ///        firstName = "Bodea",
        ///        lastName = "Raluca",
        ///        userName = "raluca",
        ///        email = "raluca@yahoo.com",
        ///        userRole = "regular"
        ///     }
        /// </remarks>
        /// <param name="userPostModel">The input user to be added</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin,UserManager")]
        [HttpPost]
        public void Post([FromBody] UserPostModel userPostModel)
        {
            userService.Create(userPostModel);
        }


        /// <summary>
        /// Modify an user if exists in dbSet , or add if not exist
        /// </summary>
        /// <param name="id">id-ul user to update</param>
        /// <param name="user">obiect user to update</param>
        /// Sample request:
        ///     <remarks>
        ///     Put /users/id
        ///     {
        ///        firstName = "Bodea",
        ///        lastName = "Raluca",
        ///        userName = "raluca",
        ///        email = "raluca@yahoo.com",
        ///        userRole = "regular"
        ///     }
        /// </remarks>
        /// <returns>Status 200 daca a fost modificat</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin,UserManager")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User user)
        {
            var currentUserLogin = userService.GetCurrentUser(HttpContext);
            var result = userService.Upsert(id, user, currentUserLogin);
            return Ok(result);

            //    User currentUserLogIn = userService.GetCurrentUser(HttpContext);

            //    string activeUserLogin = user_userRolesService.GetById(id)
            //        .FirstOrDefault(u_ur => u_ur.EndTime == null)
            //        .UserRoleName;

            //    if (activeUserLogin.Equals("UserManager"))
            //    {
            //        UserGetModel userToUpdate = userService.GetById(id);

            //        var UserRegisteredYear = currentUserLogIn.DataRegistered;        //data inregistrarii
            //        var currentMonth = DateTime.Now;                                 //data curenta
            //        var nrLuni = currentMonth.Subtract(UserRegisteredYear).Days / (365.25 / 12);   //diferenta in luni dintre date

            //        if (nrLuni >= 6)
            //        {
            //            var result3 = userService.Upsert(id, userPostModel);
            //            return Ok(result3);
            //        }

            //        UserPostModel newUserPost = new UserPostModel
            //        {
            //            FirstName = userPostModel.FirstName,
            //            LastName = userPostModel.LastName,
            //            Username = userPostModel.Username,
            //            Email = userPostModel.Email,
            //            Password = userPostModel.Password,
            //            UserRoles = userToUpdate.ToString()
            //        };

            //        var result2 = userService.Upsert(id, newUserPost);
            //        return Ok(result2);
            //    }

            //    var result = userService.Upsert(id, userPostModel);
            //    return Ok(result);
        }



        /// <summary>
        /// Delete an user
        /// </summary>
        /// <param name="id">User id to delete</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,UserManager")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = userService.Delete(id);
            if (existing == null)
            {
                return NotFound();
            }

            return Ok(existing);
            //User curentUserLogIn = userService.GetCurrentUser(HttpContext);

            //var result = userService.Delete(id);
            //if (result == null)
            //{
            //    return NotFound("User with the given id not found !");
            //}
            //return Ok(result);
        }

        [HttpPut]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult RoleChanges([FromQuery]int id, [FromBody] string Role)
        {
           
            var currentUser = userService.GetCurrentUser(HttpContext);
            var existing = userService.RoleChanges(id, Role, currentUser);
            if (existing == null)
            {
                return NotFound();
            }

            return Ok(existing);

        }

        [HttpGet("{id}", Name = "GetHistoryRoles")]
        public IActionResult GetHistoryOfARole(int id)
        {
            var found = userService.GetHistoryOfARole(id);

            if (found == null)
            {
                return NotFound();
            }

            return Ok(found);
        }

    }
}