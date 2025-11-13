using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace CoHabit.API.DTOs.Requests
{
    public class PostFeedbackRequest
    {
        [Required]
        public Guid PostId { get; set; }
        [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5 sao.")]
        [Required]
        public double Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}