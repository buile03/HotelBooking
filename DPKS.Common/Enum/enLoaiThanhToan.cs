namespace DPKS.Common.Enum
{
    public enum enLoaiThanhToan
    {
        /// <summary>Thanh toán tiền mặt</summary>
        TienMat = 1,

        /// <summary>Chuyển khoản ngân hàng</summary>
        ChuyenKhoan = 2,

        /// <summary>Thẻ tín dụng / ghi nợ</summary>
        TheTinDung = 3,

        /// <summary>Thanh toán qua ví điện tử (Momo, ZaloPay...)</summary>
        ViDienTu = 4,

        /// <summary>Thanh toán qua cổng trung gian (VNPay, OnePay...)</summary>
        CongThanhToan = 5
    }
}
