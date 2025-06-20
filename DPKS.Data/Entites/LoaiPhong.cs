using System.Collections.Generic;

namespace DPKS.Data.Entites
{
    public class LoaiPhong : BaseEntity
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string? Description { get; set; }

        public decimal DienTich { get; set; } // m²
        public string? HinhAnhChinh { get; set; } // ảnh đại diện chính

        //navigation properties
        public ICollection<Phong> phongs { get; set; } = new List<Phong>();
        public ICollection<TienNghi> tienNghis { get; set; } = new List<TienNghi>();
        public ICollection<AnhLoaiPhong> anhLoaiPhongs { get; set; } = new List<AnhLoaiPhong>();

        public ICollection<TienNghiTheoLoaiPhong> tienNghiTheoLoaiPhongs { get; set; } = new List<TienNghiTheoLoaiPhong>();
    }
}
