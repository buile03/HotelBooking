using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.LoaiPhong
{
    public class LoaiPhongDetailVm
    {
        public ThongTinLoaiPhongVm LoaiPhong { get; set; }
        public List<ThongTinPhongVm> Phongs { get; set; } = new List<ThongTinPhongVm>();
        public DateTime? NgayNhanPhong { get; set; }
        public DateTime? NgayTraPhong { get; set; }
        public int SoNguoi { get; set; } = 1;
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; } = "price_asc";
    }
}
