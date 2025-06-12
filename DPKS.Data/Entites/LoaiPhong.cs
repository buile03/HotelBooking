using System.Collections.Generic;

namespace DPKS.Data.Entites
{
    public class LoaiPhong : BaseEntity
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string? Description { get; set; }

        //navigation properties
        public ICollection<Phong> phongs { get; set; } = new List<Phong>();
        public ICollection<TienNghi> tienNghis { get; set; } = new List<TienNghi>();

        public ICollection<TienNghiTheoLoaiPhong> tienNghiTheoLoaiPhongs { get; set; } = new List<TienNghiTheoLoaiPhong>();
    }
}
