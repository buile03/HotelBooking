using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.Phong
{
    public class PhongLienQuanVm
    {
        public int PhongId { get; set; }
        public string SoPhong { get; set; }
        public decimal Gia { get; set; }
        public string AnhDaiDien { get; set; }
        public string Type { get; set; } // loại phòng
    }

}
