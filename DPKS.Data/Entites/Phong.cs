using DPKS.Common.Enum;
using System.Collections.Generic;

namespace DPKS.Data.Entites
{
    public class Phong : BaseEntity
    {
        public int PhongId { get; set; }
        public string SoPhong { get; set; }
        public int LoaiPhongId { get; set; }
        public int TrangThaiPhongId { get; set; }
        public decimal Gia {  get; set; } //Giá 1 đêm
        public enLoaiGiuong loaiGiuong { get; set; }
        public enLoaiView loaiView { get; set; }
        public bool IsActive { get; set; } = true;

        //navigation properties
        public LoaiPhong LoaiPhong { get; set; }
        public TrangThaiPhong TrangThaiPhong { get; set; }
        public ICollection<DatPhong>? datPhongs { get; set; } = new List<DatPhong>();
        public ICollection<AnhPhong>? anhPhongs { get; set; } = new List<AnhPhong>();
        

    }
}
