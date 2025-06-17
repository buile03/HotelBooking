using DPKS.Common.Enum;
using DPKS.Data.Entites;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.User
{
    public class RegisterVm
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required]
        public int QuocGiaId { get; set; }

        [Required]
        public int TinhId { get; set; }
        public List<SelectListItem> DanhSachQuocGia = new();
        public List<SelectListItem> DanhSachTinh = new();
    }
}
