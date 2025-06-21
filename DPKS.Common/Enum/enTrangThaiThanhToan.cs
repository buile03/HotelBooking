using System.ComponentModel;

namespace DPKS.Common.Enum
{

    public enum enTrangThaiThanhToan
    {
        [Description("Thành công")]
        ThanhCong = 0,

        [Description("Thất bại")]
        ThatBai = 1,

        [Description("Đang xử lý")]
        DangXuLy = 2
    }
}
