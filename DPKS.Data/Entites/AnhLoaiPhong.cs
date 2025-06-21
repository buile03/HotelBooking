using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Data.Entites
{
    public class AnhLoaiPhong : BaseEntity
    {
        public int PhotoId { get; set; }
        public int LoaiPhongId { get; set; }
        public string PhotoName { get; set; }

        // navigation property
        public LoaiPhong LoaiPhong { get; set; }
    }
}
