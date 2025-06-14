using System.ComponentModel;

namespace DPKS.Common.Enum
{
    public enum enLoaiThanhToan
    {
        [Description("Tiền mặt")]
        TienMat = 1,

        [Description("Chuyển khoản ngân hàng")]
        ChuyenKhoan = 2,

        [Description("Thẻ tín dụng / ghi nợ")]
        TheTinDung = 3,

        [Description("Ví điện tử (Momo, ZaloPay...)")]
        ViDienTu = 4,

        [Description("Cổng thanh toán (VNPay, OnePay...)")]
        CongThanhToan = 5
    }
}
