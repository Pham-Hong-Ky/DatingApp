

using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controller
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        public AccountController(UserManager<AppUsers> userManager, SignInManager<AppUsers> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerdto)
        {
            if(await UserExits(registerdto.Username)) return BadRequest("UserName is taken");

            var user = _mapper.Map<AppUsers>(registerdto);

            user.UserName = registerdto.Username.ToLower();
           
            var result = await _userManager.CreateAsync(user, registerdto.Password);

            if(!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if(!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDto{
                Username = user.UserName,
                Token = await  _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }
        [HttpPost("login")]

        public async Task<ActionResult<UserDto>> Login(LoginDto logindto)
        {
            var user = await _userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == logindto.Username.ToLower());

            if(user == null) return Unauthorized("Invanlid username");

            var result = await _signInManager.CheckPasswordSignInAsync(user, logindto.Password, false);

            if(!result.Succeeded) return Unauthorized();

            return new UserDto
            {
                Username = user.UserName,
                Token = await  _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        private async Task<bool> UserExits(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

    }
}