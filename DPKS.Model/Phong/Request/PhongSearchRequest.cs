using DPKS.Common.Enum;
using DPKS.Common.Result;

public class PhongSearchRequest : PagingRequestBase
{
    public string? Keyword { get; set; }
    public DateTime? NgayNhanPhong { get; set; }
    public DateTime? NgayTraPhong { get; set; }
    public decimal? GiaTu { get; set; }
    public decimal? GiaDen { get; set; }
    public int? LoaiPhongId { get; set; }
    public List<string> LoaiPhong { get; set; } = new List<string>();
    public List<string> TienNghi { get; set; } = new List<string>();
    public decimal Gia { get; set; }
    public int? SoLuongKhach { get; set; }
    public enTrangThaiPhong TrangThaiPhong { get; set; }
    public enLoaiGiuong? LoaiGiuong { get; set; }
    public enLoaiView? LoaiView { get; set; }
    public string? SortBy { get; set; }
}