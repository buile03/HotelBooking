using System;
using System.Collections.Generic;

namespace DPKS.Data.Entites
{
    public class DatPhong : BaseEntity
    {
        public int Id {  get; set; }
        public int PhongId {  get; set; }
        public int UserId {  get; set; }
        public DateTime NgayDat { get; set; } = DateTime.Now;
        public DateTime NgayNhanPhong { get; set; }
        public DateTime NgayTraPhong {  get; set; }

        //Tối đa 4 khách
        public int SoLuongKhach {  get; set; }
        //Tối đa 31
        public int SoDem {  get; set; }

        public int TrangThaiDatPhongId { get; set; }
        public int TrangThaiPhongId { get; set; }

        public decimal TongTien { get; set; }

       //navigation propreties
        public TrangThaiDatPhong TrangThaiDatPhong {  get; set; }
        public Phong Phong {  get; set; }
        public TrangThaiPhong TrangThaiPhong { get; set; }
        public ThanhToan ThanhToan { get; set; }
        public ICollection<FeedBack> feedBacks { get; set; } = new List<FeedBack>();

        public ApplicationUser User { get; set; }


    }
}
