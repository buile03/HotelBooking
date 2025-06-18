using System.ComponentModel.DataAnnotations;

namespace DPKS.Model.DatPhong.Request
{
    public class DatPhongCreateRequest
    {
        [Required]
        public int PhongId { get; set; }
        public int UserId { get; set; }

        [Required]
        public DateTime NgayNhanPhong { get; set; }

        [Required]
        public DateTime NgayTraPhong { get; set; }

        [Required]
        [Range(1, 4, ErrorMessage = "Số lượng khách tối đa là 4")]
        public int SoLuongKhach { get; set; }

        public decimal TongTien { get; set; }
    }

}
