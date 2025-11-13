using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Responses;

namespace CoHabit.API.Services.Interfaces
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResponse> UploadImageAsync(IFormFile file);
        Task<List<ImageUploadResponse>> UploadMultipleImagesAsync(List<IFormFile> files);
        Task<bool> DeleteImageAsync(string publicId);
    }
}