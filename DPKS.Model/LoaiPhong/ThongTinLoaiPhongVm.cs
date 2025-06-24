using DPKS.Common.Enum;
using DPKS.Model.TienNghi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.LoaiPhong
{
    public class ThongTinLoaiPhongVm
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string? Description {  get; set; }

        public int AvailableRooms { get; set; }
        public List<enLoaiGiuong> LoaiGiuong { get; set; } = new List<enLoaiGiuong>();
        public List<enLoaiView> LoaiView { get; set; } = new List<enLoaiView>();
        // Bổ sung thêm cho style mới
        public decimal DienTich { get; set; } // m²
        public string HinhAnhChinh { get; set; }
        public List<string> DanhSachHinhAnh { get; set; } = new List<string>();


        public int TongAnh { get; set; }              // => lp.anhLoaiPhongs.Count()
        public int TongTienNghi { get; set; }         // => lp.tienNghiTheoLoaiPhongs.Count()


        public int SoLuongPhong { get; set; } // Số lượng phòng thuộc loại này
        public int SoLuongPhongTrong { get; set; } // Số lượng phòng đang trống
        public decimal GiaThapNhat { get; set; } // Giá thấp nhất trong loại phòng này
        public decimal GiaCaoNhat { get; set; } // Giá cao nhất trong loại phòng này
        public List<TienNghiVm> TienNghis { get; set; }

        public List<ThongTinPhongVm> ThongTinPhongs { get; set; } = new();

    }
}
