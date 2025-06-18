using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.ThanhToan
{
    public class ThanhToanVm
    {
        public int Id { get; set; }
        public int DatPhongId { get; set; }
        public int PhuongThucThanhToanId { get; set; }
        public string TenPhuongThucThanhToan { get; set; }
        public decimal Gia { get; set; }
        public DateTime ThoiDiemThanhToan { get; set; }
    }
}
