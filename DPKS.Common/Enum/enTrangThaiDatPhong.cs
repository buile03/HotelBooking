namespace DPKS.Common.Enum
{
    public enum enTrangThaiDatPhong
    {
        /// <summary>Đang chờ xác nhận từ hệ thống hoặc lễ tân</summary>
        CHOXACNHAN = 0,

        /// <summary>Đã được xác nhận (phòng đã được giữ cho khách)</summary>
        DAXACNHAN = 1,

        /// <summary>Khách đã nhận phòng (check-in)</summary>
        DANHANPHONG = 2,

        /// <summary>Khách đã trả phòng (check-out)</summary>
        DATRAPHONG = 3,

        /// <summary>Khách hủy đơn đặt phòng</summary>
        DAHUY = 4,

        /// <summary>Không đến nhận phòng (no-show)</summary>
        KHONGDEN = 5,

        /// <summary>Thanh toán không thành công hoặc quá hạn</summary>
        THATBAI = 6
    }
}
