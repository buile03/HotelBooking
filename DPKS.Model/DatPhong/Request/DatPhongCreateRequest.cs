using DPKS.Data.Entites;
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
        public int? PhuongThucThanhToanId { get; set; } 

        [Required]
        public DateTime NgayNhanPhong { get; set; }

        [Required]
        public DateTime NgayTraPhong { get; set; }

        [Required]
        [Range(1, 4, ErrorMessage = "Số lượng khách tối đa là 4")]
        public int SoLuongKhach { get; set; }

        public decimal TongTien { get; set; }
        public decimal Gia1Dem { get; set; }
    }

}
