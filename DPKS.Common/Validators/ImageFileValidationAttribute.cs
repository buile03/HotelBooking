using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Common.Validators
{
    public class ImageFileValidationAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB

        public override bool IsValid(object value)
        {
            if (value == null) return true; // Allow null for optional files

            if (!(value is IFormFile file)) return false;

            // Check file extension
            var extension = Path.GetExtension(file.FileName)?.ToLower();
            if (!_allowedExtensions.Contains(extension))
            {
                ErrorMessage = "Chỉ chấp nhận file ảnh (.jpg, .jpeg, .png, .gif)!";
                return false;
            }

            // Check file size
            if (file.Length > _maxFileSize)
            {
                ErrorMessage = "File ảnh không được vượt quá 5MB!";
                return false;
            }

            return true;
        }
    }
}
