using System.ComponentModel;

namespace DPKS.Common.Enum
{
    public enum enLoaiView
    {
        [Description("Không có")]
        KHONGCO = 0,

        [Description("Biển")]
        BIEN = 1,

        [Description("Núi")]
        NUI = 2,

        [Description("Thành phố")]
        THANHPHO = 3,

        [Description("Hồ bơi")]
        HOBOI = 4,

        [Description("Sân vườn")]
        SANVUON = 5,

        [Description("Sông")]
        SONG = 6
    }
}
