using DPKS.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.LoaiPhong
{
    public class ThongTinPhongVm
    {
        public int PhongId { get; set; }
        public string SoPhong { get; set; } //Tên phòng
        public string Type { get; set; } // loại phòng
        public decimal Gia { get; set; } //Giá 1 đêm
        public enLoaiGiuong? LoaiGiuong { get; set; }
        public enLoaiView? LoaiView { get; set; }
        public enTrangThaiPhong TrangThaiPhong { get; set; }
        public int SoNguoiLonToiDa { get; set; }
        public int SoTreEmToiDa { get; set; }
        public int SoNguoiToiDa => SoNguoiLonToiDa + SoTreEmToiDa;

        public List<string> AnhPhong { get; set; } = new List<string>();
        public List<string> TienNghis { get; set; } = new List<string>();
        public List<string> LoaiPhong { get; set; } = new List<string>();


    }
}
