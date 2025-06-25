using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.LoaiPhong.Request
{
    public class LoaiPhongCreateRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập loại phòng")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string Description { get; set; }

        [Range(1, 1000, ErrorMessage = "Diện tích phải lớn hơn 0")]
        public decimal DienTich { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ảnh đại diện")]
        public IFormFile HinhAnhUpload { get; set; }


        //[BindNever]// bỏ qua khi binding (option)
        [ValidateNever]
        public string HinhAnhChinh { get; set; }
        public List<int> SelectedTienNghiIds { get; set; } = new();
    }
}
