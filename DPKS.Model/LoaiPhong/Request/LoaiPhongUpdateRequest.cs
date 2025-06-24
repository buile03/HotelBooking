using DPKS.Common.Result;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DPKS.Model.LoaiPhong.Request
{
    public class LoaiPhongUpdateRequest : UpdateRequestBase
    {
        [Required(ErrorMessage = "Vui lòng nhập tên loại phòng")]
        [StringLength(100, ErrorMessage = "Tên loại phòng không được vượt quá 100 ký tự")]
        public string Type { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập diện tích")]
        [Range(1, 1000, ErrorMessage = "Diện tích phải từ 1 đến 1000 m²")]
        public decimal? DienTich { get; set; }

        //[ImageFileValidation]
        public IFormFile? HinhAnhChinhFile { get; set; }

        public string? HinhAnhChinh { get; set; }
    }
}
