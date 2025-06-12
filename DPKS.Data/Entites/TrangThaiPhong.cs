using DPKS.Common.Enum;
using System.Collections.Generic;

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
