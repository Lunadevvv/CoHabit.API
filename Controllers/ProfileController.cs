using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoHabit.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        // GET: api/v1/Profile
        [HttpGet]
        public async Task<ActionResult<ProfileResponse>> GetProfile()
        {
            try
            {
                var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");
                var profile = await _profileService.GetUserProfileAsync(userId);
                if (profile == null) return NotFound();
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/v1/Profile
        [HttpPut]
        public async Task<ActionResult> UpdateProfile([FromBody] ProfileRequest request)
        {
            try
            {
                var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");
                await _profileService.UpdateUserProfileAsync(userId, request);
                return Ok("Profile updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/v1/Profile/characteristics
        [HttpGet("characteristics")]
        public async Task<ActionResult<IEnumerable<Characteristic>>> GetUserCharacteristics()
        {
            try
            {
                var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");
                var characteristics = await _profileService.GetUserCharacteristicsByUserIdAsync(userId);
                return Ok(characteristics);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/v1/Profile/characteristics
        [HttpPut("characteristics")]
        public async Task<ActionResult> UpdateUserCharacteristics([FromBody] List<string> characteristicIds)
        {
            try
            {
                var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");
                await _profileService.UpdateUserCharacteristics(userId, characteristicIds);
                return Ok("User characteristics updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Get favorite posts
        [HttpGet("favorites")]
        public async Task<ActionResult<List<Post>>> GetFavoritePosts()
        {
            try
            {
                var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");
                var favoritePosts = await _profileService.GetFavoritePostsByUserIdAsync(userId);
                return Ok(favoritePosts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Add post to favorites
        [HttpPost("favorites/{postId}")]
        public async Task<ActionResult> AddPostToFavorites(Guid postId)
        {
            try
            {
                var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");
                await _profileService.AddPostToFavoritesAsync(userId, postId);
                return Ok("Post added to favorites successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Remove post from favorites
        [HttpDelete("favorites/{postId}")]
        public async Task<ActionResult> RemovePostFromFavorites(Guid postId)
        {
            try
            {
                var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");
                await _profileService.RemovePostFromFavoritesAsync(userId, postId);
                return Ok("Post removed from favorites successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}