using DPKS.Common.Enum;
using System.Collections.Generic;

namespace DPKS.Data.Entites
{
    public class PhuongThucThanhToan : BaseEntity 
    { 
        public int Id { get; set; }
        public enLoaiThanhToan loaiThanhToan { get; set; }

        //navigation properties
        public bool IsActive { get; set; } = true;
        public ICollection<ThanhToan> thanhToans { get; set; } = new List<ThanhToan>();
    }
}
