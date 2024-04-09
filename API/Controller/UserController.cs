using API.Entities;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controller
{
    
    public class UserController : BaseApiController
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUsers>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [Authorize]    
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUsers>> GetUsers(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}