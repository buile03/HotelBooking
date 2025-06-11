using DPKS.Common.Enum;
using DPKS.Data.Entites;


namespace DPKS.Model.Phong
{
    public class ThongTinDanhSachPhongVm
    {
        public int PhongId { get; set; }
        public string SoPhong { get; set; } //Tên phòng
        public string Type { get; set; } // loại phòng
        public decimal Gia { get; set; } //Giá 1 đêm
        public int SoLuongKhach { get; set; }
        public List<string> TienNghi { get; set; }
        public enTrangThaiPhong TrangThaiPhong { get; set; }
        public string BinhLuan { get; set; }
        public int DanhGia { get; set; } // 1 - 5 *
        public List<string> AnhPhong { get; set; }
    }
}
