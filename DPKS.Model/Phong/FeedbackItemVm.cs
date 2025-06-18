using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.Phong
{
    public class FeedbackItemVm
    {
        public string TenNguoiDung { get; set; }
        public int DanhGia { get; set; } // 1-5
        public string BinhLuan { get; set; }
        public DateTime Ngay { get; set; }
    }

}
