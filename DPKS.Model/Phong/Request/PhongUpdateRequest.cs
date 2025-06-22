using DPKS.Common.Enum;
using DPKS.Common.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.Phong.Request
{
    public class PhongUpdateRequest : UpdateRequestBase
    {
        [Required(ErrorMessage = "Vui lòng nhập số phòng")]
        [Display(Name = "Số phòng")]
        public string SoPhong { get; set; }

        [Display(Name = "Giá phòng")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
        public decimal Gia { get; set; }

        [Display(Name = "Loại giường")]
        public enLoaiGiuong loaiGiuong { get; set; }

        [Display(Name = "Loại view")]
        public enLoaiView loaiView { get; set; }

        [Display(Name = "Loại phòng")]
        public int LoaiPhongId { get; set; }

        [Display(Name = "Trạng thái phòng")]
        public int TrangThaiPhongId { get; set; }

        [Display(Name = "Số người lớn tối đa")]
        public int SoNguoiLonToiDa { get; set; }

        [Display(Name = "Số trẻ em tối đa")]
        public int SoTreEmToiDa { get; set; }
    }
}
