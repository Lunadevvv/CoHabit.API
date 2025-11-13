using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;
using CoHabit.API.Enums;
using CoHabit.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoHabit.API.Repositories.Implements
{
    public class PostRepository : IPostRepository
    {
        private readonly CoHabitDbContext _context;
        public PostRepository(CoHabitDbContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.PostImages)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<List<Post>> GetAllPostsByStatusAsync(PostStatus status)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.PostImages)
                .AsSplitQuery()
                .Where(p => p.Status == status)
                .ToListAsync();
        }

        public async Task<Post?> GetPostByIdAsync(Guid id)
        {
            return await _context.Posts
                .Include(p => p.Furnitures)
                .Include(p => p.User)
                .Include(p => p.PostImages)
                .Include(p => p.LikedByUsers)
                .AsSplitQuery()
                .FirstOrDefaultAsync(p => p.PostId == id);
        }

        public async Task<PaginationResponse<List<Post>>> GetPostsAsync(int CurrentPage, int pageSize)
        {
            var items = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.PostImages)
                .AsSplitQuery()
                .Where(p => p.Status == PostStatus.Publish)
                .ToListAsync();

            var totalItems = items.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            items = items.Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationResponse<List<Post>>
            {
                CurrentPage = CurrentPage,
                PageSize = pageSize,
                TotalCount = totalItems,
                TotalPages = totalPages,
                Items = items
            } ?? new PaginationResponse<List<Post>> { Items = new List<Post>() };
        }

        public async Task<List<Post>> GetAllPostsByUserAsync(Guid userId)
        {
            return await _context.Posts
                .Include(p => p.PostImages)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public void CreatePostAsync(Post post)
        {
            _context.Posts.Add(post);
        }

        public void UpdatePostAsync(Post post)
        {
            _context.Posts.Update(post);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Post>? IsPostInFavoritesAsync(Guid userId, Guid postId)
        {
            return await _context.Posts
                .Include(p => p.PostImages)
                .Include(p => p.LikedByUsers)
                .AsSplitQuery()
                .Where(p => p.PostId == postId && p.LikedByUsers.Any(u => u.Id == userId))
                .FirstOrDefaultAsync();
        }

        public async Task<PaginationResponse<List<Post>>> SearchPostsWithPaginationAsync(int currentPage, int pageSize, string? address, int? maxPrice, double? averageRating)
        {
            var query = _context.Posts
                .Include(p => p.User)
                .Include(p => p.PostImages)
                .AsSplitQuery()
                .Where(p => p.Status == PostStatus.Publish)
                .AsQueryable();

            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(p => p.Address.Contains(address));
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            if (averageRating.HasValue)
            {
                query = query.Where(p => p.AverageRating >= averageRating.Value);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResponse<List<Post>>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = totalItems,
                TotalPages = totalPages,
                Items = items
            } ?? new PaginationResponse<List<Post>> { Items = new List<Post>() };
        }

        public async Task<List<Post>> GetFavoritePostsByUserIdAsync(Guid userId)
        {
            return await _context.Posts
                .Where(p => p.LikedByUsers.Any(u => u.Id == userId))
                .Include(p => p.PostImages)
                .ToListAsync();
        }

        public async Task<int> AddPostToFavoritesAsync(User user, Post post)
        {
            post.LikedByUsers.Add(user);
            _context.Posts.Update(post);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemovePostFromFavoritesAsync(User user, Post post)
        {
            post.LikedByUsers.Remove(user);
            _context.Posts.Update(post);
            return await _context.SaveChangesAsync();
        }
    }
}