using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.User
{
    public class UpdateProfileVm
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        [RegularExpression("Nam|Nữ", ErrorMessage = "Giới tính không hợp lệ")]
        public string GioiTinh { get; set; }
        [Required]
        public DateTime? NgaySinh { get; set; }
        [Required]
        public string FullName { get; set; }

        [Required]
        public int QuocGiaId { get; set; }

        [Required]
        public int TinhId { get; set; }

        public string? PhotoName { get; set; }

        public string Email { get; set; }
    }
}
