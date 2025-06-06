using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Data.Entites
{
    public class FeedBack : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DatPhongId { get; set; }
        public int DanhGia { get; set; } // tu 1 - 5
        public string BinhLuan {  get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;


        //navigation properties
        public ApplicationUser User { get; set; }
        public DatPhong DatPhong { get; set; }
    }
}
