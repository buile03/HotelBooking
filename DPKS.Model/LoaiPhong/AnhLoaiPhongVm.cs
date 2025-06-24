using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.LoaiPhong
{
    public class AnhLoaiPhongVm
    {
        public int PhotoId { get; set; }
        public string PhotoName { get; set; }
        public bool IsMainPhoto { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PhotoUrl => $"/images/loaiphong/{PhotoName}";
    }

}
