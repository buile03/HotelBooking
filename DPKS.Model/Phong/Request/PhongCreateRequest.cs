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
    public class PhongCreateRequest : RequestBase
    {
        [Required(ErrorMessage = "Vui lòng nhập số phòng")]
        [Display(Name = "Số phòng")]
        public string SoPhong { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại phòng")]
        [Display(Name = "Loại phòng")]
        public int LoaiPhongId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái phòng")]
        [Display(Name = "Trạng thái phòng")]
        public int TrangThaiPhongId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá phòng")]
        [Display(Name = "Giá phòng")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phòng phải lớn hơn 0")]
        public decimal Gia { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại giường")]
        [Display(Name = "Loại giường")]
        public enLoaiGiuong LoaiGiuong { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại view")]
        [Display(Name = "Loại view")]
        public enLoaiView LoaiView { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số người lớn tối đa")]
        [Display(Name = "Số người lớn tối đa")]
        [Range(1, int.MaxValue, ErrorMessage = "Số người lớn tối đa phải lớn hơn 0")]
        public int SoNguoiLonToiDa { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số trẻ em tối đa")]
        [Display(Name = "Số trẻ em tối đa")]
        [Range(0, int.MaxValue, ErrorMessage = "Số trẻ em tối đa phải lớn hơn hoặc bằng 0")]
        public int SoTreEmToiDa { get; set; }
    }
}
