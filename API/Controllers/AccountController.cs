using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using API.Data;
using API.DTOs;
using API.Entity;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
       private readonly DataContext _Context;

       private readonly ITokenService _TokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _Context = context;
            _TokenService = tokenService;
        }

        [HttpPost("Register")] //POST api/Account/Register?username=test&password=pas
        
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if(await UserExists(registerDto.UserName))
            return BadRequest("Username is taken.");
            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key

            };
           await _Context.Users.AddAsync(user);
           await _Context.SaveChangesAsync();
           return new UserDto
           {
               Username = user.UserName,
               Token = _TokenService.CreateToken(user)
           };
            
        }

        [HttpPost("Login")]

        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _Context.Users.SingleOrDefaultAsync(
                x => x.UserName == loginDto.Username);

            if(user == null) return Unauthorized("Username is invalid.");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i=0; i< computedHash.Length; i++)
            {

                if(computedHash[i] != user.PasswordHash[i])
                return Unauthorized("Password is invalid.");
            }

           return new UserDto
           {
               Username = user.UserName,
               Token = _TokenService.CreateToken(user)
           };

        }

        private async Task<bool> UserExists(string username)
        {
            return await _Context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}