using DPKS.Common.Enum;
using DPKS.Common.Result;

namespace DPKS.Model.Phong.Request
{
    public class PhongSearchRequest : PagingRequestBase
    {
        public string? Keyword { get; set; }
        
        public DateTime? NgayNhanPhong { get; set; }
        public DateTime? NgayTraPhong { get; set; }

        public decimal? GiaTu { get; set; }
        public decimal? GiaDen { get; set; }

        public int? LoaiPhongId { get; set; }

        public List<string> LoaiPhong { get; set; }
        public List<string> TienNghi { get; set; }
        public decimal Gia { get; set; }
        public int? SoLuongKhach { get; set; }

        public enTrangThaiPhong TrangThaiPhong { get; set; }
        public enLoaiGiuong? LoaiGiuong { get; set; }
        public enLoaiView? LoaiView { get; set; }
    }
}
