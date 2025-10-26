using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoHabit.API.DTOs.Requests
{
    public record RegisterRequest
    {
        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})\b$", 
        ErrorMessage = "Số điện thoại không hợp lệ. Phải bắt đầu bằng 03, 05, 07, 08, 09 và có 10 chữ số")]
        public required string Phone { get; set; }
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public required string Email { get; set; }
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public required string Password { get; set; }
    }
}