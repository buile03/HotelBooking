using DPKS.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Data.Entites
{
    public class TrangThaiPhong : BaseEntity
    {
        public int Id { get; set; }
        public enTrangThaiPhong trangThaiPhong { get; set; }
        public string? Description { get; set; }

        //Navigation Properties

        public ICollection<Phong> Phongs { get; set; } = new List<Phong>();
    }
}
