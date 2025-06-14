using System.ComponentModel;

namespace DPKS.Common.Enum
{
    public enum enTrangThaiPhong
    {
        [Description("Trống")]
        TRONG = 1,

        [Description("Đã đặt")]
        DADAT = 2,

        [Description("Đang ở")]
        DANGO = 3,

        [Description("Đang dọn dẹp")]
        DANGDONDEP = 4,

        [Description("Bảo trì")]
        BAOTRI = 5,

        [Description("Không khả dụng")]
        KHONGKHADUNG = 6
    }
}
