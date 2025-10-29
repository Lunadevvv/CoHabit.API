using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;
using CoHabit.API.Enums;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<PaginationResponse<List<Post>>> GetPostsAsync(int CurrentPage, int pageSize);
        Task<PaginationResponse<List<Post>>> SearchPostsWithPaginationAsync(int currentPage, int pageSize, string? address, int? maxPrice, double? averageRating);
        Task<List<Post>> GetAllPostsAsync();
        Task<List<Post>> GetAllPostsByStatusAsync(PostStatus status);
        Task<List<Post>> GetAllPostsByUserAsync(Guid userId);
        Task<Post>? IsPostInFavoritesAsync(Guid userId, Guid postId);
        Task<Post?> GetPostByIdAsync(Guid id);
        void CreatePostAsync(Post post);
        void UpdatePostAsync(Post post);
        Task<int> SaveChangesAsync();
    }
}