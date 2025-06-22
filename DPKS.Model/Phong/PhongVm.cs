using DPKS.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.Phong
{
    public class PhongVm
    {
        public int PhongId { get; set; }
        [Required(ErrorMessage = "Số phòng là bắt buộc")]
        [Display(Name = "Số phòng")]
        public string SoPhong { get; set; }

        [Required(ErrorMessage = "Loại phòng là bắt buộc")]
        [Display(Name = "Loại phòng")]
        public int LoaiPhongId { get; set; }

        [Required(ErrorMessage = "Trạng thái phòng là bắt buộc")]
        [Display(Name = "Trạng thái phòng")]
        public int TrangThaiPhongId { get; set; }

        [Required(ErrorMessage = "Giá phòng là bắt buộc")]
        [Display(Name = "Giá (1 đêm)")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Gia { get; set; }

        [Display(Name = "Loại giường")]
        public enLoaiGiuong LoaiGiuong { get; set; }

        [Display(Name = "Loại view")]
        public enLoaiView LoaiView { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Số người lớn tối đa là bắt buộc")]
        [Display(Name = "Số người lớn tối đa")]
        [Range(1, int.MaxValue, ErrorMessage = "Số người lớn phải lớn hơn 0")]
        public int SoNguoiLonToiDa { get; set; }

        [Display(Name = "Số trẻ em tối đa")]
        [Range(0, int.MaxValue, ErrorMessage = "Số trẻ em không được âm")]
        public int SoTreEmToiDa { get; set; }

        // Navigation properties for display
        [Display(Name = "Tên loại phòng")]
        public string? TenLoaiPhong { get; set; }

        [Display(Name = "Trạng thái")]
        public string? TenTrangThai { get; set; }
    }
}
