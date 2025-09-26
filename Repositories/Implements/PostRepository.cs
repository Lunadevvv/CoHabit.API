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
                .ToListAsync();
        }

        public async Task<List<Post>> GetAllPostsByStatusAsync(PostStatus status)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Where(p => p.Status == status)
                .ToListAsync();
        }

        public async Task<Post?> GetPostByIdAsync(Guid id)
        {
            return await _context.Posts
                .Include(p => p.Furnitures)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PostId == id);
        }

        public async Task<PaginationResponse<List<Post>>> GetPostsAsync(int CurrentPage, int pageSize)
        {
            var items = await _context.Posts
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
    }
}