using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;
using CoHabit.API.Enums;

namespace CoHabit.API.Services.Interfaces
{
    public interface IPostService
    {
        Task<PaginationResponse<List<Post>>> GetPostsAsync(int CurrentPage, int pageSize);
        Task<List<PostResponse>> GetAllPostsAsync();
        Task<List<Post>> GetAllPostsByStatusAsync(PostStatus status);
        Task<List<Post>> GetAllPostsByUserAsync(Guid userId);
        Task<List<Post>> GetAllPublishPostsByUserAsync(Guid userId);
        Task<PostResponse?> GetPostByIdAsync(Guid id);
        Task<int> CreatePostAsync(Guid userId, PostRequest req);
        Task<int> UpdatePostAsync(PostRequest req, Guid postId);
        Task<int> UpdatePostStatusAsync(Guid postId, PostStatus status);
        Task<int> UpdateFurnitureInPostAsync(Guid postId, List<string> furnitureIds);
    }
}