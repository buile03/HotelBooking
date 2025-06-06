using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Data.Entites
{
    public class ThanhToan : BaseEntity
    {
        public int Id { get; set; }
        public int DatPhongId {  get; set; }
        public int PhuongThucThanhToanId {  get; set; }


        public decimal Gia { get; set; }
        public DateTime ThoiDiemThanhToan { get; set; }


        //navigation properties 
        public PhuongThucThanhToan PhuongThucThanhToan { get; set;}
        public DatPhong DatPhong { get; set; }
    }
}
