using System.ComponentModel.DataAnnotations;

namespace DPKS.Model.DatPhong
{
    public class DatPhongCreateVm
    {
        public int PhongId { get; set; }

        public string SoPhong { get; set; }

        [Display(Name = "Ngày nhận phòng")]
        [DataType(DataType.Date)]
        public DateTime NgayNhanPhong { get; set; }

        [Display(Name = "Ngày trả phòng")]
        [DataType(DataType.Date)]
        public DateTime NgayTraPhong { get; set; }

        [Display(Name = "Số lượng khách")]
        [Range(1, 4, ErrorMessage = "Số lượng khách từ 1 đến 4")]
        public int SoLuongKhach { get; set; }

        public decimal Gia1Dem { get; set; }

        public decimal TongTien => Gia1Dem * (NgayTraPhong - NgayNhanPhong).Days;
    }
}
