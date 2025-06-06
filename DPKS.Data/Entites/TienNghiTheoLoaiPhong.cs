using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Data.Entites
{
    public class TienNghiTheoLoaiPhong
    {
        
        public int LoaiPhongId { get; set; }
        public int TienNghiId { get; set; }

        //Navigation Properties 

        public LoaiPhong LoaiPhong { get; set; }

        public TienNghi TienNghi { get; set; }
    }
}
