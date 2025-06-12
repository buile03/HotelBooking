using System.Collections.Generic;

namespace DPKS.Data.Entites
{
    public class TienNghi : BaseEntity
    {
        public int Id {  get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        //navigation properties

        public ICollection<LoaiPhong> loaiPhongs { get; set; } = new List<LoaiPhong>();
        public ICollection<TienNghiTheoLoaiPhong> tienNghiTheoLoaiPhongs { get; set; } = new List<TienNghiTheoLoaiPhong>();
        

    }
}
