using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;
using CoHabit.API.Enums;
using CoHabit.API.Helpers;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoHabit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<Post>>>> GetAllPosts(int currentPage, int pageSize)
        {
            try
            {
                var posts = await _postService.GetPostsAsync(currentPage, pageSize);
                return Ok(ApiResponse<PaginationResponse<List<Post>>>.SuccessResponse(posts, "Posts retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //API lấy tất cả bài viết của admin
        [HttpGet("admin")]
        public async Task<ActionResult<List<PostResponse>>> GetAllAdminPosts()
        {
            try
            {
                var posts = await _postService.GetAllPostsAsync();
                return Ok(ApiResponse<List<PostResponse>>.SuccessResponse(posts, "Admin posts retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //API lất tất cả bài viết theo userId
        [HttpGet("user/all")]
        [Authorize]
        public async Task<ActionResult<List<Post>>> GetAllPostsByUser()
        {
            try
            {
                var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");
                var posts = await _postService.GetAllPostsByUserAsync(userId);
                return Ok(ApiResponse<List<Post>>.SuccessResponse(posts, "User posts retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //API lất tất cả bài viết đã được duyệt theo userId
        [HttpGet("user/publish")]
        [Authorize]
        public async Task<ActionResult<List<Post>>> GetAllPublishPostsByUser()
        {
            try
            {
                var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");
                var posts = await _postService.GetAllPublishPostsByUserAsync(userId);
                return Ok(ApiResponse<List<Post>>.SuccessResponse(posts, "User posts retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //API lấy detail bài viết theo postId
        [HttpGet("{postId}")]
        public async Task<ActionResult<PostResponse>> GetPostById(Guid postId)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(postId);
                if (post == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("Post not found."));
                }
                return Ok(ApiResponse<PostResponse>.SuccessResponse(post, "Post retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //API tạo bài viết
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreatePost([FromBody] PostRequest req)
        {
            try
            {
                var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var parsedUserId)
                    ? parsedUserId
                    : throw new Exception("Invalid user ID");
                var result = await _postService.CreatePostAsync(userId, req);
                if (result == 0)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Failed to create post."));
                }
                return Ok(ApiResponse<object>.SuccessResponse(new { PostId = result }, "Post created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //API chinh sửa bài viết
        [HttpPut("{postId}")]
        [Authorize]
        public async Task<ActionResult> UpdatePost([FromBody] PostRequest req, Guid postId)
        {
            try
            {
                var result = await _postService.UpdatePostAsync(req, postId);
                if (result == 0)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Failed to update post."));
                }
                return Ok(ApiResponse<object>.SuccessResponse(new { }, "Post updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //API update status thành Hidden cho User
        [HttpPatch("user/{postId}/hidden")]
        // [Authorize]
        public async Task<ActionResult> UpdatePostStatusToHiddenByUser(Guid postId)
        {
            try
            {
                var result = await _postService.UpdatePostStatusAsync(postId, PostStatus.Hidden);
                if (result == 0)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Failed to update post status."));
                }
                return Ok(ApiResponse<object>.SuccessResponse(new { }, "Post status updated to Hidden successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //API update status cho Admin
        [HttpPatch("user/{postId}/{status}")]
        // [Authorize]
        public async Task<ActionResult> UpdatePostStatusToHiddenByUser(Guid postId, PostStatus status)
        {
            try
            {
                var result = await _postService.UpdatePostStatusAsync(postId, status);
                if (result == 0)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Failed to update post status."));
                }
                return Ok(ApiResponse<object>.SuccessResponse(new { }, "Post status updated to Hidden successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //API Update Furniture
        [HttpPatch("furniture/{postId}")]
        public async Task<ActionResult> UpdateFurnitureInPost(Guid postId, [FromBody] List<string> furnitureIds)
        {
            try
            {
                var result = await _postService.UpdateFurnitureInPostAsync(postId, furnitureIds);
                if (result == 0)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Failed to update furniture in post."));
                }
                return Ok(ApiResponse<object>.SuccessResponse(new { }, "Furniture in post updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}