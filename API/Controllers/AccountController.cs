

using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    public class AccountController: BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }

        //creating and endpoint for the user to register
      [HttpPost("register")]  //POST: api/account/register

      public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
      {
        if(await UserExists(registerDto.Username)) return BadRequest("User name is taken");

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt =  hmac.Key
        };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // return new UserDto
            // {
            //      Username = user.UserName,
            //      Token = _tokenService.CreateToken(user)
            // };
           return user;
           
      }
      private async Task<bool> UserExists(string username){
        
        return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
      }

      //adding the login endpoint
      [HttpPost("login")]

      public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
        var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
  
              if (user == null) 
              return Unauthorized("Invalid user name"); //using an http response, needs actionresult

        using var hmac = new HMACSHA512(user.PasswordSalt);//hmac initiated with the user's passwordSalt stored in the DB

         var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

         for (int i = 0; i < computedHash.Length; i++)
         {
             if(computedHash[i] != user.PasswordHash[i])
             return Unauthorized("Invalid Password");
         }
              return new UserDto
            {
                 Username = user.UserName,
                 Token = _tokenService.CreateToken(user)
            };
      }
    }
}