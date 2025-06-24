using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

public class ImageFileValidationAttribute : ValidationAttribute
{
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
    private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;

        // Trường hợp 1: Một file đơn
        if (value is IFormFile singleFile)
        {
            return ValidateFile(singleFile);
        }

        // Trường hợp 2: Danh sách file
        if (value is IEnumerable<IFormFile> fileList)
        {
            foreach (var file in fileList)
            {
                var result = ValidateFile(file);
                if (result != ValidationResult.Success)
                    return result;
            }
            return ValidationResult.Success;
        }

        // Không đúng định dạng
        return new ValidationResult("Dữ liệu upload không hợp lệ.");
    }

    private ValidationResult ValidateFile(IFormFile file)
    {
        if (file == null)
            return ValidationResult.Success;

        var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
        if (!_allowedExtensions.Contains(extension))
        {
            return new ValidationResult("Chỉ chấp nhận file ảnh (.jpg, .jpeg, .png, .gif)!");
        }

        if (file.Length > _maxFileSize)
        {
            return new ValidationResult("File ảnh không được vượt quá 5MB!");
        }

        return ValidationResult.Success;
    }
}
