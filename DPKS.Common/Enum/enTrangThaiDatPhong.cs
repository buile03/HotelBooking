using System.ComponentModel;

namespace DPKS.Common.Enum
{
    public enum enTrangThaiDatPhong
    {
        [Description("Chờ xác nhận")]
        CHOXACNHAN = 0,

        [Description("Đã xác nhận")]
        DAXACNHAN = 1,

        [Description("Đã nhận phòng")]
        DANHANPHONG = 2,

        [Description("Đã trả phòng")]
        DATRAPHONG = 3,

        [Description("Đã hủy")]
        DAHUY = 4,

        [Description("Không đến")]
        KHONGDEN = 5,

        [Description("Thất bại")]
        THATBAI = 6
    }
}
