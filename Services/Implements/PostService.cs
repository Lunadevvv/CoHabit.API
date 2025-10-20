using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Enitites;
using CoHabit.API.Enums;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.Services.Interfaces;

namespace CoHabit.API.Services.Implements
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IFurnitureRepository _furnitureRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<PostService> _logger;
        public PostService(IPostRepository postRepository, IAuthRepository authRepository,
        IFurnitureRepository furnitureRepository, ICloudinaryService cloudinaryService,
        ILogger<PostService> logger)
        {
            _furnitureRepository = furnitureRepository;
            _authRepository = authRepository;
            _postRepository = postRepository;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
        }

        public async Task<PaginationResponse<List<PostResponse>>> GetPostsAsync(int CurrentPage, int pageSize)
        {
            var result = await _postRepository.GetPostsAsync(CurrentPage, pageSize);

            // Map Post to PostResponse
            var postResponses = result.Items.Select(p => new PostResponse
            {
                PostId = p.PostId,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                Address = p.Address,
                Condition = p.Condition,
                DepositPolicy = p.DepositPolicy,
                Status = p.Status,
                User = p.User != null ? new UserResponse
                (
                    p.User.Id,
                    p.User.FirstName,
                    p.User.LastName,
                    p.User.PhoneNumber,
                    p.User.Image
                ) : null,
                ImageUrl = p.PostImages != null ? p.PostImages.Select(img => img.ImageUrl).ToList() : new List<string>(),
                Furnitures = null
            }).ToList();

            var response = new PaginationResponse<List<PostResponse>>
            {
                Items = postResponses,
                CurrentPage = result.CurrentPage,
                TotalPages = result.TotalPages,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            };

            return response;
        }
        public async Task<List<PostResponse>> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            var postResponses = posts.Select(p => new PostResponse
            {
                PostId = p.PostId,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                Address = p.Address,
                Condition = p.Condition,
                DepositPolicy = p.DepositPolicy,
                Status = p.Status,
                User = p.User != null ? new UserResponse
                (
                    p.User.Id,
                    p.User.FirstName,
                    p.User.LastName,
                    p.User.PhoneNumber,
                    p.User.Image
                ) : null,
                ImageUrl = p.PostImages != null ? p.PostImages.Select(img => img.ImageUrl).ToList() : new List<string>(),
                Furnitures = null
            }).ToList();
            return postResponses;
        }

        public Task<List<Post>> GetAllPostsByStatusAsync(PostStatus status)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PostResponse>> GetAllPostsByUserAsync(Guid userId)
        {
            var posts = await _postRepository.GetAllPostsByUserAsync(userId);

            var postResponses = posts.Select(p => new PostResponse
            {
                PostId = p.PostId,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                Address = p.Address,
                Condition = p.Condition,
                DepositPolicy = p.DepositPolicy,
                Status = p.Status,
                User = p.User != null ? new UserResponse
                (
                    p.User.Id,
                    p.User.FirstName,
                    p.User.LastName,
                    p.User.PhoneNumber,
                    p.User.Image
                ) : null,
                ImageUrl = p.PostImages != null ? p.PostImages.Select(img => img.ImageUrl).ToList() : new List<string>(),
                Furnitures = null
            }).ToList();

            return postResponses;
        }

        public async Task<List<PostResponse>> GetAllPublishPostsByUserAsync(Guid userId)
        {
            var posts = await _postRepository.GetAllPostsByUserAsync(userId);
            var publishPosts = posts.Where(p => p.Status == PostStatus.Publish).ToList();

            var postResponses = publishPosts.Select(p => new PostResponse
            {
                PostId = p.PostId,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                Address = p.Address,
                Condition = p.Condition,
                DepositPolicy = p.DepositPolicy,
                Status = p.Status,
                User = p.User != null ? new UserResponse
                (
                    p.User.Id,
                    p.User.FirstName,
                    p.User.LastName,
                    p.User.PhoneNumber,
                    p.User.Image
                ) : null,
                ImageUrl = p.PostImages != null ? p.PostImages.Select(img => img.ImageUrl).ToList() : new List<string>(),
                Furnitures = null
            }).ToList();

            return postResponses;
        }

        public async Task<PostResponse?> GetPostByIdAsync(Guid id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null) return new PostResponse();

            var postResponse = new PostResponse
            {
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                Price = post.Price,
                Address = post.Address,
                Condition = post.Condition,
                DepositPolicy = post.DepositPolicy,
                Status = post.Status,
                User = post.User != null ? new UserResponse
                (
                    post.User.Id,
                    post.User.FirstName,
                    post.User.LastName,
                    post.User.PhoneNumber,
                    post.User.Image
                ) : null,
                ImageUrl = post.PostImages != null ? post.PostImages.Select(img => img.ImageUrl).ToList() : new List<string>(),
                Furnitures = post.Furnitures?.Select(f => new FurnitureResponse(f.FurId, f.Name)).ToList()
            };
            return postResponse;
        }

        public async Task<int> CreatePostAsync(Guid userId, PostRequest req)
        {
            var user = await _authRepository.GetUserByIdAsync(userId);
            if (user == null) return 0;

            var newPost = new Post
            {
                PostId = Guid.NewGuid(),
                Title = req.Title,
                Address = req.Address,
                Price = req.Price,
                Description = req.Description,
                Condition = req.Condition,
                DepositPolicy = req.DepositPolicy,
                Status = PostStatus.Pending,
                UserId = userId
            };

            //Upload Images here if any
            if (req.Images != null && req.Images.Any())
            {
                try
                {
                    var uploadResults = await _cloudinaryService.UploadMultipleImagesAsync(req.Images);

                    var postImages = uploadResults.Select((result, index) => new PostImage
                    {
                        PostId = newPost.PostId,
                        ImageUrl = result.Url
                    }).ToList();

                    newPost.PostImages = postImages;
                }
                catch (Exception ex)
                {
                    // Log the error
                    _logger.LogError($"Cloudinary upload error: {ex.Message}");
                }
            }
            
            //Add Furnitures if any
            if (req.FurnitureIds != null && req.FurnitureIds.Any())
            {
                var furnitures = await _furnitureRepository.GetFurnituresAsync();
                var selectedFurnitures = furnitures.Where(f => req.FurnitureIds.Contains(f.FurId)).ToList();
                newPost.Furnitures = selectedFurnitures;
            }

            _postRepository.CreatePostAsync(newPost);
            var result = await _postRepository.SaveChangesAsync();
            return result;
        }

        public async Task<int> UpdatePostAsync(PostRequest req, Guid postId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post == null) return 0;

            post.Title = req.Title;
            post.Address = req.Address;
            post.Price = req.Price;
            post.Description = req.Description;
            post.Condition = req.Condition;
            post.DepositPolicy = req.DepositPolicy;
            post.UpdatedAt = DateTime.UtcNow;

            _postRepository.UpdatePostAsync(post);
            return await _postRepository.SaveChangesAsync();
        }

        public async Task<int> UpdatePostStatusAsync(Guid postId, PostStatus status)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post == null) return 0;
            post.Status = status;
            post.UpdatedAt = DateTime.UtcNow;
            _postRepository.UpdatePostAsync(post);
            return await _postRepository.SaveChangesAsync();
        }

        public async Task<int> UpdateFurnitureInPostAsync(Guid postId, List<string> furnitureIds)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post == null) return 0;

            post.Furnitures.Clear();

            var furnitures = await _furnitureRepository.GetFurnituresAsync();

            var selectedFurnitures = furnitures.Where(f => furnitureIds.Contains(f.FurId)).ToList();
            post.Furnitures = selectedFurnitures;
            _postRepository.UpdatePostAsync(post);
            return await _postRepository.SaveChangesAsync();

        }
    }
}