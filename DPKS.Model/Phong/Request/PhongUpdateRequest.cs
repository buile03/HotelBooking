using DPKS.Common.Enum;
using DPKS.Common.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DPKS.Model.Phong.Request
{
    public class PhongUpdateRequest : UpdateRequestBase
    {
        [Required(ErrorMessage = "Vui lòng nhập số phòng")]
        public string SoPhong { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá phòng")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
        public decimal? Gia { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại giường")]
        public enLoaiGiuong? loaiGiuong { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại view")]
        public enLoaiView? loaiView { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại phòng")]
        public int? LoaiPhongId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái phòng")]
        public int? TrangThaiPhongId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số người lớn tối đa")]
        [Range(0, 20)]
        public int? SoNguoiLonToiDa { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số trẻ em tối đa")]
        [Range(0, 20)]
        public int? SoTreEmToiDa { get; set; }
    }
}
