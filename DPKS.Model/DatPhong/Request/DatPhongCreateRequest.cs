using System;
using System.ComponentModel.DataAnnotations;

namespace DPKS.Model.DatPhong.Request
{
    public class DatPhongCreateRequest
    {
        public int DatPhongId { get; set; }

        [Required]
        public int PhongId { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public int? PhuongThucThanhToanId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày nhận phòng")]
        public DateTime NgayNhanPhong { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày trả phòng")]
        public DateTime NgayTraPhong { get; set; }

        [Required]
        [Range(1, 4, ErrorMessage = "Số lượng khách tối đa là 4")]
        public int SoLuongKhach { get; set; }
        public decimal Gia1Dem { get; set; }
        public decimal TongTien { get; set; }

        //Thông tin người nhận

        [Required(ErrorMessage = "Vui lòng nhập họ tên:")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SDT { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        public string? GhiChu { get; set; }
    }
}
