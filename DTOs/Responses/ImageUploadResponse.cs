using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Responses
{
    public class ImageUploadResponse
    {
        public string PublicId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string SecureUrl { get; set; } = string.Empty;
    }
}