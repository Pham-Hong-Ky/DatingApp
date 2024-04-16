

using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controller
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerdto)
        {
            if(await UserExits(registerdto.Username)) return BadRequest("UserName is taken");

            using var hmac = new HMACSHA256();

            var user = new AppUsers
            {
                UserName = registerdto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerdto.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();  
            return new UserDto{
                Username = user.UserName,
                Token =  _tokenService.CreateToken(user)
            };
        }
        [HttpPost("login")]

        public async Task<ActionResult<UserDto>> Login(LoginDto logindto)
        {
            var user = await _context.Users
                    .SingleOrDefaultAsync(x => x.UserName == logindto.Username);

            if(user == null) return Unauthorized("Invanlid username");

            using var hmac = new HMACSHA256(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));

            for(var i = 0 ; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invanlid password");
            }
            return new UserDto
            {
                Username = user.UserName,
                Token =  _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExits(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

    }
}