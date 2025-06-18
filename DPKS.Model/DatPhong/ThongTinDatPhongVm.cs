using DPKS.Common.Enum;

namespace DPKS.Model.DatPhong
{
    public class ThongTinDatPhongVm
    {
        public int Id { get; set; }
        public int PhongId { get; set; }
        public string SoPhong { get; set; }
        public int UserId { get; set; }
        public string TenKhachHang { get; set; }

        public DateTime NgayNhanPhong { get; set; }
        public DateTime NgayTraPhong { get; set; }
        public int SoDem { get; set; }
        public int SoLuongKhach { get; set; }

        public decimal TongTien { get; set; }
        public enTrangThaiDatPhong TrangThaiDatPhong { get; set; }

        public bool DaThanhToan { get; set; }
    }
}
