using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entity;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;

        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(){

            var users = await this.userRepository.GetMembersAsync();
            return  Ok(users);
        }

        //api/users/3
         [HttpGet("{username}")]

        public async Task<ActionResult<MemberDto>> GetUser(string username){

            return await this.userRepository.GetMemberAsync(username);     
        }

        [HttpPut]

        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await this.userRepository.GetUserByUserNameAsync(username);
            if(user == null) return NotFound();
            this.mapper.Map(memberUpdateDto, user);
            if(await this.userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Unable to update the data."); 

        }


    }
}