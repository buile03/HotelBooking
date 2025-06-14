using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.Phong.Request
{
    public class CalculatePriceRequest
    {
        public int PhongId { get; set; }
        public string NgayNhanPhong { get; set; }
        public string NgayTraPhong { get; set; }
    }
}
