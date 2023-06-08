

using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{  
    [Authorize]
      public class UsersController : BaseApiController
    {

        private readonly DataContext _context;

        public UsersController(DataContext context) 
        {
            _context = context;
        }

        [HttpGet]
        /* attribute that will get the resource from the API Endpoint. The method below is a public endpoint to get all users from the Users table*/
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync(); //this is the session with the DB,  we get the Users table, and then we list them.
            return users;
        }
        
        
        [HttpGet("{id}")]//endpoint to get a single user with the id as a parameter
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);//returnig the value directly
            return user;
        }
    }
}