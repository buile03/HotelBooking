using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.User
{
    public class PasswordVm
    {
        public class ChangePasswordVm
        {
            [Required]
            public int UserId { get; set; }

            [Required, DataType(DataType.Password)]
            public string CurrentPassword { get; set; }

            [Required, DataType(DataType.Password)]
            public string NewPassword { get; set; }
            [Required, DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }
        }
        public class ForgotPasswordVm
        {
            [Required(ErrorMessage = "Vui lòng nhập email.")]
            [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
            public string Email { get; set; }
        }
        public class ResetPasswordVm
        {
            [Required(ErrorMessage = "Vui lòng nhập email.")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập mã đặt lại.")]
            public string ResetCode { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
            [MinLength(6)]
            [DataType(DataType.Password)]
            public string NewPassword { get; set; }

            [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu.")]
            [DataType(DataType.Password)]
            [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không đúng.")]
            public string ConfirmPassword { get; set; }
        }

        //public class ForgotResetPasswordVm
        //{
        //    // Bước 1: Gửi mã
        //    [Required(ErrorMessage = "Vui lòng nhập email.")]
        //    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        //    public string Email { get; set; }

        //    // Bước 2: Đặt lại mật khẩu
        //    public bool IsResetStep { get; set; } // Cờ để biết đang ở bước nào

        //    [Required(ErrorMessage = "Vui lòng nhập mã xác nhận.")]
        //    public string? ResetCode { get; set; }

        //    [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
        //    [MinLength(6)]
        //    [DataType(DataType.Password)]
        //    public string? NewPassword { get; set; }

        //    [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu.")]
        //    [DataType(DataType.Password)]
        //    [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không đúng.")]
        //    public string? ConfirmPassword { get; set; }
        //}


    }

}
