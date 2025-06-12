using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.Feedback.Request
{
    public class CreateFeedbackRequest
    {
        [Required(ErrorMessage = "UserId là bắt buộc")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "DatPhongId là bắt buộc")]
        public int DatPhongId { get; set; }

        [Required(ErrorMessage = "Đánh giá là bắt buộc")]
        [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5")]
        public int DanhGia { get; set; }

        [Required(ErrorMessage = "Bình luận là bắt buộc")]
        [StringLength(1000, ErrorMessage = "Bình luận không được vượt quá 1000 ký tự")]
        public string BinhLuan { get; set; }
    }
}
