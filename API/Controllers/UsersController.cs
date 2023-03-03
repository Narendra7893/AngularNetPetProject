using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entity;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IPhotoInterface photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoInterface photoService)
        {
            this.photoService = photoService;
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
            var username = User.GetUserName();
            var user = await this.userRepository.GetUserByUserNameAsync(username);
            if(user == null) return NotFound();
            this.mapper.Map(memberUpdateDto, user);
            if(await this.userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Unable to update the data."); 

        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await userRepository.GetUserByUserNameAsync(User.GetUserName());
            if(user == null) return NotFound();
            var result = await this.photoService.AddPhotoAsync(file);
            if(result.Error != null) return BadRequest(result.Error.Message);
            var photo =  new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0) photo.IsMain = true;
            user.Photos.Add(photo);
            if(await userRepository.SaveAllAsync()) 
            {
                return CreatedAtAction(nameof(GetUser),new{username=user.UserName},this.mapper.Map<PhotoDto>(photo));
            }
            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoid}")]
        public async Task<ActionResult> SetMainPhoto(int photoid)
        {
            var user = await this.userRepository.GetUserByUserNameAsync(User.GetUserName());
            if(user==null) return NotFound();
            var photo =  user.Photos.FirstOrDefault(x => x.Id == photoid);
            if(photo == null) return NotFound();
            if (photo.IsMain == true) return BadRequest("This is already main photo.");
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if(currentMain != null) currentMain.IsMain = false;
            photo.IsMain =true;
            if(await this.userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem setting main photo.");
        }

        [HttpDelete("delete-photo/{photoid}")]

        public async Task<ActionResult> DeletePhoto(int photoid)
        {
             var user = await this.userRepository.GetUserByUserNameAsync(User.GetUserName());
            if(user==null) return NotFound();
              var photo =  user.Photos.FirstOrDefault(x => x.Id == photoid);
            if(photo == null) return NotFound();
            if (photo.IsMain == true) return BadRequest("You can not delete main photo.");
            if(photo.PublicId == null){
                var result = await this.photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);
             if(await this.userRepository.SaveAllAsync()) return Ok();

             return BadRequest("Problem deleting photo.");

        }

    }
}