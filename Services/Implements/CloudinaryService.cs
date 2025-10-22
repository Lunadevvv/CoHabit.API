
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Services.Interfaces;

namespace CoHabit.API.Services.Implements
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private static string CLOUDINARY_FOLDER = "Post_Images";
        public CloudinaryService(Cloudinary cloudinary)
        {

            _cloudinary = cloudinary;
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok";
        }

        public async Task<ImageUploadResponse> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty", nameof(file));
            }

            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Width(300).Height(400).Crop("pad")              // Thêm viền để vừa khung
                    .Background("auto").Quality("auto").FetchFormat("auto"),
                Folder = CLOUDINARY_FOLDER
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
            {
                throw new Exception($"Image upload failed: {uploadResult.Error.Message}");
            }
            
            return new ImageUploadResponse
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.Url.ToString(),
                SecureUrl = uploadResult.SecureUrl.ToString()
            };
        }

        public async Task<List<ImageUploadResponse>> UploadMultipleImagesAsync(List<IFormFile> files)
        {
            var uploadTasks = files.Select(UploadImageAsync);
            var results = await Task.WhenAll(uploadTasks);
            return results.ToList();
        }
    }
}