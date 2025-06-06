using DPKS.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Data.Entites
{
    public class TrangThaiDatPhong : BaseEntity
    {
        public int Id { get; set; }
       
        public enTrangThaiDatPhong trangThaiDatPhong { get; set; }

        //navigation properties
        public ICollection<DatPhong> datPhongs = new List<DatPhong>();
    }
}
