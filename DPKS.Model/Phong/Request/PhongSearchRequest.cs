using DPKS.Common.Enum;
using DPKS.Common.Result;
using DPKS.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.Phong.Request
{
    public class PhongSearchRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        
        public DateTime NgayNhanPhong { get; set; }
        public DateTime NgayTraPhong { get; set; }

        public List<string> LoaiPhong { get; set; }
        public List<string> TienNghi { get; set; }
        public decimal Gia { get; set; }
        public int SoLuongKhach { get; set; }

        public enTrangThaiPhong TrangThaiPhong { get; set; }
    }
}
