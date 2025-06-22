using System.ComponentModel;

namespace DPKS.Common.Enum
{
    public enum enLoaiGiuong
    {
        [Description("Không rõ")]
        KHONGRO = 0,

        [Description("Giường đơn")]
        DON = 1,

        [Description("Giường đôi")]
        DOI = 2,

        [Description("Giường queen")]
        QUEEN = 3,

        [Description("Giường king")]
        KING = 4,

        [Description("Giường tầng")]
        TANG = 5,

        [Description("Nhiều giường")]
        NHIEU_GIUONG = 6,

        [Description("Giường sofa")]
        SOFA = 7
    }
}
