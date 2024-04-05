using API.Entities;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<AppUsers>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
            
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUsers>> GetUsers(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}